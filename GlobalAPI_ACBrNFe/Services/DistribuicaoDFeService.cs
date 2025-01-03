﻿using ACBrLib.Core;
using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using GlobalErpData.Data;
using GlobalErpData.Models;
using GlobalLib.Strings;
using Microsoft.EntityFrameworkCore;

public class DistribuicaoDFeService : IHostedService, IDisposable
{
    private Task _executingTask;
    private readonly CancellationTokenSource _stoppingCts = new();
    private readonly PeriodicTimer _timer;
    private readonly ACBrNFe nfe;
    private readonly IConfiguration _config;
    private readonly ILogger<DistribuicaoDFeService> _logger;
    private readonly GlobalErpFiscalBaseContext context;

    public DistribuicaoDFeService(ILogger<DistribuicaoDFeService> logger, ACBrLib.NFe.ACBrNFe aCBrNFe, IConfiguration config, GlobalErpFiscalBaseContext context)
    {
        _logger = logger;
        nfe = aCBrNFe;
        _config = config;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        this.context = context;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DistribuicaoDFeService iniciado.");

        // Executa o loop em background
        _executingTask = ExecuteAsync(_stoppingCts.Token);

        // Se a tarefa completou imediatamente, havia um erro
        if (_executingTask.IsCompleted)
        {
            await _executingTask;
        }

        return;
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Ignorar exceção de cancelamento normal
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado no serviço.");
            throw;
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Iniciando verificação de empresas...");

            // 1) Carregar a lista de empresas do banco
            List<Empresa> listaEmpresas = await CarregarEmpresasDoBanco();

            // 2) Percorrer cada empresa
            foreach (var empresa in listaEmpresas)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                DateTime dataUltimaExecucao = empresa.UltimaExecucaoDfe ?? DateTime.MinValue;
                var diferenca = DateTime.Now - dataUltimaExecucao;

                if (diferenca.TotalMinutes >= 61)
                {
                    _logger.LogInformation($"Executando consulta SEFAZ para empresa {empresa.CdEmpresa}");
                    await xCore(empresa);
                }
                else
                {
                    _logger.LogInformation($"Menos de 61 minutos para a empresa {empresa.CdEmpresa}. Pulando...");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar DoWork no DistribuicaoDFeService.");
        }
    }

    private string GetPathIni()
    {
        var diretorioPadrao = "C:\\Global\\NFE\\DistribuicaoDFe\\ACBrLib.ini";
        if (!Directory.Exists(diretorioPadrao))
        {
            Directory.CreateDirectory(diretorioPadrao);
        }
        return diretorioPadrao;
    }

    private async void SetConfiguracaoNfe(int CdEmpresa, Empresa empresa, Certificado cer)
    {
        nfe.Config.VersaoDF = VersaoNFe.ve400;
        nfe.Config.ModeloDF = ModeloNFe.moNFe;
        nfe.Config.DFe.SSLCryptLib = SSLCryptLib.cryOpenSSL;
        nfe.Config.DFe.SSLHttpLib = SSLHttpLib.httpOpenSSL;
        nfe.Config.DFe.SSLXmlSignLib = SSLXmlSignLib.xsLibXml2;
        nfe.Config.DFe.ArquivoPFX = cer.CaminhoCertificado;
        nfe.Config.DFe.Senha = cer.Senha;
        nfe.Config.DFe.VerificarValidade = true;
        nfe.Config.DFe.UF = empresa.CdCidadeNavigation.Uf;
        if ((cer.Tipo ?? "").Equals("H"))
        {
            nfe.Config.Ambiente = TipoAmbiente.taHomologacao;
        }
        else
        {
            nfe.Config.Ambiente = TipoAmbiente.taProducao;
        }
        nfe.Config.SSLType = SSLType.LT_TLSv1_2;
        nfe.Config.Timeout = 10000;
        nfe.Config.Proxy.Servidor = "";
        nfe.Config.Proxy.Porta = "";
        nfe.Config.Proxy.Usuario = "";
        nfe.Config.Proxy.Senha = "";
        nfe.Config.SalvarGer = true;
        nfe.Config.SepararPorMes = true;
        nfe.Config.AdicionarLiteral = false;
        nfe.Config.EmissaoPathNFe = false;
        nfe.Config.SalvarArq = true;
        nfe.Config.SepararPorCNPJ = true;
        nfe.Config.SepararPorModelo = true;
        if (string.IsNullOrEmpty(_config["AcbrSettings:PathIni"]))
        {
            throw new Exception("PathIni não configurado");
        }
        if (string.IsNullOrEmpty(_config["AcbrSettings:PathSchemas"]))
        {
            throw new Exception("PathSchemas não configurado");
        }
        if (string.IsNullOrEmpty(_config["AcbrSettings:PathNFe"]))
        {
            throw new Exception("PathNFe não configurado");
        }
        if (string.IsNullOrEmpty(_config["AcbrSettings:PathInu"]))
        {
            throw new Exception("PathInu não configurado");
        }
        if (string.IsNullOrEmpty(_config["AcbrSettings:PathEvento"]))
        {
            throw new Exception("PathEvento não configurado");
        }
        if (string.IsNullOrEmpty(_config["AcbrSettings:BasePathPDF"]))
        {
            throw new Exception("BasePathPDF não configurado");
        }
        if (!Directory.Exists(_config["AcbrSettings:PathSchemas"]))
        {
            Directory.CreateDirectory(_config["AcbrSettings:PathSchemas"]);
        }
        if (!Directory.Exists(_config["PathIni"]))
        {
            Directory.CreateDirectory(_config["AcbrSettings:PathIni"]);
        }
        if (!Directory.Exists(_config["PathNFe"]))
        {
            Directory.CreateDirectory(_config["AcbrSettings:PathNFe"]);
        }
        if (!Directory.Exists(_config["PathInu"]))
        {
            Directory.CreateDirectory(_config["AcbrSettings:PathInu"]);
        }
        if (!Directory.Exists(_config["PathEvento"]))
        {
            Directory.CreateDirectory(_config["AcbrSettings:PathEvento"]);
        }

        nfe.Config.PathSchemas = _config["AcbrSettings:PathSchemas"];
        nfe.Config.IniServicos = _config["AcbrSettings:PathIni"];
        nfe.Config.PathNFe = _config["AcbrSettings:PathNFe"];
        nfe.Config.PathInu = _config["AcbrSettings:PathInu"];
        nfe.Config.PathEvento = _config["AcbrSettings:PathEvento"];

        string pathPdf = _config["AcbrSettings:BasePathPDF"] + CdEmpresa.ToString();
        if (!Directory.Exists(pathPdf))
        {
            Directory.CreateDirectory(pathPdf);
        }
        nfe.Config.DANFe.PathPDF = pathPdf;
        nfe.Config.Principal.TipoResposta = TipoResposta.fmtINI;
        nfe.Config.DANFe.PathLogo = "";
        nfe.Config.DANFe.TipoDANFE = TipoDANFE.tiRetrato;
        nfe.Config.DANFe.MostraSetup = false;
        nfe.Config.DANFe.MostraPreview = false;
        nfe.Config.DANFe.MostraStatus = false;
        nfe.Config.Email.Nome = "";
        nfe.Config.Email.Conta = "";
        nfe.Config.Email.Usuario = "";
        nfe.Config.Email.Senha = "";
        nfe.Config.Email.Servidor = "";
        nfe.Config.Email.Porta = "";
        nfe.Config.Email.SSL = false;
        nfe.Config.Email.TLS = false;

        var path = GetPathIni();
        nfe.ConfigGravar(path);
    }

    private async Task xCore(Empresa empresa)
    {
        throw new NotImplementedException();
        /*var certificado = await context.Certificados.FirstOrDefaultAsync(x => x.IdEmpresa == empresa.CdEmpresa);
        if (certificado == null)
        {
            _logger.LogError($"Certificado não encontrado para empresa {empresa.CdEmpresa}");
            return;
        }

        SetConfiguracaoNfe(empresa.CdEmpresa, empresa, certificado);

        string ultimoNSU = empresa.UltimoNsu ?? "0";
        bool temMais = true;

        while (temMais)
        {
            DistribuicaoDFeResposta<TipoEventoNFe> distribuicao = nfe.DistribuicaoDFePorUltNSU(GetUf(empresa.CdCidadeNavigation), UtlStrings.OnlyInteger(empresa.CdCnpj), ultimoNSU);

            if (distribuicao == null)
            {
                _logger.LogError($"Erro ao executar DistribuicaoDFePorUltNSU para empresa {empresa.CdEmpresa}");
                break;
            }

            // Verifica o status retornado
            if (distribuicao.CStat == 137 || distribuicao.CStat == 656)
            {
                temMais = false;
                _logger.LogInformation($"Consulta finalizada: {distribuicao.XMotivo}");
                break;
            }

            // Atualiza o último NSU
            if (!string.IsNullOrEmpty(distribuicao.ultNSU))
            {
                ultimoNSU = distribuicao.ultNSU;
                empresa.UltimoNsu = ultimoNSU;
                context.Empresas.Update(empresa);
                await context.SaveChangesAsync();
            }

            // Processa os documentos retornados
            if (distribuicao.CStat == 138)
            {
                foreach (var doc in distribuicao.ResDFeResposta)
                {
                    try
                    {
                        var distribDfe = MapearDistribuicaoDfe(empresa.CdEmpresa, doc, distribuicao.Resposta);
                        await context.DistribuicaoDfes.AddAsync(distribDfe);
                        await context.SaveChangesAsync();

                        _logger.LogInformation($"Documento {doc.ChDFe} processado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erro ao processar documento NSU {doc.NSU}.");
                    }
                }
            }
            else
            {
                temMais = false;
                _logger.LogWarning($"Consulta interrompida: {distribuicao.XMotivo}");
            }
        }
        */
    }

    /// <summary>
    /// Mapeia um documento de resposta para o modelo de Distribuição DFe.
    /// </summary>
    private DistribuicaoDfe MapearDistribuicaoDfe(int idEmpresa, ResDFeResposta doc, string xml)
    {
        throw new NotImplementedException();
        /*
        return new DistribuicaoDfe
        {
            Id = Guid.NewGuid(),
            IdEmpresa = idEmpresa,
            Serie = doc.ChDFe?.Substring(22, 3) ?? string.Empty,
            NrNotaFiscal = doc.ChDFe?.Substring(25, 9) ?? string.Empty,
            ChaveAcessoNfe = doc.ChDFe,
            Cnpj = doc.CNPJCPF,
            Nome = doc.xNome,
            Ie = doc.IE,
            TpNfe = doc.tpNF == TipoNFe.tnEntrada ? "tnEntrada" : "tnSaida",
            Nsu = doc.NSU,
            Emissao = doc.dhEmi.ToString("yyyy-MM-dd"),
            Valor = doc.vNF,
            Impresso = doc.cSitNFe == SituacaoDFe.snAutorizado ? "snAutorizado" :
                       doc.cSitNFe == SituacaoDFe.snDenegado ? "snDenegado" : "snCancelado",
            TpResposta = 'R',
            DtRecebimento = DateOnly.FromDateTime(doc.dhRecbto),
            Xml = xml,
            DtInclusao = DateOnly.FromDateTime(DateTime.Now)
        };*/
    }


    private int GetUf(Cidade cdCidadeNavigation)
    {
        return cdCidadeNavigation.Uf switch
        {
            "AC" => 12, // Acre
            "AL" => 27, // Alagoas
            "AP" => 16, // Amapá
            "AM" => 13, // Amazonas
            "BA" => 29, // Bahia
            "CE" => 23, // Ceará
            "DF" => 53, // Distrito Federal
            "ES" => 32, // Espírito Santo
            "GO" => 52, // Goiás
            "MA" => 21, // Maranhão
            "MT" => 51, // Mato Grosso
            "MS" => 50, // Mato Grosso do Sul
            "MG" => 31, // Minas Gerais
            "PA" => 15, // Pará
            "PB" => 25, // Paraíba
            "PR" => 41, // Paraná
            "PE" => 26, // Pernambuco
            "PI" => 22, // Piauí
            "RJ" => 33, // Rio de Janeiro
            "RN" => 24, // Rio Grande do Norte
            "RS" => 43, // Rio Grande do Sul
            "RO" => 11, // Rondônia
            "RR" => 14, // Roraima
            "SC" => 42, // Santa Catarina
            "SP" => 35, // São Paulo
            "SE" => 28, // Sergipe
            "TO" => 17, // Tocantins
            _ => throw new ArgumentException($"Invalid UF: {cdCidadeNavigation.Uf}")
        };
    }

    private async Task AtualizarUltimaExecucaoNoBanco(Empresa empresa, DateTime now)
    {
        empresa.UltimaExecucaoDfe = now;
        await context.SaveChangesAsync();
    }

    private async Task<List<Empresa>> CarregarEmpresasDoBanco()
    {
        var empresas = await context.Empresas.Include(i => i.CdCidadeNavigation).ToListAsync();
        return empresas;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Atualizador de Token pausando execução.");

        if (_executingTask == null)
            return;

        // Sinaliza para parar
        _stoppingCts.Cancel();

        // Aguarda até que a tarefa em execução termine
        await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

        cancellationToken.ThrowIfCancellationRequested();
    }

    public void Dispose()
    {
        _stoppingCts.Cancel();
        _stoppingCts.Dispose();
        _timer?.Dispose();
    }
}