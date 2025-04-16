using System.Timers;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GlobalErpData.Uairango.Dto;
using WFA_UaiRango_Global.Services.Culinaria;
using WFA_UaiRango_Global.Services.Estabelecimentos;
using WFA_UaiRango_Global.Services.Login;
using WFA_UaiRango_Global.Services.FormasPagamento;
using Microsoft.AspNetCore.Mvc.Filters;
using WFA_UaiRango_Global.Services.Config;
using GlobalErpData.Dto.Uairango;
using System;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using WFA_UaiRango_Global.Services.Categoria;
using GlobalLib.Utils;
using WFA_UaiRango_Global.Services.CategoriaOpcao;

namespace WFA_UaiRango_Global
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer _timer;
        private System.Windows.Forms.Timer _uiTimer; // para atualizar o textBox1
        private DateTime _ultimaExecucao;
        private bool _executando = false;
        private bool _executandoGetRecorrente = false;
        private readonly GlobalErpFiscalBaseContext _db;
        private readonly ILogger<MainForm> _logger;

        private DateTime _proximaExecucao;
        private readonly TimeSpan _intervaloExecucao = TimeSpan.FromMinutes(9);

        #region Inject services
        private readonly ILoginService _loginService;
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly ICulinariaService _culinariaService;
        private readonly IFormasPagamentoService _formasPagamentoService;
        private readonly IConfigService _configService;

        private readonly ICategoriaService _categoriaService;
        private readonly ICategoriaOpcaoService _categoriaOpcaoService;
        #endregion

        private string iniFilePath;

        public MainForm(GlobalErpFiscalBaseContext db, ILogger<MainForm> logger,
        #region Inject Services
            ILoginService loginService,
            IEstabelecimentoService estabelecimentoService,
            ICulinariaService culinariaService,
            IFormasPagamentoService formasPagamentoService,
            IConfigService configService,
            ICategoriaService categoriaService,
            ICategoriaOpcaoService categoriaOpcaoService
        #endregion
            )
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            _logger = logger;
            _db = db;
            _logger.LogInformation("MainForm iniciado com sucesso.");
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            notifyIcon1.Visible = true;

            _ultimaExecucao = DateTime.Now;

            #region Inject Services
            _loginService = loginService;
            _culinariaService = culinariaService;
            _estabelecimentoService = estabelecimentoService;
            _formasPagamentoService = formasPagamentoService;
            _configService = configService;
            _categoriaService = categoriaService;
            _categoriaOpcaoService = categoriaOpcaoService;
            #endregion

            iniFilePath = Path.Combine(Application.StartupPath, "configuracao_integrador_uairango.ini");

            CarregarEstadoCheckBox();

            _proximaExecucao = DateTime.Now; // ou DateTime.Now.AddMinutes(30)

            _timer = new System.Timers.Timer(1000); // ticks a cada 1 seg. (exemplo)
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 1000;
            _uiTimer.Tick += UiTimer_Tick;
            _uiTimer.Start();
        }

        private void AdicionarLinhaRichTextBox(string texto)
        {
            if (richTextBox1.InvokeRequired)
            {
                // se estamos em outra thread, chamamos Invoke no controle (UI)
                richTextBox1.Invoke(new Action(() => AdicionarLinhaRichTextBox(texto)));
            }
            else
            {
                // se estamos na thread da UI, podemos atualizar direto
                if (richTextBox1.Text.Length > 0)
                    richTextBox1.AppendText(Environment.NewLine + texto);
                else
                    richTextBox1.AppendText(texto);
            }
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            bool ativo = checkBox1.Checked;
            if ((!_executando) && ativo)
            {
                _executando = true;
                Task.Run(async () =>
                {
                    try
                    {
                        await EnviarSomenteModificacao();

                        if (DateTime.Now >= _proximaExecucao)
                        {
                            _ultimaExecucao = DateTime.Now; // registra o momento em que começou

                            try
                            {
                                _executandoGetRecorrente = true;
                                await UairangoIntegrarAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Erro na integração: {ex.Message}", ex);
                            }
                            finally
                            {
                                _executandoGetRecorrente = false;
                                // terminou de executar, então configura o próximo horário
                                _proximaExecucao = DateTime.Now.Add(_intervaloExecucao);
                                _logger.LogInformation($"Próxima execução agendada para: {_proximaExecucao}");
                                AdicionarLinhaRichTextBox($"Próxima execução agendada para: {_proximaExecucao}");
                            }
                        }
                    }
                    finally
                    {
                        _executando = false; // marca como não executando
                    }
                });
            }
        }



        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.Text = $"Desativado...";
            }
            else if (_executandoGetRecorrente)
            {
                var tempo = DateTime.Now - _ultimaExecucao;
                textBox1.ForeColor = Color.Green;
                textBox1.Text = $"Executando há: {tempo:hh\\:mm\\:ss}";
            }
            else
            {
                var restante = _proximaExecucao - DateTime.Now;
                if (restante < TimeSpan.Zero)
                    restante = TimeSpan.Zero;

                textBox1.ForeColor = Color.Red;
                textBox1.Text = $"Próxima execução em: {restante:hh\\:mm\\:ss}";
            }
        }

        #region Core
        #region Enviar
        private async Task EnviarSomenteModificacao()
        {
            _logger.LogInformation($"ESM iniciando ({DateTime.Now})");
            AdicionarLinhaRichTextBox($"ESM iniciando ({DateTime.Now})");
            try
            {
                var ultimoLogin = await _db.UairangoTokens
                    .FromSqlRaw($@"select * from uairango_tokens order by id desc limit 1")
                    .FirstOrDefaultAsync();
                bool idadeDoTokenMenorQueUmDia =
                    ultimoLogin != null
                    && ultimoLogin.DataHoraGeracao.HasValue
                    && (DateTime.Now - ultimoLogin.DataHoraGeracao.Value).TotalHours < 24;
                if (idadeDoTokenMenorQueUmDia)
                {
                    var empresasComTokenVinculo = await _db.Empresas.FromSqlRaw($@"
                        select * from empresa s
                        where length(coalesce(s.uairango_token_vinculo, '')) > 0
                    ").ToListAsync();
                    if (empresasComTokenVinculo != null && empresasComTokenVinculo.Count > 0)
                    {
                        foreach (Empresa empresa in empresasComTokenVinculo)
                        {
                            if ((!string.IsNullOrEmpty(empresa.UairangoIdEstabelecimento))
                                && (empresa.UairangoVinculado ?? false)
                                && (empresa.UairangoIdEstabelecimento.Length > 0))
                            {
                                /**************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 *                                                *
                                 *                                                *
                                 *                                                *
                                 *         ITERACAO POR ESTABELECIMENTO           *
                                 *                                                *
                                 *                                                *
                                 *                                                *
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************
                                 **************************************************/
                                await EnviarFormasPagamento(ultimoLogin.TokenAcesso, empresa);
                                await EnviarConfiguracoes(empresa, ultimoLogin.TokenAcesso);
                                await EnviarCategorias(empresa, ultimoLogin.TokenAcesso);
                                await EnviarCategoriasOpcoes(empresa, ultimoLogin.TokenAcesso);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Nenhum estabelecimento encontrado");
                        AdicionarLinhaRichTextBox($"Nenhum estabelecimento encontrado ({DateTime.Now})");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar somente modificações: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar somente modificações ({DateTime.Now}): {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"ESM finalizando ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"ESM finalizando ({DateTime.Now})");
            }
        }

        private async Task EnviarCategoriasOpcoes(Empresa empresa, string tokenAcesso)
        {
            // Dicionário para guardar os IDs retornados da API para cada registro de opção.
            // Chave: o ID interno (por exemplo, item.Id) e valor: o ID da API.
            Dictionary<int, int> opcoesApiIds = new Dictionary<int, int>();

            try
            {
                _db.ChangeTracker.Clear();
                var opcoes = await _db.UairangoOpcoesCategoria.FromSqlRaw($@"
                    select * from uairango_opcoes_categoria
                    where unity = {empresa.Unity}
                    and coalesce(integrated,0) in (0,2)
                    and cd_grupo in (
                        select distinct cd_grupo from grupo_estoque 
                        where coalesce(uairango_id_categoria, 0) > 0
                    )
                ")
                .Include(g => g.CdGrupoNavigation)
                .ToListAsync();

                if (opcoes != null && opcoes.Count > 0)
                {
                    foreach (var item in opcoes)
                    {
                        if (item.UairangoIdOpcao.HasValue && item.UairangoIdOpcao > 0)
                        {
                            var responseStatusOpcao = await _categoriaOpcaoService.AlterarStatusOpcaoAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                item.UairangoIdOpcao.Value,
                                item.UairangoStatus ?? 0);

                            CategoriaOpcaoAlterarDto categoriaOpcaoAlterarDto = new CategoriaOpcaoAlterarDto
                            {
                                Nome = item.UairangoNome
                            };
                            var responseNomeOpcao = await _categoriaOpcaoService.AlterarOpcaoAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                item.CdGrupoNavigation.UairangoIdCategoria ?? 0,
                                item.UairangoIdOpcao.Value,
                                categoriaOpcaoAlterarDto);

                            if (!responseNomeOpcao || !responseStatusOpcao)
                            {
                                _logger.LogError($"Erro ao alterar opção da categoria: {item.CdGrupoNavigation.NmGrupo} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"Erro ao alterar opção da categoria: {item.CdGrupoNavigation.NmGrupo} ({DateTime.Now})");
                            }
                            else
                            {
                                opcoesApiIds[item.Id] = item.UairangoIdOpcao.Value;
                                item.Integrated = 1;
                                _db.UairangoOpcoesCategoria.Update(item);
                            }
                        }
                        else
                        {
                            // Caso o registro não possua ID, faz POST para criar a opção
                            CategoriaOpcaoNovoDto categoriaOpcaoNovo = new CategoriaOpcaoNovoDto
                            {
                                Nome = item.UairangoNome
                            };

                            var response = await _categoriaOpcaoService.CriarOpcaoAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                item.CdGrupoNavigation.UairangoIdCategoria ?? 0,
                                categoriaOpcaoNovo);

                            if (!response.HasValue)
                            {
                                _logger.LogError($"Erro ao criar opção da categoria: {item.CdGrupoNavigation.NmGrupo} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"Erro ao criar opção da categoria: {item.CdGrupoNavigation.NmGrupo} ({DateTime.Now})");
                            }
                            else
                            {
                                opcoesApiIds[item.Id] = response.Value;
                                item.UairangoIdOpcao = response;
                                item.UairangoCodigoOpcao = ""; // Atribua conforme sua lógica
                                item.Integrated = 1;
                                item.UairangoStatus = 1;
                                _db.UairangoOpcoesCategoria.Update(item);
                            }
                        }
                    }
                    // Tenta salvar com retry para tratar conflitos de concorrência
                    await SaveChangesWithRetryOpcao(opcoesApiIds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar categorias opções: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar categorias opções ({DateTime.Now}): {ex.Message}");
            }
        }

        private async Task SaveChangesWithRetryOpcao(Dictionary<int, int> opcoesApiIds)
        {
            bool salvo = false;
            int tentativas = 0;
            const int maxTentativas = 3;
            while (!salvo && tentativas < maxTentativas)
            {
                try
                {
                    await _db.SaveChangesAsync();
                    salvo = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tentativas++;
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is UairangoOpcoesCategorium opcao)
                        {
                            var currentValuesAsNoTracking = _db.UairangoOpcoesCategoria
                           .AsNoTracking()
                           .FirstOrDefault(x => x.Id == opcao.Id);
                            if (currentValuesAsNoTracking != null)
                            {
                                var databaseValues = entry.GetDatabaseValues();
                                if (databaseValues != null)
                                {
                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                                var entryNoTracking = _db.Entry(currentValuesAsNoTracking);
                                foreach (var property in entry.Metadata.GetProperties())
                                {
                                    var theOriginalValue = entry
                                        .Property(property.Name).OriginalValue;
                                    var otherUserValue = entryNoTracking
                                        .Property(property.Name).CurrentValue;
                                    var whatIWantedItToBe = entry
                                        .Property(property.Name).CurrentValue;

                                    if (property.Name == nameof(UairangoOpcoesCategorium.UairangoIdOpcao)
                                        || property.Name == nameof(UairangoOpcoesCategorium.UairangoCodigoOpcao)
                                        )
                                    {
                                        if (property.Name == nameof(UairangoOpcoesCategorium.UairangoIdOpcao))
                                        {
                                            if (opcoesApiIds.TryGetValue(opcao.Id, out int storedApiId))
                                                opcao.UairangoIdOpcao = storedApiId;
                                            else
                                                entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                        }
                                        else if (property.Name == nameof(UairangoOpcoesCategorium.UairangoCodigoOpcao))
                                            entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                    }
                                    else
                                    {
                                        entry.Property(property.Name).CurrentValue = otherUserValue;
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"[Categorias - Concurrency Exception] Opcao não encontrada: {opcao.Id} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"[Categorias - Concurrency Exception] Opcao não encontrada: {opcao.Id} ({DateTime.Now})");
                            }
                        }
                        else
                        {
                            await entry.ReloadAsync();
                        }
                    }
                }
            }
            if (!salvo)
            {
                throw new Exception("Não foi possível salvar as alterações de opções após várias tentativas.");
            }
        }


        private async Task EnviarCategorias(Empresa empresa, string tokenAcesso)
        {
            Dictionary<int, int> categoriasApiIds = new Dictionary<int, int>();
            Dictionary<int, int> opcoesApiIds = new Dictionary<int, int>();

            _db.ChangeTracker.Clear();
            try
            {
                var categorias = await _db.GrupoEstoques.FromSqlRaw($@"
                    select * from grupo_estoque g
                    where unity = {empresa.Unity}
                    and coalesce(integrated,0) in (0,2)
                    and cd_grupo in (
                        select distinct i.cd_grupo 
                        from grupo_estoque i
                        inner join uairango_empresa_categoria j on j.cd_grupo = i.cd_grupo
                        inner join uairango_opcoes_categoria k on k.cd_grupo = i.cd_grupo
                        where j.cd_empresa = {empresa.CdEmpresa}
                    )
                ")
                .Include(x => x.UairangoOpcoesCategoria)
                .Include(x => x.UairangoEmpresaCategoria)
                .ToListAsync();

                if (categorias != null && categorias.Count > 0)
                {
                    foreach (GrupoEstoque categoria in categorias)
                    {
                        // Caso a categoria não tenha o campo obrigatório configurado, pule-a
                        if ((!categoria.UairangoIdCulinaria.HasValue) || (categoria.UairangoIdCulinaria <= 0))
                        {
                            _logger.LogError($"Erro ao enviar categoria (IdCulinaria não informado): {categoria.NmGrupo} ({DateTime.Now})");
                            AdicionarLinhaRichTextBox($"Erro ao enviar categoria (IdCulinaria não informado): {categoria.NmGrupo} ({DateTime.Now})");
                            continue;
                        }

                        // Se a categoria já foi integrada (update) ou for nova (post), trate de forma diferente:
                        if ((categoria.UairangoIdCategoria ?? 0) > 0)
                        {
                            // -- Atualiza a categoria via PUT na API
                            CategoriaAlterarDto categoriaAlterarDto = new CategoriaAlterarDto
                            {
                                IdCulinaria = Convert.ToInt32(categoria.UairangoIdCulinaria),
                                Nome = categoria.NmGrupo,
                                Descricao = categoria.UairangoDescricao ?? "",
                                OpcaoMeia = categoria.UairangoOpcaoMeia ?? "",
                                Disponivel = new DisponibilidadeDto()
                                {
                                    Domingo = categoria.UairangoDisponivelDomingo ?? 0,
                                    Segunda = categoria.UairangoDisponivelSegunda ?? 0,
                                    Terca = categoria.UairangoDisponivelTerca ?? 0,
                                    Quarta = categoria.UairangoDisponivelQuarta ?? 0,
                                    Quinta = categoria.UairangoDisponivelQuinta ?? 0,
                                    Sexta = categoria.UairangoDisponivelSexta ?? 0,
                                    Sabado = categoria.UairangoDisponivelSabado ?? 0
                                },
                                Inicio = categoria.UairangoInicio?.ToString("HH:mm:ss") ?? "00:00:00",
                                Fim = categoria.UairangoFim?.ToString("HH:mm:ss") ?? "00:00:00"
                            };

                            // Chama os serviços de atualização de categoria e status
                            var response = await _categoriaService.AlterarCategoriaAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                categoria.UairangoIdCategoria.Value,
                                categoriaAlterarDto);
                            var responseStatus = await _categoriaService.AlterarStatusCategoriaAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                categoria.UairangoIdCategoria.Value,
                                categoria.UairangoAtivo ?? 0);

                            if ((!response) || (!responseStatus))
                            {
                                _logger.LogError($"Erro ao alterar categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"Erro ao alterar categoria: {categoria.NmGrupo} ({DateTime.Now})");
                            }
                            else
                            {
                                // Guarde o id retornado pela API (mesmo que seja o mesmo, é importante "reafirmar" o valor)
                                categoriasApiIds[categoria.CdGrupo] = categoria.UairangoIdCategoria.Value;
                                categoria.Integrated = 1;
                                _db.GrupoEstoques.Update(categoria);

                                // Para cada opção vinculada à categoria:
                                foreach (var item in categoria.UairangoOpcoesCategoria)
                                {
                                    if ((item.Integrated ?? 0) != 1)
                                    {
                                        if (item.UairangoIdOpcao.HasValue && item.UairangoIdOpcao > 0)
                                        {
                                            // Atualiza opção via PUT
                                            var responseStatusOpcao = await _categoriaOpcaoService.AlterarStatusOpcaoAsync(
                                                tokenAcesso,
                                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                                item.UairangoIdOpcao.Value,
                                                item.UairangoStatus ?? 0);
                                            CategoriaOpcaoAlterarDto categoriaOpcaoAlterarDto = new CategoriaOpcaoAlterarDto
                                            {
                                                Nome = item.UairangoNome
                                            };
                                            var responseNomeOpcao = await _categoriaOpcaoService.AlterarOpcaoAsync(
                                                tokenAcesso,
                                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                                categoria.UairangoIdCategoria.Value,
                                                item.UairangoIdOpcao.Value,
                                                categoriaOpcaoAlterarDto);
                                            if (!responseNomeOpcao || !responseStatusOpcao)
                                            {
                                                _logger.LogError($"Erro ao alterar opção da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                                AdicionarLinhaRichTextBox($"Erro ao alterar opção da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                            }
                                            else
                                            {
                                                // Armazena o id obtido para a opção na nossa estrutura auxiliar
                                                opcoesApiIds[item.Id] = item.UairangoIdOpcao.Value;
                                                item.Integrated = 1;
                                                _db.UairangoOpcoesCategoria.Update(item);
                                            }
                                        }
                                        else
                                        {
                                            // Caso não haja id na opção, faz POST para inserir a opção
                                            CategoriaOpcaoNovoDto categoriaOpcaoNovo = new CategoriaOpcaoNovoDto
                                            {
                                                Nome = item.UairangoNome
                                            };
                                            var responseOpcao = await _categoriaOpcaoService.CriarOpcaoAsync(
                                                tokenAcesso,
                                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                                categoria.UairangoIdCategoria.Value,
                                                categoriaOpcaoNovo);
                                            if (!responseOpcao.HasValue)
                                            {
                                                _logger.LogError($"Erro ao criar opção da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                                AdicionarLinhaRichTextBox($"Erro ao criar opção da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                            }
                                            else
                                            {
                                                opcoesApiIds[item.Id] = responseOpcao.Value;
                                                item.UairangoIdOpcao = responseOpcao;
                                                item.UairangoCodigoOpcao = ""; // ou qualquer outro valor
                                                item.Integrated = 1;
                                                item.UairangoStatus = 1;
                                                _db.UairangoOpcoesCategoria.Update(item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Caso seja nova: faz POST de categoria
                            CategoriaNovoDto categoriaNovo = new CategoriaNovoDto
                            {
                                IdCulinaria = Convert.ToInt32(categoria.UairangoIdCulinaria),
                                Nome = categoria.NmGrupo,
                                Opcoes = categoria.UairangoOpcoesCategoria.Select(x => x.UairangoNome).ToList() ?? new List<string>(),
                                Codigo = "GGU" + categoria.CdGrupo.ToString(),
                                Descricao = categoria.UairangoDescricao ?? "",
                                OpcaoMeia = categoria.UairangoOpcaoMeia ?? "",
                                Disponivel = new DisponibilidadeDto()
                                {
                                    Domingo = categoria.UairangoDisponivelDomingo ?? 0,
                                    Segunda = categoria.UairangoDisponivelSegunda ?? 0,
                                    Terca = categoria.UairangoDisponivelTerca ?? 0,
                                    Quarta = categoria.UairangoDisponivelQuarta ?? 0,
                                    Quinta = categoria.UairangoDisponivelQuinta ?? 0,
                                    Sexta = categoria.UairangoDisponivelSexta ?? 0,
                                    Sabado = categoria.UairangoDisponivelSabado ?? 0
                                },
                                Inicio = categoria.UairangoInicio?.ToString("HH:mm:ss") ?? "",
                                Fim = categoria.UairangoFim?.ToString("HH:mm:ss") ?? ""
                            };
                            var response = await _categoriaService.CriarCategoriaAsync(
                                tokenAcesso,
                                Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                categoriaNovo);
                            if (!response.HasValue)
                            {
                                _logger.LogError($"Erro ao criar categoria: {categoria.NmGrupo}");
                                AdicionarLinhaRichTextBox($"Erro ao criar categoria: {categoria.NmGrupo} ({DateTime.Now})");
                            }
                            else
                            {
                                categoriasApiIds[categoria.CdGrupo] = response.Value;
                                categoria.UairangoCodigo = "GGU" + categoria.CdGrupo.ToString();
                                categoria.UairangoIdCategoria = response;
                                categoria.Integrated = 1;
                                _db.GrupoEstoques.Update(categoria);

                                // Após criar a categoria, obtenha as opções já cadastradas na API para mesclar nos seus registros locais
                                var opcoesCategoriaUairango = await _categoriaOpcaoService.ObterOpcoesDaCategoriaAsync(
                                    tokenAcesso,
                                    Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                                    response.Value);
                                if (opcoesCategoriaUairango != null && opcoesCategoriaUairango.Count > 0)
                                {
                                    foreach (var item in categoria.UairangoOpcoesCategoria)
                                    {
                                        var opcoesUairango = opcoesCategoriaUairango.FirstOrDefault(x => x.Nome.Equals(item.UairangoNome));
                                        if (opcoesUairango != null)
                                        {
                                            opcoesApiIds[item.Id] = opcoesUairango.IdOpcao;
                                            item.UairangoIdOpcao = opcoesUairango.IdOpcao;
                                            item.UairangoCodigoOpcao = opcoesUairango.CodigoOpcao;
                                            item.UairangoStatus = opcoesUairango.Status;
                                            item.Integrated = 1;
                                            _db.UairangoOpcoesCategoria.Update(item);
                                        }
                                        else
                                        {
                                            _logger.LogError($"Erro ao obter opções da categoria: {categoria.NmGrupo}");
                                            AdicionarLinhaRichTextBox($"Erro ao obter opções da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                        }
                                    }
                                }
                                else
                                {
                                    _logger.LogError($"Erro ao obter opções da categoria: {categoria.NmGrupo}");
                                    AdicionarLinhaRichTextBox($"Erro ao obter opções da categoria: {categoria.NmGrupo} ({DateTime.Now})");
                                }
                            }
                        }
                    }

                    await CategoriasSaveChangesWithRetry(categoriasApiIds, opcoesApiIds);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("[Categorias] Concurrency Exception");
                AdicionarLinhaRichTextBox($"[Categorias] Concurrency Exception. ({DateTime.Now})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar categorias: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar categorias ({DateTime.Now}): {ex.Message}");
            }
        }

        private async Task CategoriasSaveChangesWithRetry(Dictionary<int, int> categoriasApiIds, Dictionary<int, int> opcoesApiIds)
        {
            bool salvo = false;
            int tentativas = 0;
            const int maxTentativas = 36;
            while (!salvo && tentativas < maxTentativas)
            {
                try
                {
                    await _db.SaveChangesAsync();
                    salvo = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tentativas++;
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is GrupoEstoque categoria)
                        {
                            var currentValuesAsNoTracking = _db.GrupoEstoques
                                .AsNoTracking()
                                .FirstOrDefault(x => x.CdGrupo == categoria.CdGrupo);
                            if (currentValuesAsNoTracking != null)
                            {
                                var databaseValues = entry.GetDatabaseValues();
                                if (databaseValues != null)
                                {
                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                                var entryNoTracking = _db.Entry(currentValuesAsNoTracking);
                                foreach (var property in entry.Metadata.GetProperties())
                                {
                                    var theOriginalValue = entry
                                        .Property(property.Name).OriginalValue;
                                    var otherUserValue = entryNoTracking
                                        .Property(property.Name).CurrentValue;
                                    var whatIWantedItToBe = entry
                                        .Property(property.Name).CurrentValue;
                                    if (property.Name == nameof(GrupoEstoque.UairangoIdCategoria) ||
                                        property.Name == nameof(GrupoEstoque.UairangoCodigo)
                                        )
                                    {
                                        if (property.Name == nameof(GrupoEstoque.UairangoIdCategoria))
                                        {
                                            if (categoriasApiIds.TryGetValue(categoria.CdGrupo, out int storedApiId))
                                                categoria.UairangoIdCategoria = storedApiId;
                                            else
                                                entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                        }
                                        else if (property.Name == nameof(GrupoEstoque.UairangoCodigo))
                                            entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                    }
                                    else
                                    {
                                        entry.Property(property.Name).CurrentValue = otherUserValue;
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"[Categorias - Concurrency Exception] Categoria não encontrada: {categoria.CdGrupo} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"[Categorias - Concurrency Exception] Categoria não encontrada: {categoria.CdGrupo} ({DateTime.Now})");
                            }
                        }
                        if (entry.Entity is UairangoOpcoesCategorium opcao)
                        {
                            var currentValuesAsNoTracking = _db.UairangoOpcoesCategoria
                           .AsNoTracking()
                           .FirstOrDefault(x => x.Id == opcao.Id);
                            if (currentValuesAsNoTracking != null)
                            {
                                var databaseValues = entry.GetDatabaseValues();
                                if (databaseValues != null)
                                {
                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                                var entryNoTracking = _db.Entry(currentValuesAsNoTracking);
                                foreach (var property in entry.Metadata.GetProperties())
                                {
                                    var theOriginalValue = entry
                                        .Property(property.Name).OriginalValue;
                                    var otherUserValue = entryNoTracking
                                        .Property(property.Name).CurrentValue;
                                    var whatIWantedItToBe = entry
                                        .Property(property.Name).CurrentValue;

                                    if (property.Name == nameof(UairangoOpcoesCategorium.UairangoIdOpcao)
                                        || property.Name == nameof(UairangoOpcoesCategorium.UairangoCodigoOpcao)
                                        )
                                    {
                                        if (property.Name == nameof(UairangoOpcoesCategorium.UairangoIdOpcao))
                                        {
                                            if (opcoesApiIds.TryGetValue(opcao.Id, out int storedApiId))
                                                opcao.UairangoIdOpcao = storedApiId;
                                            else
                                                entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                        }
                                        else if (property.Name == nameof(UairangoOpcoesCategorium.UairangoCodigoOpcao))
                                            entry.Property(property.Name).CurrentValue = whatIWantedItToBe;
                                    }
                                    else
                                    {
                                        entry.Property(property.Name).CurrentValue = otherUserValue;
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"[Categorias - Concurrency Exception] Opcao não encontrada: {opcao.Id} ({DateTime.Now})");
                                AdicionarLinhaRichTextBox($"[Categorias - Concurrency Exception] Opcao não encontrada: {opcao.Id} ({DateTime.Now})");
                            }
                        }
                        else
                        {
                            await entry.ReloadAsync();
                        }
                    }
                }
            }
            if (!salvo)
            {
                throw new Exception("Não foi possível salvar as alterações após várias tentativas.");
            }
        }


        private async Task EnviarConfiguracoes(Empresa empresa, string tokenAcesso)
        {
            try
            {
                _db.ChangeTracker.Clear();
                var configuracoes = await _db.UairangoConfiguracoes
                    .FromSqlRaw($@"
                    select * from uairango_configuracoes u
                    where cd_empresa = {empresa.CdEmpresa}
                    and unity = {empresa.Unity}
                    and (coalesce(integrated,0) in (0,2))
                    and chave in (
                        '{ChavesConfigUairango.STATUS_ESTABELECIMENTO}',
                        '{ChavesConfigUairango.STATUS_DELIVERY}',
                        '{ChavesConfigUairango.ID_TEMPO_DELIVERY}',
                        '{ChavesConfigUairango.STATUS_RETIRADA}',
                        '{ChavesConfigUairango.ID_TEMPO_RETIRADA}'
                    )
                ")
                    .ToListAsync();
                if (configuracoes != null && configuracoes.Count > 0)
                {
                    foreach (var item in configuracoes)
                    {
                        bool sucesso = await _configService.AtualizarEstabelecimentoAsync(
                         tokenAcesso,
                         Convert.ToInt32(empresa.UairangoIdEstabelecimento),
                         item.Chave,
                         item.Valor);
                        if (sucesso)
                        {
                            item.Integrated = 1;
                            _db.UairangoConfiguracoes.Update(item);
                        }
                    }
                    await _db.SaveChangesAsync();
                    _logger.LogInformation($"Configurações enviadas com sucesso ({DateTime.Now})");
                    AdicionarLinhaRichTextBox($"Configurações enviadas com sucesso ({DateTime.Now})");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("[Formas de pagamento] Alguns registros não foram atualizados pois foram modificados enquanto aguardavam integração.");
                AdicionarLinhaRichTextBox($"[Formas de pagamento] Aviso: Alguns registros foram modificados durante a integração e não foram marcados como integrados. ({DateTime.Now})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar configurações: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar configurações ({DateTime.Now}): {ex.Message}");
            }
        }

        private async Task EnviarFormasPagamento(string tokenAcesso, Empresa empresa)
        {
            _db.ChangeTracker.Clear();
            var allFormasPorEmpresa = await _db.UairangoFormasPagamentos
                .FromSqlRaw($@"
                    select * from uairango_formas_pagamento u
                    where empresa = {empresa.CdEmpresa}
                    and unity = {empresa.Unity}
                ")
                .ToListAsync();
            var formasDeliveryPrecisamEnviar = allFormasPorEmpresa
                .Where(x => x.TipoEntrega == "D" && (x.Integrated ?? 0) != 1)
                .ToList();

            var formasRetiradaPrecisamEnviar = allFormasPorEmpresa
                .Where(x => x.TipoEntrega == "R" && (x.Integrated ?? 0) != 1)
                .ToList();
            try
            {
                if (formasDeliveryPrecisamEnviar != null && formasDeliveryPrecisamEnviar.Count > 0)
                {
                    var formasDelivery = allFormasPorEmpresa
                        .Where(x => x.Ativo == 1 && x.TipoEntrega == "D")
                        .Select(x => Convert.ToInt32(x.IdFormaUairango))
                        .ToList();
                    if (formasDelivery.Count == 0)
                    {
                        AdicionarLinhaRichTextBox($"Você precisa informar pelo menos 1 forma de pagamento. Tipo de entrega (D) ({DateTime.Now})");
                        _logger.LogWarning($"Você precisa informar pelo menos 1 forma de pagamento. Tipo de entrega (D) ({DateTime.Now}) Empresa({empresa.CdEmpresa})");
                    }
                    else
                    {
                        var retorno = await _formasPagamentoService.AtualizarFormasPagamentoAsync(Convert.ToInt32(empresa.UairangoIdEstabelecimento), "D", formasDelivery, tokenAcesso);
                        if (retorno)
                        {
                            var _formasDelivery = allFormasPorEmpresa
                                .Where(x => x.TipoEntrega == "D")
                                .ToList();
                            foreach (var item in _formasDelivery)
                            {
                                item.Integrated = 1;
                                _db.UairangoFormasPagamentos.Update(item);
                            }
                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("[Formas de pagamento] Alguns registros não foram atualizados pois foram modificados enquanto aguardavam integração.");
                AdicionarLinhaRichTextBox($"[Formas de pagamento] Aviso: Alguns registros foram modificados durante a integração e não foram marcados como integrados. ({DateTime.Now})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar formas de pagamento (d): {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar formas de pagamento (d) ({DateTime.Now}): {ex.Message}");
            }
            try
            {
                if (formasRetiradaPrecisamEnviar != null && formasRetiradaPrecisamEnviar.Count > 0)
                {
                    var formasRetirada = allFormasPorEmpresa
                        .Where(x => x.Ativo == 1 && x.TipoEntrega == "R")
                        .Select(x => Convert.ToInt32(x.IdFormaUairango))
                        .ToList();
                    if (formasRetirada.Count == 0)
                    {
                        AdicionarLinhaRichTextBox($"Você precisa informar pelo menos 1 forma de pagamento. Tipo de entrega (R) ({DateTime.Now})");
                        _logger.LogWarning($"Você precisa informar pelo menos 1 forma de pagamento. Tipo de entrega (R) ({DateTime.Now}) Empresa({empresa.CdEmpresa})");
                    }
                    else
                    {
                        var retorno = await _formasPagamentoService.AtualizarFormasPagamentoAsync(Convert.ToInt32(empresa.UairangoIdEstabelecimento), "R", formasRetirada, tokenAcesso);
                        if (retorno)
                        {
                            var _formasRetirada = allFormasPorEmpresa
                                .Where(x => x.TipoEntrega == "R")
                                .ToList();
                            foreach (var item in _formasRetirada)
                            {
                                item.Integrated = 1;
                                _db.UairangoFormasPagamentos.Update(item);
                            }
                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("[Formas de pagamento] Alguns registros não foram atualizados pois foram modificados enquanto aguardavam integração.");
                AdicionarLinhaRichTextBox($"[Formas de pagamento] Aviso: Alguns registros foram modificados durante a integração e não foram marcados como integrados. ({DateTime.Now})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar formas de pagamento (r): {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar formas de pagamento (r) ({DateTime.Now}): {ex.Message}");
            }
        }
        #endregion
        #region Receber
        private async Task UairangoIntegrarAsync()
        {
            var ultimoLogin = await _db.UairangoTokens
                .OrderByDescending(t => t.DataHoraGeracao)
                .FirstOrDefaultAsync();
            bool idadeDoTokenMenorQueUmDia =
                ultimoLogin != null
                && ultimoLogin.DataHoraGeracao.HasValue
                && (DateTime.Now - ultimoLogin.DataHoraGeracao.Value).TotalHours < 24;

            string? token = ultimoLogin?.TokenAcesso;
            if (ultimoLogin == null || (!idadeDoTokenMenorQueUmDia))
            {
                LoginUaiRangoResponseDto login = await this._loginService.LoginAsync();
                token = login.Token;

                UairangoToken uairangoToken = new UairangoToken
                {
                    TokenAcesso = token,
                    DataHoraGeracao = DateTime.Now
                };

                await _db.UairangoTokens.AddAsync(uairangoToken);
                await _db.SaveChangesAsync();
            }

            if (token != null)
            {
                AdicionarLinhaRichTextBox($"Logado com sucesso ({DateTime.Now})");
                #region EntitidadesComunsATodosEstabelecimentos
                /**************************************************
                 **************************************************
                 **************************************************
                 **************************************************
                 **************************************************
                 **************************************************
                 *                                                *
                 *                                                *
                 *                                                *
                 *            Entitidades Comuns                  *
                 *                                                *
                 *                                                *
                 *                                                *
                 **************************************************
                 **************************************************
                 **************************************************
                 **************************************************
                 **************************************************
                 **************************************************/
                await GetCulinarias(token);
                await GetPrazos(token);
                #endregion
                await IterarEstabelecimento(token);
            }
            else
            {
                _logger.LogError("Erro desconhecido ao fazer login no UaiRango");
            }
        }

        private async Task GetPrazos(string token)
        {
            try
            {
                AdicionarLinhaRichTextBox($"Obtendo prazos ({DateTime.Now})");
                var prazos = await this._configService.ObterPrazosAsync(token);
                if (prazos != null)
                {
                    foreach (var prazoDto in prazos)
                    {
                        var prazoExistente = await _db.UairangoPrazos
                            .FirstOrDefaultAsync(c => (c.IdTempo ?? -1) == prazoDto.IdTempo);
                        if (prazoExistente != null)
                        {
                            prazoExistente.Min = prazoDto.Min;
                            prazoExistente.Max = prazoDto.Max;
                            prazoExistente.Status = prazoDto.Status;
                            _db.UairangoPrazos.Update(prazoExistente);
                        }
                        else
                        {
                            var novoPrazo = new UairangoPrazo
                            {
                                IdTempo = prazoDto.IdTempo,
                                Status = prazoDto.Status,
                                Min = prazoDto.Min,
                                Max = prazoDto.Max
                            };
                            await _db.UairangoPrazos.AddAsync(novoPrazo);
                        }
                    }
                    await _db.SaveChangesAsync();
                    AdicionarLinhaRichTextBox($"Prazos obtidos ({DateTime.Now})");
                }
                else
                {
                    _logger.LogError("Erro desconhecido ao obter prazos do UaiRango");
                    AdicionarLinhaRichTextBox($"Erro desconhecido ao obter prazos do UaiRango ({DateTime.Now})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter prazos: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao obter prazos ({DateTime.Now}): {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"Finalizando obtenção de prazos ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"Finalizando obtenção de prazos ({DateTime.Now})");
            }
        }



        private async Task GetCulinarias(string token)
        {
            AdicionarLinhaRichTextBox($"Obtendo culinarias ({DateTime.Now})");
            try
            {
                var culinarias = await this._culinariaService.ObterCulinariasAsync(token);
                if (culinarias != null)
                {
                    foreach (var culinariaDto in culinarias)
                    {
                        var culinariaExistente = await _db.UairangoCulinarias
                            .FirstOrDefaultAsync(c => c.IdCulinariaUairango.Equals(culinariaDto.IdCulinaria.ToString()));

                        if (culinariaExistente != null)
                        {
                            culinariaExistente.NmCulinaria = culinariaDto.Nome;
                            culinariaExistente.MeioMeio = culinariaDto.MeioMeio;
                            _db.UairangoCulinarias.Update(culinariaExistente);
                        }
                        else
                        {
                            var novaCulinaria = new UairangoCulinaria
                            {
                                IdCulinariaUairango = culinariaDto.IdCulinaria.ToString(),
                                NmCulinaria = culinariaDto.Nome,
                                MeioMeio = culinariaDto.MeioMeio
                            };
                            await _db.UairangoCulinarias.AddAsync(novaCulinaria);
                        }
                    }

                    await _db.SaveChangesAsync();
                    AdicionarLinhaRichTextBox($"Culinarias obtidas ({DateTime.Now})");
                }
                else
                {
                    _logger.LogError("Erro desconhecido ao obter culinárias do UaiRango");
                    AdicionarLinhaRichTextBox($"Erro desconhecido ao obter culinárias do UaiRango ({DateTime.Now})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter culinarias: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao obter culinárias do UaiRango ({DateTime.Now}): {ex.Message}");
            }
        }

        private async Task IterarEstabelecimento(string token)
        {
            AdicionarLinhaRichTextBox($"Iniciando iteracao por estabelecimentos ({DateTime.Now})");
            try
            {
                var empresasComTokenVinculo = await _db.Empresas.FromSqlRaw($@"
                    select * from empresa s
                    where length(coalesce(s.uairango_token_vinculo, '')) > 0
                    ").ToListAsync();
                if (empresasComTokenVinculo != null && empresasComTokenVinculo.Count > 0)
                {
                    List<Estabelecimento>? lEstabelecimentos = null;
                    foreach (Empresa empresa in empresasComTokenVinculo)
                    {
                        if ((!(empresa.UairangoVinculado ?? false)) ||
                            string.IsNullOrEmpty(empresa.UairangoIdEstabelecimento))
                        {
                            await VincularEstabelecimentoDeveloper(empresa, token);
                        }
                        #region Core_Iteracao_Estabelecimento
                        if ((!string.IsNullOrEmpty(empresa.UairangoIdEstabelecimento))
                            && (empresa.UairangoVinculado ?? false)
                            && (empresa.UairangoIdEstabelecimento.Length > 0))
                        {
                            /**************************************************
                             **************************************************
                             **************************************************
                             **************************************************
                             **************************************************
                             **************************************************
                             *                                                *
                             *                                                *
                             *                                                *
                             *         ITERACAO POR ESTABELECIMENTO           *
                             *                                                *
                             *                                                *
                             *                                                *
                             **************************************************
                             **************************************************
                             **************************************************
                             **************************************************
                             **************************************************
                             **************************************************/
                            await ReceberFormasPagamento(empresa, token);
                            await ReceberConfigEstabelecimento(empresa, token);
                            await ReceberCategorias(empresa, token);


                        }
                        else if (string.IsNullOrEmpty(empresa.UairangoIdEstabelecimento) &&
                            (empresa.UairangoVinculado ?? false))
                        {
                            _logger.LogWarning($"Estabelecimento sem IdUairango porem vinculado: {empresa.NmEmpresa}");
                            AdicionarLinhaRichTextBox($"Estabelecimento sem IdUairango porem vinculado: {empresa.NmEmpresa} ({DateTime.Now})");
                        }
                        #endregion
                    }
                }
                else
                {
                    _logger.LogWarning("Nenhum estabelecimento encontrado");
                    AdicionarLinhaRichTextBox($"Nenhum estabelecimento encontrado ({DateTime.Now})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao iterar estabelecimentos: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao iterar estabelecimentos ({DateTime.Now}): {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"Finalizando iteracao por estabelecimentos ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"Finalizando iteracao por estabelecimentos ({DateTime.Now})");
            }

        }

        private async Task ReceberCategorias(Empresa empresa, string token)
        {
            try
            {
                var categoriasUairango = await this._categoriaService
                    .ObterCategoriasAsync(token, Convert.ToInt32(empresa.UairangoIdEstabelecimento));

                if (categoriasUairango != null && categoriasUairango.Count > 0)
                {
                    var todasCategoriasExistentes = await _db.GrupoEstoques.FromSqlRaw($@"
                        select * from grupo_estoque where cd_grupo IN (
                            select distinct g.cd_grupo from grupo_estoque g
                            inner join uairango_empresa_categoria ec on ec.cd_grupo = g.cd_grupo
                            inner join empresa e on e.cd_empresa = ec.cd_empresa
                            where e.cd_empresa = {empresa.CdEmpresa}
                            and coalesce(g.uairango_id_categoria, 0) > 0
                        )
                    ")
                        .Include(x => x.UairangoOpcoesCategoria)
                        .ToListAsync();

                    foreach (var _categoriaUairango in categoriasUairango)
                    {
                        var categoriaEspecifica = todasCategoriasExistentes?
                            .FirstOrDefault(x => x.UairangoIdCategoria == _categoriaUairango.IdCategoria);

                        if (categoriaEspecifica != null)
                        {
                            if ((categoriaEspecifica.Integrated ?? 0) == 1)
                            {
                                categoriaEspecifica.NmGrupo = _categoriaUairango.Nome;
                                categoriaEspecifica.UairangoIdCategoria = _categoriaUairango.IdCategoria;
                                categoriaEspecifica.UairangoOrder = "";
                                categoriaEspecifica.UairangoCodigo = _categoriaUairango.Codigo;
                                categoriaEspecifica.UairangoDescricao = _categoriaUairango.Descricao;
                                categoriaEspecifica.UairangoInicio = DateUtils.StringFormatHHMMSSToTimeOnly(_categoriaUairango.Inicio);
                                categoriaEspecifica.UairangoFim = DateUtils.StringFormatHHMMSSToTimeOnly(_categoriaUairango.Fim);
                                categoriaEspecifica.UairangoAtivo = _categoriaUairango.Ativo;
                                categoriaEspecifica.UairangoOpcaoMeia = _categoriaUairango.OpcaoMeia;
                                categoriaEspecifica.UairangoDisponivelDomingo = _categoriaUairango.Disponivel.Domingo;
                                categoriaEspecifica.UairangoDisponivelSegunda = _categoriaUairango.Disponivel.Segunda;
                                categoriaEspecifica.UairangoDisponivelTerca = _categoriaUairango.Disponivel.Terca;
                                categoriaEspecifica.UairangoDisponivelQuarta = _categoriaUairango.Disponivel.Quarta;
                                categoriaEspecifica.UairangoDisponivelQuinta = _categoriaUairango.Disponivel.Quinta;
                                categoriaEspecifica.UairangoDisponivelSexta = _categoriaUairango.Disponivel.Sexta;
                                categoriaEspecifica.UairangoDisponivelSabado = _categoriaUairango.Disponivel.Sabado;
                                categoriaEspecifica.Integrated = 1;
                                _db.GrupoEstoques.Update(categoriaEspecifica);
                            }

                            var opcoesExistentes = categoriaEspecifica.UairangoOpcoesCategoria;
                            var opcoesUairango = await this._categoriaOpcaoService
                                .ObterOpcoesDaCategoriaAsync(token, Convert.ToInt32(empresa.UairangoIdEstabelecimento), _categoriaUairango.IdCategoria);

                            if (opcoesExistentes != null && opcoesExistentes.Count > 0)
                            {
                                foreach (var opcao in opcoesExistentes)
                                {
                                    var opcaoUairango = opcoesUairango
                                        .FirstOrDefault(x => x.IdOpcao == opcao.UairangoIdOpcao);
                                    if (opcaoUairango != null)
                                    {
                                        opcao.UairangoNome = opcaoUairango.Nome;
                                        opcao.UairangoStatus = opcaoUairango.Status;
                                        opcao.UairangoCodigoOpcao = opcaoUairango.CodigoOpcao;
                                        opcao.Integrated = 1;
                                        _db.UairangoOpcoesCategoria.Update(opcao);
                                    }
                                    else
                                    {
                                        _db.UairangoOpcoesCategoria.Remove(opcao);
                                    }
                                }
                            }
                            else
                            {
                                List<UairangoOpcoesCategorium> opcoes = new List<UairangoOpcoesCategorium>();
                                if (opcoesUairango != null && opcoesUairango.Count > 0)
                                {
                                    foreach (var opcao in opcoesUairango)
                                    {
                                        UairangoOpcoesCategorium opcaoCategoria = new UairangoOpcoesCategorium
                                        {
                                            UairangoIdCategoria = _categoriaUairango.IdCategoria,
                                            UairangoIdOpcao = opcao.IdOpcao,
                                            UairangoNome = opcao.Nome,
                                            UairangoStatus = opcao.Status,
                                            UairangoCodigoOpcao = opcao.CodigoOpcao,
                                            Integrated = 1,
                                            Unity = empresa.Unity,
                                        };
                                        opcoes.Add(opcaoCategoria);
                                    }
                                }
                                categoriaEspecifica.UairangoOpcoesCategoria = opcoes;
                            }

                        }
                        else
                        {
                            GrupoEstoque grupo = new GrupoEstoque
                            {
                                NmGrupo = _categoriaUairango.Nome,
                                UairangoIdCategoria = _categoriaUairango.IdCategoria,
                                UairangoOrder = "",
                                UairangoCodigo = _categoriaUairango.Codigo,
                                UairangoDescricao = _categoriaUairango.Descricao,
                                UairangoInicio = DateUtils.StringFormatHHMMSSToTimeOnly(_categoriaUairango.Inicio),
                                UairangoFim = DateUtils.StringFormatHHMMSSToTimeOnly(_categoriaUairango.Fim),
                                UairangoAtivo = _categoriaUairango.Ativo,
                                UairangoOpcaoMeia = _categoriaUairango.OpcaoMeia,
                                UairangoDisponivelDomingo = _categoriaUairango.Disponivel.Domingo,
                                UairangoDisponivelSegunda = _categoriaUairango.Disponivel.Segunda,
                                UairangoDisponivelTerca = _categoriaUairango.Disponivel.Terca,
                                UairangoDisponivelQuarta = _categoriaUairango.Disponivel.Quarta,
                                UairangoDisponivelQuinta = _categoriaUairango.Disponivel.Quinta,
                                UairangoDisponivelSexta = _categoriaUairango.Disponivel.Sexta,
                                UairangoDisponivelSabado = _categoriaUairango.Disponivel.Sabado,
                                Integrated = 1,
                                Unity = empresa.Unity
                            };

                            UairangoEmpresaCategorium associacao = new UairangoEmpresaCategorium
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Integrated = 1
                            };
                            grupo.UairangoEmpresaCategoria.Add(associacao);

                            List<UairangoOpcoesCategorium> opcoes = new List<UairangoOpcoesCategorium>();

                            var opcoesApi = await this._categoriaOpcaoService
                                .ObterOpcoesDaCategoriaAsync(token, Convert.ToInt32(empresa.UairangoIdEstabelecimento), _categoriaUairango.IdCategoria);
                            if (opcoesApi != null && opcoesApi.Count > 0)
                            {
                                foreach (var opcao in opcoesApi)
                                {
                                    UairangoOpcoesCategorium opcaoCategoria = new UairangoOpcoesCategorium
                                    {
                                        UairangoIdCategoria = _categoriaUairango.IdCategoria,
                                        UairangoIdOpcao = opcao.IdOpcao,
                                        UairangoNome = opcao.Nome,
                                        UairangoStatus = opcao.Status,
                                        UairangoCodigoOpcao = opcao.CodigoOpcao,
                                        Integrated = 1,
                                        Unity = empresa.Unity,
                                    };
                                    opcoes.Add(opcaoCategoria);
                                }
                            }
                            grupo.UairangoOpcoesCategoria = opcoes;

                            _db.GrupoEstoques.Add(grupo);
                        }
                    }

                    await _db.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("[Categorias] Concurrency Exception");
                AdicionarLinhaRichTextBox($"[Categorias] Concurrency Exception. ({DateTime.Now})");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao receber categorias: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao receber categorias ({DateTime.Now}): {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"Finalizando recebimento de categorias ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"Finalizando recebimento de categorias ({DateTime.Now})");
            }
        }

        private async Task ReceberConfigEstabelecimento(Empresa empresa, string token)
        {
            try
            {
                var config = await this._configService.ObterEstabelecimentoAsync(token,
                    Convert.ToInt32(empresa.UairangoIdEstabelecimento));
                if (config != null)
                {
                    var listagemConfiguracoes = await _db.UairangoConfiguracoes
                        .Where(c => c.CdEmpresa == empresa.CdEmpresa
                        && empresa.Unity == c.Unity).ToListAsync();
                    if (listagemConfiguracoes == null || listagemConfiguracoes.Count <= 0)
                    {
                        List<UairangoConfiguraco> novaConfig = new List<UairangoConfiguraco>();
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.STATUS_ESTABELECIMENTO,
                            Valor = config.StatusEstabelecimento.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.STATUS_DELIVERY,
                            Valor = config.StatusDelivery.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.ID_TEMPO_DELIVERY,
                            Valor = config.IdTempoDelivery.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.PRAZO_DELIVERY,
                            Valor = config.PrazoDelivery.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.STATUS_RETIRADA,
                            Valor = config.StatusRetirada.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.ID_TEMPO_RETIRADA,
                            Valor = config.IdTempoRetirada.ToString(),
                            Integrated = 1
                        });
                        novaConfig.Add(item: new UairangoConfiguraco
                        {
                            CdEmpresa = empresa.CdEmpresa,
                            Unity = empresa.Unity,
                            Chave = ChavesConfigUairango.PRAZO_RETIRADA,
                            Valor = config.PrazoRetirada.ToString(),
                            Integrated = 1
                        });
                        await _db.UairangoConfiguracoes.AddRangeAsync(novaConfig);

                    }
                    else
                    {
                        UairangoConfiguraco? uairangoConfiguraco_STATUS_ESTABELECIMENTO =
                            listagemConfiguracoes.Where(X => X.Chave ==
                            ChavesConfigUairango.STATUS_ESTABELECIMENTO).FirstOrDefault();
                        if (uairangoConfiguraco_STATUS_ESTABELECIMENTO != null)
                        {
                            if ((uairangoConfiguraco_STATUS_ESTABELECIMENTO.Integrated ?? 0) == 1)
                            {
                                uairangoConfiguraco_STATUS_ESTABELECIMENTO.Valor = config.StatusEstabelecimento.ToString();
                                uairangoConfiguraco_STATUS_ESTABELECIMENTO.Integrated = 1;
                                _db.UairangoConfiguracoes.Update(uairangoConfiguraco_STATUS_ESTABELECIMENTO);
                            }
                        }
                        else
                        {
                            await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Chave = ChavesConfigUairango.STATUS_ESTABELECIMENTO,
                                Valor = config.StatusEstabelecimento.ToString(),
                                Integrated = 1
                            });
                        }
                        UairangoConfiguraco? uairangoConfiguraco_STATUS_DELIVERY =
                            listagemConfiguracoes.Where(X => X.Chave ==
                            ChavesConfigUairango.STATUS_DELIVERY).FirstOrDefault();
                        if (uairangoConfiguraco_STATUS_DELIVERY != null)
                        {
                            if ((uairangoConfiguraco_STATUS_DELIVERY.Integrated ?? 0) == 1)
                            {
                                uairangoConfiguraco_STATUS_DELIVERY.Valor = config.StatusDelivery.ToString();
                                uairangoConfiguraco_STATUS_DELIVERY.Integrated = 1;
                                _db.UairangoConfiguracoes.Update(uairangoConfiguraco_STATUS_DELIVERY);
                            }
                        }
                        else
                        {
                            await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Chave = ChavesConfigUairango.STATUS_DELIVERY,
                                Valor = config.StatusDelivery.ToString(),
                                Integrated = 1
                            });
                        }
                        UairangoConfiguraco? uairangoConfiguraco_ID_TEMPO_DELIVERY =
                            listagemConfiguracoes.Where(X => X.Chave ==
                            ChavesConfigUairango.ID_TEMPO_DELIVERY).FirstOrDefault();
                        if (uairangoConfiguraco_ID_TEMPO_DELIVERY != null)
                        {
                            if ((uairangoConfiguraco_ID_TEMPO_DELIVERY.Integrated ?? 0) == 1)
                            {
                                uairangoConfiguraco_ID_TEMPO_DELIVERY.Valor = config.IdTempoDelivery.ToString();
                                uairangoConfiguraco_ID_TEMPO_DELIVERY.Integrated = 1;
                                _db.UairangoConfiguracoes.Update(uairangoConfiguraco_ID_TEMPO_DELIVERY);
                            }
                        }
                        else
                        {
                            await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Chave = ChavesConfigUairango.ID_TEMPO_DELIVERY,
                                Valor = config.IdTempoDelivery.ToString(),
                                Integrated = 1
                            });
                        }
                        //UairangoConfiguraco? uairangoConfiguraco_PRAZO_DELIVERY =
                        //    listagemConfiguracoes.Where(X => X.Chave ==
                        //    ChavesConfigUairango.PRAZO_DELIVERY).FirstOrDefault();
                        //if (uairangoConfiguraco_PRAZO_DELIVERY != null)
                        //{
                        //    if ((uairangoConfiguraco_PRAZO_DELIVERY.Integrated ?? 0) == 1)
                        //    {
                        //        uairangoConfiguraco_PRAZO_DELIVERY.Valor = config.PrazoDelivery.ToString();
                        //        uairangoConfiguraco_PRAZO_DELIVERY.Integrated = 1;
                        //        _db.UairangoConfiguracoes.Update(uairangoConfiguraco_PRAZO_DELIVERY);
                        //    }
                        //}
                        //else
                        //{
                        //    await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                        //    {
                        //        CdEmpresa = empresa.CdEmpresa,
                        //        Unity = empresa.Unity,
                        //        Chave = ChavesConfigUairango.PRAZO_DELIVERY,
                        //        Valor = config.PrazoDelivery.ToString(),
                        //        Integrated = 1
                        //    });
                        //}
                        UairangoConfiguraco? uairangoConfiguraco_STATUS_RETIRADA =
                            listagemConfiguracoes.Where(X => X.Chave ==
                            ChavesConfigUairango.STATUS_RETIRADA).FirstOrDefault();
                        if (uairangoConfiguraco_STATUS_RETIRADA != null)
                        {
                            if ((uairangoConfiguraco_STATUS_RETIRADA.Integrated ?? 0) == 1)
                            {
                                uairangoConfiguraco_STATUS_RETIRADA.Valor = config.StatusRetirada.ToString();
                                uairangoConfiguraco_STATUS_RETIRADA.Integrated = 1;
                                _db.UairangoConfiguracoes.Update(uairangoConfiguraco_STATUS_RETIRADA);
                            }
                        }
                        else
                        {
                            await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Chave = ChavesConfigUairango.STATUS_RETIRADA,
                                Valor = config.StatusRetirada.ToString(),
                                Integrated = 1
                            });
                        }
                        UairangoConfiguraco? uairangoConfiguraco_ID_TEMPO_RETIRADA =
                            listagemConfiguracoes.Where(X => X.Chave ==
                            ChavesConfigUairango.ID_TEMPO_RETIRADA).FirstOrDefault();
                        if (uairangoConfiguraco_ID_TEMPO_RETIRADA != null)
                        {
                            if ((uairangoConfiguraco_ID_TEMPO_RETIRADA.Integrated ?? 0) == 1)
                            {
                                uairangoConfiguraco_ID_TEMPO_RETIRADA.Valor = config.IdTempoRetirada.ToString();
                                uairangoConfiguraco_ID_TEMPO_RETIRADA.Integrated = 1;
                                _db.UairangoConfiguracoes.Update(uairangoConfiguraco_ID_TEMPO_RETIRADA);
                            }
                        }
                        else
                        {
                            await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                            {
                                CdEmpresa = empresa.CdEmpresa,
                                Unity = empresa.Unity,
                                Chave = ChavesConfigUairango.ID_TEMPO_RETIRADA,
                                Valor = config.IdTempoRetirada.ToString(),
                                Integrated = 1
                            });
                        }
                        //UairangoConfiguraco? uairangoConfiguraco_PRAZO_RETIRADA =
                        //    listagemConfiguracoes.Where(X => X.Chave ==
                        //    ChavesConfigUairango.PRAZO_RETIRADA).FirstOrDefault();
                        //if (uairangoConfiguraco_PRAZO_RETIRADA != null)
                        //{
                        //    if ((uairangoConfiguraco_PRAZO_RETIRADA.Integrated ?? 0) == 1)
                        //    {
                        //        uairangoConfiguraco_PRAZO_RETIRADA.Valor = config.PrazoRetirada.ToString();
                        //        uairangoConfiguraco_PRAZO_RETIRADA.Integrated = 1;
                        //        _db.UairangoConfiguracoes.Update(uairangoConfiguraco_PRAZO_RETIRADA);
                        //    }
                        //}
                        //else
                        //{
                        //    await _db.UairangoConfiguracoes.AddAsync(new UairangoConfiguraco
                        //    {
                        //        CdEmpresa = empresa.CdEmpresa,
                        //        Unity = empresa.Unity,
                        //        Chave = ChavesConfigUairango.PRAZO_RETIRADA,
                        //        Valor = config.PrazoRetirada.ToString(),
                        //        Integrated = 1
                        //    });
                        //}
                    }
                    await _db.SaveChangesAsync();
                    AdicionarLinhaRichTextBox($"Configuração do estabelecimento atualizada ({DateTime.Now})");
                }
                else
                {
                    _logger.LogError("Erro desconhecido ao obter configuração do estabelecimento");
                    AdicionarLinhaRichTextBox($"Erro desconhecido ao obter configuração do estabelecimento ({DateTime.Now})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter configuração do estabelecimento: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao obter configuração do estabelecimento ({DateTime.Now}): {ex.Message}");
            }
        }

        private async Task ReceberFormasPagamento(Empresa empresa, string token)
        {
            try
            {
                _db.ChangeTracker.Clear();

                var formasDelivery = await this._formasPagamentoService.ObterFormasPagamentoAsync(Convert.ToInt32(empresa.UairangoIdEstabelecimento), "D", token);
                var formasRetirada = await this._formasPagamentoService.ObterFormasPagamentoAsync(Convert.ToInt32(empresa.UairangoIdEstabelecimento), "R", token);
                var allFormasPorEmpresa = await _db.UairangoFormasPagamentos.FromSqlRaw($@"
                        select * from uairango_formas_pagamento u
                        where empresa = {empresa.CdEmpresa}
                        and unity = {empresa.Unity}
                        ").ToListAsync();
                var formasDeliveryPrecisamEnviar = allFormasPorEmpresa
                    .Where(x => x.TipoEntrega == "D" && (x.Integrated ?? 0) != 1)
                    .ToList();

                var formasRetiradaPrecisamEnviar = allFormasPorEmpresa
                    .Where(x => x.TipoEntrega == "R" && (x.Integrated ?? 0) != 1)
                    .ToList();

                if ((formasDeliveryPrecisamEnviar == null || formasDeliveryPrecisamEnviar.Count == 0) && formasDelivery != null)
                {
                    foreach (var item in formasDelivery)
                    {
                        var formaDb = allFormasPorEmpresa
                            .FirstOrDefault(x => x.IdFormaUairango == item.IdForma.ToString() && x.TipoEntrega == "D");
                        if (formaDb != null)
                        {
                            formaDb.Nome = item.Nome;
                            formaDb.Ativo = item.Ativo;
                            formaDb.Integrated = 1;
                            _db.UairangoFormasPagamentos.Update(formaDb);
                        }
                        else
                        {
                            var novaForma = new UairangoFormasPagamento
                            {
                                IdFormaUairango = item.IdForma.ToString(),
                                Nome = item.Nome,
                                Ativo = item.Ativo,
                                TipoEntrega = "D",
                                Integrated = 1,
                                Empresa = empresa.CdEmpresa,
                                Unity = empresa.Unity
                            };
                            await _db.UairangoFormasPagamentos.AddAsync(novaForma);
                        }
                    }
                    await _db.SaveChangesAsync();
                }
                if ((formasRetiradaPrecisamEnviar == null || formasRetiradaPrecisamEnviar.Count == 0) && formasRetirada != null)
                {
                    foreach (var item in formasRetirada)
                    {
                        var formaDb = allFormasPorEmpresa
                            .FirstOrDefault(x => x.IdFormaUairango == item.IdForma.ToString() && x.TipoEntrega == "R");
                        if (formaDb != null)
                        {
                            formaDb.Nome = item.Nome;
                            formaDb.Ativo = item.Ativo;
                            formaDb.Integrated = 1;
                            _db.UairangoFormasPagamentos.Update(formaDb);
                        }
                        else
                        {
                            var novaForma = new UairangoFormasPagamento
                            {
                                IdFormaUairango = item.IdForma.ToString(),
                                Nome = item.Nome,
                                Ativo = item.Ativo,
                                TipoEntrega = "R",
                                Integrated = 1,
                                Empresa = empresa.CdEmpresa,
                                Unity = empresa.Unity
                            };
                            await _db.UairangoFormasPagamentos.AddAsync(novaForma);
                        }
                    }
                    await _db.SaveChangesAsync();
                }

                _logger.LogInformation($"Formas de pagamento atualizadas/inseridas com sucesso ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"Formas de pagamento atualizadas/inseridas com sucesso ({DateTime.Now})");

            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar formas de pagamento: {e.Message}", e);
                AdicionarLinhaRichTextBox($"Erro ao atualizar formas de pagamento ({DateTime.Now}): {e.Message}");
            }
            finally
            {
                _logger.LogInformation($"Finalizando atualização de formas de pagamento ({DateTime.Now})");
                AdicionarLinhaRichTextBox($"Finalizando atualização de formas de pagamento ({DateTime.Now})");
            }
        }

        private async Task VincularEstabelecimentoDeveloper(Empresa empresa, string token)
        {
            var response = await this._estabelecimentoService.ChecarVinculoPorTokenAsync(token, empresa.UairangoTokenVinculo);
            if (response != null)
            {
                if (response.Vinculado)
                {
                    empresa.UairangoVinculado = true;
                    //empresa.UairangoIdEstabelecimento = response.IdEstabelecimento.ToString();
                    _db.Empresas.Update(empresa);
                    await _db.SaveChangesAsync();
                    AdicionarLinhaRichTextBox($"Estabelecimento vinculado ({DateTime.Now})");
                }
                else
                {
                    var responseVincular = await this._estabelecimentoService.VincularEstabelecimentoAsync(token, empresa.UairangoTokenVinculo);
                    if (responseVincular != null)
                    {
                        if (responseVincular.Success)
                        {
                            empresa.UairangoVinculado = true;
                            empresa.UairangoIdEstabelecimento = responseVincular.IdEstabelecimento.ToString();
                            _db.Empresas.Update(empresa);
                            await _db.SaveChangesAsync();
                            AdicionarLinhaRichTextBox($"Estabelecimento vinculado ({DateTime.Now})");
                        }
                        else
                        {
                            _logger.LogError($"Erro ao vincular estabelecimento: {responseVincular.Message}");
                            AdicionarLinhaRichTextBox($"Erro ao vincular estabelecimento ({DateTime.Now}): {responseVincular.Message}");
                        }
                    }
                    else
                    {
                        _logger.LogError("Erro desconhecido ao vincular estabelecimento");
                        AdicionarLinhaRichTextBox($"Erro desconhecido ao vincular estabelecimento ({DateTime.Now})");
                    }
                }
            }
            else
            {
                _logger.LogError("Erro desconhecido ao checar vinculo do estabelecimento");
                AdicionarLinhaRichTextBox($"Erro desconhecido ao checar vinculo do estabelecimento ({DateTime.Now})");
            }
        }

        #endregion
        #endregion
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void buttonMinimizar_Click(object sender, EventArgs e)
        {
            // Esconde a janela, mas mantém o app rodando na bandeja
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var resultado = MessageBox.Show("Tem certeza que deseja sair?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                _timer?.Stop();
                _timer?.Dispose();
                notifyIcon1.Visible = false;
                // segue o fechamento normalmente
            }
            else
            {
                e.Cancel = true; // impede o fechamento
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visible && this.WindowState != FormWindowState.Minimized)
                {
                    // Minimiza e esconde
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                    this.Hide();
                }
                else
                {
                    // Restaura e mostra
                    this.StartPosition = FormStartPosition.CenterScreen; // só se quiser garantir que fique no centro
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SalvarEstadoCheckBox(checkBox1.Checked);
        }

        private void SalvarEstadoCheckBox(bool isChecked)
        {
            string valor = isChecked ? "1" : "0";
            IniFile.WritePrivateProfileString("Configuracoes", "CheckBox1", valor, iniFilePath);
        }

        private bool LerEstadoCheckBox()
        {
            byte[] buffer = new byte[255];
            IniFile.GetPrivateProfileString("Configuracoes", "CheckBox1", "0", buffer, buffer.Length, iniFilePath);
            string valor = System.Text.Encoding.ASCII.GetString(buffer).TrimEnd('\0');
            return valor == "1";
        }

        private void CarregarEstadoCheckBox()
        {
            if (File.Exists(iniFilePath))
            {
                checkBox1.Checked = LerEstadoCheckBox();
            }
            else
            {
                SalvarEstadoCheckBox(false);
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1_MouseDoubleClick(sender, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
        }
    }
}
