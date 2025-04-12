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
        private readonly TimeSpan _intervaloExecucao = TimeSpan.FromMinutes(30);

        #region Inject services
        private readonly ILoginService _loginService;
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly ICulinariaService _culinariaService;
        private readonly IFormasPagamentoService _formasPagamentoService;
        #endregion

        public MainForm(GlobalErpFiscalBaseContext db, ILogger<MainForm> logger,
        #region Inject Services
            ILoginService loginService,
            IEstabelecimentoService estabelecimentoService,
            ICulinariaService culinariaService,
            IFormasPagamentoService formasPagamentoService
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
            #endregion

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
            if (!_executando)
            {
                Task.Run(async () =>
                {
                    _executando = true;
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
                    if (empresasComTokenVinculo != null)
                    {
                        foreach (Empresa empresa in empresasComTokenVinculo)
                        {
                            if ((!string.IsNullOrEmpty(empresa.UairangoIdEstabelecimento))
                                && (empresa.UairangoVinculado ?? false)
                                && (empresa.UairangoIdEstabelecimento.Length > 0))
                            {
                                await EnviarFormasPagamento(ultimoLogin.TokenAcesso, empresa);
                            }
                        }
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
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar formas de pagamento (r): {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao enviar formas de pagamento (r) ({DateTime.Now}): {ex.Message}");
            }
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (_executandoGetRecorrente)
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
            if (ultimoLogin == null || idadeDoTokenMenorQueUmDia)
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
                await GetCulinarias(token);
                #endregion
                #region IterarEstabelecimentos
                await IterarEstabelecimento(token);
                #endregion
            }
            else
            {
                _logger.LogError("Erro desconhecido ao fazer login no UaiRango");
            }
        }


        #region Services_Calls
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
                if (empresasComTokenVinculo != null)
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
                            await ReceberFormasPagamento(empresa, token);









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
            //if (e.Button == MouseButtons.Left)
            //{
            //    // Restaura a janela
            //    this.Show();
            //    this.WindowState = FormWindowState.Normal;
            //    this.ShowInTaskbar = true;
            //}
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

    }
}
