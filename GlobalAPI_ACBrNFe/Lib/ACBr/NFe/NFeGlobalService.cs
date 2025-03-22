using ACBrLib.Core;
using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils;
using GlobalAPI_ACBrNFe.Lib.Enum;
using GlobalAPI_ACBrNFe.Lib.Xml;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Services;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NFe.Classes;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using Org.BouncyCastle.Ocsp;
using System;
using System.ComponentModel;
using System.Globalization;

namespace GlobalAPI_ACBrNFe.Lib.ACBr.NFe
{
    public class NFeGlobalService
    {
        private readonly ACBrNFe nfe;
        private readonly ILogger<NFeGlobalService> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private Saida saida;
        private readonly SaidaCalculationService saidaCalculationService;
        public NFeGlobalService(ACBrNFe aCBrNFe,
            ILogger<NFeGlobalService> logger,
            IMapper mapper,
            IHubContext<ImportProgressHub> hubContext,
            GlobalErpFiscalBaseContext db,
            SaidaCalculationService saidaCalculationService,
            IConfiguration config)
        {
            nfe = aCBrNFe;
            _logger = logger;
            this.mapper = mapper;
            _hubContext = hubContext;
            this.saidaCalculationService = saidaCalculationService;
            this.db = db;
            _config = config;
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
            nfe.ConfigGravar();
        }

        public async Task<ResponseGerarDto> GerarNFeAsync(NotaFiscal notaFiscal, Saida saida, Empresa empresa, Certificado cer)
        {
            ResponseGerarDto responseGerarDto = new ResponseGerarDto();

            this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
            string nota = notaFiscal.ToString();
            string doc = $"S{saida.NrLanc}";
            responseGerarDto.pathPdf = System.IO.Path.Combine(nfe.Config.DANFe.PathPDF, doc + ".pdf");
            if (System.IO.File.Exists(responseGerarDto.pathPdf))
            {
                System.IO.File.Delete(responseGerarDto.pathPdf);
            }

            nfe.LimparLista();
            nfe.Config.DANFe.NomeDocumento = doc;
            nfe.CarregarINI(nota);
            nfe.Assinar();
            nfe.Validar();
            EnvioRetornoResposta? rep = nfe.Enviar(saida.NrLanc, false, true);
            responseGerarDto.envioRetornoResposta = rep;
            if (rep.Envio.CStat.Equals(100) || rep.Envio.CStat.Equals(103))
            {
                responseGerarDto.xml = nfe.ObterXml(0);
                nfe.ImprimirPDF();
            }

            return responseGerarDto;
        }

        public async Task<NotaFiscal> MontarNFeAsync(Saida saida, Empresa empresa, Certificado cer, bool isContingencia = false)
        {
            var notaFiscal = new NotaFiscal();

            notaFiscal.InfNFe.Versao = "4.0";
            await GrupoIde(saida, notaFiscal, empresa, cer, isContingencia);
            await GrupoEmitente(saida, notaFiscal, empresa);
            await GrupoDestinatario(saida, notaFiscal);
            notaFiscal.DadosAdicionais.infCpl = saida.TxtObsNf ?? "";
            await GrupoProduto(saida, notaFiscal, empresa);
            GrupoTotais(notaFiscal, saida);
            await GrupoTransportadora(saida, notaFiscal);
            await GrupoFatura(saida, notaFiscal);
            await GrupoDuplicata(saida, notaFiscal);
            await GrupoPagamento(saida, notaFiscal);

            return notaFiscal;
        }

        private async Task GrupoDuplicata(Saida saida, NotaFiscal notaFiscal)
        {
            int k = 1;
            var contas = await db.ContasARecebers.AsNoTracking().Where(car => car.NrSaida == saida.NrLanc).ToListAsync();
            if (contas != null && contas.Count > 0)
            {
                foreach (var duplicata in contas)
                {
                    var dup = new DuplicataNFe();
                    dup.nDup = k.ToString();// duplicata.NrDuplicata;
                    dup.dVenc = DateUtils.DateOnlyToDateTime(duplicata.DtVencimento);
                    dup.vDup = duplicata.VlConta;
                    notaFiscal.Duplicatas.Add(dup);

                    k++;
                }
            }
        }

        private async Task GrupoFatura(Saida saida, NotaFiscal notaFiscal)
        {
            var fat = notaFiscal.Fatura;
            fat.nFat = notaFiscal.Identificacao.cNF.ToString();
            fat.vOrig = notaFiscal.Total.vNF;
            fat.vDesc = notaFiscal.Total.vDesc;
            fat.vLiq = notaFiscal.Total.vNF;
        }

        private async Task GrupoPagamento(Saida saida, NotaFiscal notaFiscal)
        {
            var pagamento = new PagamentoNFe();
            if (notaFiscal.Identificacao.finNFe == FinalidadeNFe.fnDevolucao)
            {
                pagamento.tPag = FormaPagamento.fpSemPagamento;
                pagamento.vPag = 0;
            }
            else
            {
                if (saida.TpPagt.Equals("V"))
                    pagamento.indPag = IndicadorPagamento.ipVista;
                else
                    pagamento.indPag = IndicadorPagamento.ipPrazo;
                pagamento.tpIntegra = TpIntegra.tiNaoInformado;
                switch (saida.TpPagt)
                {
                    case "V":
                        pagamento.tPag = FormaPagamento.fpDinheiro;
                        break;
                    case "P":
                        pagamento.tPag = FormaPagamento.fpCreditoLoja;
                        break;
                    case "C":
                        pagamento.tPag = FormaPagamento.fpCartaoCredito;
                        break;
                    case "D":
                        pagamento.tPag = FormaPagamento.fpCartaoDebito;
                        break;
                    case "B":
                        pagamento.tPag = FormaPagamento.fpBoletoBancario;
                        break;
                    default:
                        pagamento.tPag = FormaPagamento.fpDinheiro;
                        break;
                }
                pagamento.vPag = notaFiscal.Total.vNF;
            }

            pagamento.vTroco = 0;
            notaFiscal.Pagamentos.Add(pagamento);
        }

        private async Task GrupoTransportadora(Saida saida, NotaFiscal notaFiscal)
        {
            if (saida.Fretes == null || !saida.Fretes.Any())
            {
                // No freight data, set modFrete to mfSemFrete
                notaFiscal.Transportador.modFrete = ACBrLib.NFe.ModalidadeFrete.mfSemFrete;
            }
            else
            {
                var frete = saida.Fretes.First();
                var modalidadeFrete = (ACBrLib.NFe.ModalidadeFrete)frete.FretePorConta;
                var tipoAmarracaoTranportadora = FreteHelper.EncontrarTipoAmarracao(modalidadeFrete);

                switch (tipoAmarracaoTranportadora)
                {
                    case TTipoAmarracaoTranportadora.tatOpcional:
                        {
                            if (frete.CdTransp == null)
                            {
                                notaFiscal.Transportador.modFrete = modalidadeFrete;
                                notaFiscal.Transportador.CNPJCPF = "";
                                notaFiscal.Transportador.xNome = "";
                                notaFiscal.Transportador.IE = "";
                                notaFiscal.Transportador.xEnder = "";
                                notaFiscal.Transportador.xMun = "";
                                notaFiscal.Transportador.UF = "";

                                if (notaFiscal.Identificacao.idDest != ACBrLib.NFe.DestinoOperacao.doInterestadual)
                                {
                                    if (notaFiscal.Emitente.cMun == notaFiscal.Destinatario.cMun)
                                    {
                                        notaFiscal.Transportador.Placa = frete.Transportadora?.PlacaVeiculo ?? "";
                                        notaFiscal.Transportador.UFPlaca = frete.Transportadora?.Uf ?? "";
                                        notaFiscal.Transportador.RNTC = "";
                                    }
                                }

                                await InserirVolumesSaidas(saida, notaFiscal);
                            }
                            else
                            {
                                // Transporter provided
                                notaFiscal.Transportador.modFrete = modalidadeFrete;
                                var transportadora = frete.Transportadora;

                                if (transportadora != null)
                                {
                                    notaFiscal.Transportador.CNPJCPF = (transportadora.CdCnpj ?? "").Length > 0 ? UtlStrings.OnlyInteger(transportadora.CdCnpj ?? "") : "";
                                    notaFiscal.Transportador.xNome = transportadora.NmTransportadora;
                                    notaFiscal.Transportador.IE = transportadora.CdIe;
                                    notaFiscal.Transportador.xEnder = $"{transportadora.NmEndereco},{transportadora.Numero}";
                                    notaFiscal.Transportador.xMun = transportadora.NmCidade;
                                    notaFiscal.Transportador.UF = transportadora.Uf;

                                    if (notaFiscal.Identificacao.idDest != ACBrLib.NFe.DestinoOperacao.doInterestadual)
                                    {
                                        if (!string.IsNullOrEmpty(transportadora.PlacaVeiculo))
                                        {
                                            // Set vehicle data from transportadora
                                            notaFiscal.Transportador.Placa = transportadora.PlacaVeiculo;
                                            notaFiscal.Transportador.UFPlaca = transportadora.Uf;
                                            notaFiscal.Transportador.RNTC = "";
                                        }
                                    }
                                }

                                await InserirVolumesSaidas(saida, notaFiscal);
                            }
                            break;
                        }
                    case TTipoAmarracaoTranportadora.tatObrigatorio:
                        {
                            notaFiscal.Transportador.modFrete = modalidadeFrete;
                            var transportadora = frete.Transportadora;

                            if (transportadora != null)
                            {
                                notaFiscal.Transportador.CNPJCPF = (transportadora.CdCnpj ?? "").Length > 0 ? UtlStrings.OnlyInteger(transportadora.CdCnpj ?? "") : "";
                                notaFiscal.Transportador.xNome = transportadora.NmTransportadora;
                                notaFiscal.Transportador.IE = transportadora.CdIe;
                                notaFiscal.Transportador.xEnder = $"{transportadora.NmEndereco},{transportadora.Numero}";
                                notaFiscal.Transportador.xMun = transportadora.NmCidade;
                                notaFiscal.Transportador.UF = transportadora.Uf;

                                if (notaFiscal.Identificacao.idDest != ACBrLib.NFe.DestinoOperacao.doInterestadual)
                                {
                                    if (!string.IsNullOrEmpty(transportadora.PlacaVeiculo))
                                    {
                                        // Set vehicle data from transportadora
                                        notaFiscal.Transportador.Placa = transportadora.PlacaVeiculo;
                                        notaFiscal.Transportador.UFPlaca = transportadora.Uf;
                                        notaFiscal.Transportador.RNTC = "";
                                    }
                                }
                            }

                            await InserirVolumesSaidas(saida, notaFiscal);
                            break;
                        }
                    case TTipoAmarracaoTranportadora.tatNaoAmarra:
                        {
                            notaFiscal.Transportador.modFrete = modalidadeFrete;
                            notaFiscal.Transportador.CNPJCPF = "";
                            notaFiscal.Transportador.xNome = "";
                            notaFiscal.Transportador.IE = "";
                            notaFiscal.Transportador.xEnder = "";
                            notaFiscal.Transportador.xMun = "";
                            notaFiscal.Transportador.UF = "";

                            if (notaFiscal.Identificacao.idDest != ACBrLib.NFe.DestinoOperacao.doInterestadual)
                            {
                                if (notaFiscal.Emitente.cMun == notaFiscal.Destinatario.cMun)
                                {
                                    // Set vehicle data from frete
                                    notaFiscal.Transportador.Placa = frete.Transportadora?.PlacaVeiculo ?? "";
                                    notaFiscal.Transportador.UFPlaca = frete.Transportadora?.Uf ?? "";
                                    notaFiscal.Transportador.RNTC = "";
                                }
                            }

                            await InserirVolumesSaidas(saida, notaFiscal);
                            break;
                        }
                }
            }
        }

        private async Task InserirVolumesSaidas(Saida saida, NotaFiscal notaFiscal)
        {
            if (saida.SaidasVolumes != null && saida.SaidasVolumes.Count > 0)
            {
                foreach (var volume in saida.SaidasVolumes)
                {
                    var vol = new VolumeNFe();
                    vol.qVol = volume.QVol;
                    vol.esp = volume.Esp;
                    vol.Marca = volume.Marca;
                    vol.nVol = volume.NVol;
                    vol.pesoL = volume.PesoL;
                    vol.pesoB = volume.PesoB;
                    //vol.Lacres= volume.Lacres;
                    notaFiscal.Volumes.Add(vol);
                }
            }
        }

        private void GrupoTotais(NotaFiscal notaFiscal, Saida saida)
        {
            notaFiscal.Total.vBC = notaFiscal.Produtos.Sum(p => p.ICMS.vBC ?? 0);
            notaFiscal.Total.vICMS = notaFiscal.Produtos.Sum(p => p.ICMS.vICMS ?? 0);
            notaFiscal.Total.vBCST = notaFiscal.Produtos.Sum(p => p.ICMS.vBCST ?? 0);
            notaFiscal.Total.vST = notaFiscal.Produtos.Sum(p => p.ICMS.vICMSST ?? 0);
            notaFiscal.Total.vProd = notaFiscal.Produtos.Sum(p => p.vProd);
            notaFiscal.Total.vFrete = notaFiscal.Produtos.Sum(p => p.vFrete ?? 0);
            notaFiscal.Total.vSeg = notaFiscal.Produtos.Sum(p => p.vSeg ?? 0);
            notaFiscal.Total.vDesc = notaFiscal.Produtos.Sum(p => p.vDesc ?? 0);
            notaFiscal.Total.vII = 0;
            notaFiscal.Total.vICMSDeson = 0;
            notaFiscal.Total.vFCPST = 0;
            notaFiscal.Total.vIPI = notaFiscal.Produtos.Sum(p => p.IPI.vIPI ?? 0);
            notaFiscal.Total.vPIS = notaFiscal.Produtos.Sum(p => p.PIS.vPIS ?? 0);
            notaFiscal.Total.vCOFINS = notaFiscal.Produtos.Sum(p => p.COFINS.vCOFINS ?? 0);
            notaFiscal.Total.vOutro = notaFiscal.Produtos.Sum(p => p.vOutro ?? 0);
            notaFiscal.Total.vTotTrib = notaFiscal.Produtos.Sum(p => p.vTotTrib ?? 0);
            notaFiscal.Total.vFCPUFDest = notaFiscal.Produtos.Sum(p => p.ICMSUFDEST.vFCPUFDest ?? 0);
            notaFiscal.Total.vICMSUFDest = notaFiscal.Produtos.Sum(p => p.ICMSUFDEST.vICMSUFDest ?? 0);
            notaFiscal.Total.vICMSUFRemet = notaFiscal.Produtos.Sum(p => p.ICMSUFDEST.vICMSUFRemet ?? 0);
            notaFiscal.Total.vNF = notaFiscal.Produtos.Sum(p =>
            p.vProd - (p.vDesc ?? 0) + (p.ICMS.vICMSST ?? 0) + (p.vOutro ?? 0) + (p.vFrete ?? 0) + (p.vSeg ?? 0)
            + (p.IPI.vIPI ?? 0)
            );
        }

        private async Task GrupoProduto(Saida saida, NotaFiscal notaFiscal, Empresa empresa)
        {
            if (saida.ProdutoSaida == null || saida.ProdutoSaida.Count() == 0)
            {
                throw new Exception("Nenhum produto encontrado.");
            }
            int TOTAL_DE_ITENS = saida.ProdutoSaida.Count();
            var produto = new ProdutoNFe();

            saidaCalculationService.CalculateTotals(saida);

            decimal totalDescontoItens = 0;
            decimal totalFreteItens = 0;
            decimal totalSeguroItens = 0;
            decimal totalOutroItens = 0;

            int i = 1;

            foreach (ProdutoSaidum ps in saida.ProdutoSaida)
            {
                /*********************************************************************************/
                /************GGGGGGG***G*********GGGGGGG***GGGGGGG***GGGGGGG***G******************/
                /************G*********G*********G*****G***G*****G***G*****G***G******************/
                /************G**GGGG***G*********G*****G***G**GGGG***GGGGGGG***G******************/
                /************G*****G***G*********G*****G***G*****G***G*****G***G******************/
                /************GGGGGGG***GGGGGGG***GGGGGGG***GGGGGGG***G*****G***GGGGGGG************/
                /*********************************************************************************/

                decimal descontoItem = 0;
                decimal vlOutroItem = 0;
                decimal vlFreteItem = 0;
                decimal vlSeguroItem = 0;
                var pe = ps.ProdutoEstoque;

                #region Rateio de Valores
                decimal Φ = (ps.Quant * ps.VlVenda - ps.Desconto)
                        / Convert.ToDecimal(saida.ValorTotalProdutos ?? 1);
                if (eUltimoItem(TOTAL_DE_ITENS, i))
                {
                    descontoItem = Convert.ToDecimal(saida.ValorTotalDesconto ?? 0) - totalDescontoItens;
                    vlOutroItem = Convert.ToDecimal(saida.VlOutro ?? 0) - totalOutroItens;
                    vlFreteItem = Convert.ToDecimal((saida.Fretes != null && saida.Fretes.Count() > 0) ? saida.Fretes.First().VlFrete : 0) - totalFreteItens;
                    vlSeguroItem = Convert.ToDecimal(saida.VlSeguro ?? 0) - totalSeguroItens;
                }
                else
                {
                    descontoItem = ps.Desconto +
                        Φ
                        * (saida.VlDescGlobal ?? 0);
                    totalDescontoItens = totalDescontoItens + descontoItem;

                    vlOutroItem = Φ * (saida.VlOutro ?? 0);
                    totalOutroItens = totalOutroItens + vlOutroItem;

                    vlFreteItem = Φ * Convert.ToDecimal((saida.Fretes != null && saida.Fretes.Count() > 0) ? saida.Fretes.First().VlFrete : 0);
                    totalFreteItens = totalFreteItens + vlFreteItem;

                    vlSeguroItem = Φ * (saida.VlSeguro ?? 0);
                    totalSeguroItens = totalSeguroItens + vlSeguroItem;
                }
                #endregion

                produto.nItem = i;
                produto.cProd = ps.CdProduto.ToString();
                produto.cEAN = ps.CdBarra;
                produto.xProd = pe.NmProduto.Length > 120 ? pe.NmProduto.Substring(0, 120) : pe.NmProduto;
                produto.NCM = ps.Ncm;
                produto.EXTIPI = "";
                produto.CFOP = ps.Cfop;
                produto.uCom = ps.Un;
                produto.qCom = ps.Quant;
                produto.vUnCom = ps.VlVenda;
                produto.vProd = ps.VlVenda * ps.Quant;
                produto.cEANTrib = ps.CdBarra;
                produto.uTrib = ps.Un;
                produto.qTrib = ps.Quant;
                produto.vUnTrib = ps.VlVenda;
                produto.vOutro = vlOutroItem;
                produto.vFrete = vlFreteItem;
                produto.vSeg = vlSeguroItem;
                produto.vDesc = descontoItem;
                produto.infAdProd = "";
                produto.indTot = ACBrLib.NFe.IndicadorTotal.itSomaTotalNFe;

                if ((pe.ExTipi ?? "").Length > 0)
                {
                    produto.EXTIPI = pe.ExTipi;
                }

                var listaCfopAnp = new List<string>
                {
                    "1651", "1652", "1653", "1658", "1659", "1660", "1661", "1662", "1663", "1664",
                    "2651", "2652", "2653", "2658", "2659", "2660", "2661", "2662", "2663", "2664",
                    "3651", "3652", "3653",
                    "5651", "5652", "5653", "5654", "5655", "5656", "5657", "5658", "5659", "5660",
                    "5661", "5662", "5663", "5664", "5665", "5666", "5667",
                    "6651", "6652", "6653", "6654", "6655", "6656", "6657", "6658", "6659", "6660",
                    "6661", "6662", "6663", "6664", "6665", "6666", "6667",
                    "7651", "7654", "7667"
                };
                if (listaCfopAnp.Contains(ps.Cfop ?? ""))
                {
                    if (pe.CdAnp != null)
                    {
                        var tblAnp = await db.TabelaAnps.Where(t => t.Codigo == pe.CdAnp).FirstOrDefaultAsync();
                        if (tblAnp != null)
                        {
                            produto.Combustivel.cProdANP = pe.CdAnp ?? 0;
                            produto.Combustivel.CODIF = "";
                            produto.Combustivel.descANP = tblAnp.Produto;
                            produto.Combustivel.qTemp = ps.Quant;
                            produto.Combustivel.UFCons = empresa.CdCidadeNavigation.Uf;

                            if (pe.CdAnp == 210203001)
                            {
                                produto.Combustivel.pGLP = 100;
                                produto.Combustivel.vPart = produto.vUnTrib;
                            }
                        }
                    }
                }
                //produto.xPed
                //produto.nItemPed
                if (VerificaAplicacaoCEST(ps, saida, empresa))
                {
                    produto.CEST = ps.Cest;
                }

                //ICMS
                #region Validações
                if (((ps.Cfop ?? "").Length <= 0))
                {
                    throw new Exception("Grupo Produto: CFOP é obrigatório.");
                }
                bool DevolucaoSimplesNacional = empresa.TipoRegime == 1 && (!saida.TpSaida.Equals("V"));
                bool RegimeNormal = (empresa.TipoRegime > 1);
                bool RegimeSimplesNacional = !RegimeNormal;
                if ((RegimeNormal || DevolucaoSimplesNacional) &&
                    ((ps.Cst ?? "").Length <= 0))
                {
                    throw new Exception("Grupo Produto: CST e CFOP são obrigatórios para empresas do regime normal.");
                }
                else if (RegimeSimplesNacional &&
                    ((ps.CdCsosn ?? "").Length <= 0))
                {
                    throw new Exception("Grupo Produto: CSOSN e CFOP são obrigatórios para empresas do regime simples nacional.");
                }
                #endregion
                string origem = (RegimeNormal || DevolucaoSimplesNacional) ? ps.Cst.Substring(0, 1) : ps.CdCsosn.Substring(0, 1);
                produto.ICMS.orig = GetOrigem(origem);
                bool configuracaoRetidoSaida = await GetConfiguracaoRetido(saida);

                if (RegimeNormal || DevolucaoSimplesNacional)
                {
                    produto.ICMS.CST = GetCst(ps);
                    switch (produto.ICMS.CST)
                    {
                        case CSTIcms.cst00:
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.vBC = ps.VlBaseIcms;
                            produto.ICMS.pICMS = ps.PocIcms;
                            produto.ICMS.vICMS = ps.VlIcms;
                            break;
                        case CSTIcms.cst10:
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = 0;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PorcSt;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.vBC = 0;
                            produto.ICMS.pICMS = 0;
                            produto.ICMS.vICMS = 0;
                            break;
                        case CSTIcms.cst20:
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.pRedBC = ps.PocReducao;
                            produto.ICMS.vBC = ps.VlBaseIcms;
                            produto.ICMS.pICMS = ps.PocIcms;
                            produto.ICMS.vICMS = ps.VlIcms;

                            break;
                        case CSTIcms.cst30:
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = ps.PorcSt;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PorcSt;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;

                            break;
                        case CSTIcms.cst40:
                            break;
                        case CSTIcms.cst41:
                            break;
                        case CSTIcms.cst50:
                            break;
                        case CSTIcms.cst60:
                            if (produto.Combustivel.cProdANP == 210203001)
                                produto.ICMS.CST = CSTIcms.cstRep60;
                            if (configuracaoRetidoSaida)
                            {
                                produto.ICMS.pST = ps.St;
                                produto.ICMS.vICMSSubstituto = ps.IcmsSubstituto;

                                produto.ICMS.vBCST = ps.VlBaseSt;
                                produto.ICMS.vICMSST = ps.VlSt;
                                produto.ICMS.vBCSTRet = ps.VlBaseRetido;
                                produto.ICMS.vICMSSTRet = ps.VlIcmsRet;
                            }
                            break;
                        case CSTIcms.cst70:
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = 0;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PocIcms;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.pRedBC = ps.PocReducao;
                            produto.ICMS.vBC = ps.VlBaseIcms;
                            produto.ICMS.pICMS = ps.PocIcms;
                            produto.ICMS.vICMS = ps.VlIcms;

                            break;
                        case CSTIcms.cst90:
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = 0;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PocIcms;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.pRedBC = ps.PocReducao;
                            produto.ICMS.vBC = ps.VlBaseIcms;
                            produto.ICMS.pICMS = ps.PocIcms;
                            produto.ICMS.vICMS = ps.VlIcms;

                            break;
                    }

                }
                else if (RegimeSimplesNacional)
                {
                    produto.ICMS.CSOSN = GetCsosn(ps);
                    switch (produto.ICMS.CSOSN)
                    {
                        case CSOSNIcms.csosnVazio:
                            break;
                        case CSOSNIcms.csosn101:
                            produto.ICMS.pCredSN = ps.PcreditoIcms;
                            produto.ICMS.vCredICMSSN = ps.VlCreditoIcms;
                            break;
                        case CSOSNIcms.csosn102:
                            break;
                        case CSOSNIcms.csosn103:
                            break;
                        case CSOSNIcms.csosn201:
                        case CSOSNIcms.csosn202:
                        case CSOSNIcms.csosn203:
                            if (produto.ICMS.CSOSN == CSOSNIcms.csosn201)
                            {
                                produto.ICMS.pCredSN = ps.PcreditoIcms;
                                produto.ICMS.vCredICMSSN = ps.VlCreditoIcms;
                            }
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = 0;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PorcSt;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            break;
                        case CSOSNIcms.csosn300:
                            break;
                        case CSOSNIcms.csosn400:
                            break;
                        case CSOSNIcms.csosn500:
                            if (configuracaoRetidoSaida)
                            {
                                produto.ICMS.pST = ps.St;
                                produto.ICMS.vICMSSubstituto = ps.IcmsSubstituto;
                                produto.ICMS.vBCSTRet = ps.VlBaseRetido;
                                produto.ICMS.vICMSSTRet = ps.VlIcmsRet;
                                produto.ICMS.vBCST = ps.VlBaseSt;
                                produto.ICMS.vICMSST = ps.VlSt;
                            }
                            break;
                        case CSOSNIcms.csosn900:
                            produto.ICMS.modBC = DeterminacaoBaseIcms.dbiValorOperacao;
                            produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;
                            produto.ICMS.pMVAST = ps.MvaSt;
                            produto.ICMS.pRedBCST = 0;
                            produto.ICMS.vBCST = ps.VlBaseSt;
                            produto.ICMS.pICMSST = ps.PocIcms;
                            produto.ICMS.vICMSST = ps.VlSt;
                            produto.ICMS.pRedBC = ps.PocReducao;
                            produto.ICMS.vBC = ps.VlBaseIcms;
                            produto.ICMS.pICMS = ps.PocIcms;
                            produto.ICMS.vICMS = ps.VlIcms;
                            produto.ICMS.pCredSN = ps.PcreditoIcms;
                            produto.ICMS.vCredICMSSN = ps.VlCreditoIcms;
                            break;
                    }
                }
                produto.ICMSUFDEST.vBCUFDest = ps.Vbcufdest;
                produto.ICMSUFDEST.pICMSUFDest = ps.Picmsufdest;
                produto.ICMSUFDEST.vICMSUFDest = ps.Vicmsufdest;
                produto.ICMSUFDEST.vICMSUFRemet = ps.Vicmsufremt;
                produto.ICMSUFDEST.vBCFCPUFDest = ps.Vbcfcpufdest;
                produto.ICMSUFDEST.pFCPUFDest = ps.Pfcpufdest;
                produto.ICMSUFDEST.vFCPUFDest = ps.Vfcpufdest;
                produto.ICMSUFDEST.pICMSInter = ps.Picmsinter;
                produto.ICMSUFDEST.pICMSInterPart = ps.Picmsinterpart;
                produto.ICMSUFDEST.vICMSUFRemet = ps.Vicmsufremt;


                //PIS
                if ((ps.CstPis ?? "").Length > 0)
                {
                    produto.PIS.CST = GetCstPis(ps);
                    if (ps.VlPis > 0 || ps.VlPisSt > 0)
                    {
                        produto.PIS.qBCProd = 0;
                        produto.PIS.vAliqProd = 0;

                        bool ePisSt = (int.Parse((ps.CstPis ?? "").Substring(1, 2)) == 5);

                        if (ePisSt)
                        {
                            produto.PIS.vBC = ps.VlBasePisSt;
                            produto.PIS.pPIS = ps.PorcPisSt;
                            produto.PIS.vPIS = ps.VlPisSt;
                        }
                        else
                        {
                            produto.PIS.vBC = ps.VlBasePis;
                            produto.PIS.pPIS = ps.PorcPis;
                            produto.PIS.vPIS = ps.VlPis;
                        }
                    }
                }
                else
                {
                    produto.PIS.CST = CSTPIS.pis99;
                    produto.PIS.qBCProd = 0;
                    produto.PIS.vAliqProd = 0;
                }

                //COFINS
                if ((ps.CstCofins ?? "").Length > 0)
                {
                    produto.COFINS.CST = GetCstCofins(ps);
                    if (ps.VlCofins > 0 || ps.VlCofinsSt > 0)
                    {
                        produto.COFINS.qBCProd = 0;
                        produto.COFINS.vAliqProd = 0;

                        bool eCofinsSt = (int.Parse((ps.CstCofins ?? "").Substring(1, 2)) == 5);

                        if (eCofinsSt)
                        {
                            produto.COFINS.vBC = ps.VlBaseCofinsSt;
                            produto.COFINS.pCOFINS = ps.PorcCofinsSt;
                            produto.COFINS.vCOFINS = ps.VlCofinsSt;
                        }
                        else
                        {
                            produto.COFINS.vBC = ps.VlBaseCofins;
                            produto.COFINS.pCOFINS = ps.PorcCofins;
                            produto.COFINS.vCOFINS = ps.VlCofins;
                        }
                    }
                }
                else
                {
                    produto.COFINS.CST = CSTCofins.cof99;
                    produto.COFINS.qBCProd = 0;
                    produto.COFINS.vAliqProd = 0;
                }
                if (ps.VlBaseIpi > 0)
                {
                    produto.IPI.vBC = ps.VlBaseIpi;
                    produto.IPI.pIPI = ps.PorcIpi;
                    produto.IPI.vIPI = ps.VlIpi;
                }

                produto.vTotTrib = ps.VlAproxImposto;

                notaFiscal.Produtos.Add(produto);
                i++;
                /*********************************************************************************/
                /************GGGGGGG***G*********GGGGGGG***GGGGGGG***GGGGGGG***G******************/
                /************G*********G*********G*****G***G*****G***G*****G***G******************/
                /************G**GGGG***G*********G*****G***G**GGGG***GGGGGGG***G******************/
                /************G*****G***G*********G*****G***G*****G***G*****G***G******************/
                /************GGGGGGG***GGGGGGG***GGGGGGG***GGGGGGG***G*****G***GGGGGGG************/
                /*********************************************************************************/
            }

        }

        private CSTCofins GetCstCofins(ProdutoSaidum ps)
        {
            try
            {
                int cst = Convert.ToInt32(ps.CstCofins ?? "");

                switch (cst)
                {
                    case 1: return CSTCofins.cof01;
                    case 2: return CSTCofins.cof02;
                    case 3: return CSTCofins.cof03;
                    case 4: return CSTCofins.cof04;
                    case 5: return CSTCofins.cof05;
                    case 6: return CSTCofins.cof06;
                    case 7: return CSTCofins.cof07;
                    case 8: return CSTCofins.cof08;
                    case 9: return CSTCofins.cof09;
                    case 49: return CSTCofins.cof49;
                    case 50: return CSTCofins.cof50;
                    case 51: return CSTCofins.cof51;
                    case 52: return CSTCofins.cof52;
                    case 53: return CSTCofins.cof53;
                    case 54: return CSTCofins.cof54;
                    case 55: return CSTCofins.cof55;
                    case 56: return CSTCofins.cof56;
                    case 60: return CSTCofins.cof60;
                    case 61: return CSTCofins.cof61;
                    case 62: return CSTCofins.cof62;
                    case 63: return CSTCofins.cof63;
                    case 64: return CSTCofins.cof64;
                    case 65: return CSTCofins.cof65;
                    case 66: return CSTCofins.cof66;
                    case 67: return CSTCofins.cof67;
                    case 70: return CSTCofins.cof70;
                    case 71: return CSTCofins.cof71;
                    case 72: return CSTCofins.cof72;
                    case 73: return CSTCofins.cof73;
                    case 74: return CSTCofins.cof74;
                    case 75: return CSTCofins.cof75;
                    case 98: return CSTCofins.cof98;
                    case 99: return CSTCofins.cof99;
                    default:
                        throw new Exception("CST Cofins inválido: " + cst);
                }
            }
            catch (FormatException)
            {
                throw new Exception("CST Cofins não é um número válido.");
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao converter CST Cofins: " + e.Message);
            }
        }


        private CSTPIS GetCstPis(ProdutoSaidum ps)
        {
            try
            {
                int cst = Convert.ToInt32(ps.CstPis ?? "");

                switch (cst)
                {
                    case 1: return CSTPIS.pis01;
                    case 2: return CSTPIS.pis02;
                    case 3: return CSTPIS.pis03;
                    case 4: return CSTPIS.pis04;
                    case 5: return CSTPIS.pis05;
                    case 6: return CSTPIS.pis06;
                    case 7: return CSTPIS.pis07;
                    case 8: return CSTPIS.pis08;
                    case 9: return CSTPIS.pis09;
                    case 49: return CSTPIS.pis49;
                    case 50: return CSTPIS.pis50;
                    case 51: return CSTPIS.pis51;
                    case 52: return CSTPIS.pis52;
                    case 53: return CSTPIS.pis53;
                    case 54: return CSTPIS.pis54;
                    case 55: return CSTPIS.pis55;
                    case 56: return CSTPIS.pis56;
                    case 60: return CSTPIS.pis60;
                    case 61: return CSTPIS.pis61;
                    case 62: return CSTPIS.pis62;
                    case 63: return CSTPIS.pis63;
                    case 64: return CSTPIS.pis64;
                    case 65: return CSTPIS.pis65;
                    case 66: return CSTPIS.pis66;
                    case 67: return CSTPIS.pis67;
                    case 70: return CSTPIS.pis70;
                    case 71: return CSTPIS.pis71;
                    case 72: return CSTPIS.pis72;
                    case 73: return CSTPIS.pis73;
                    case 74: return CSTPIS.pis74;
                    case 75: return CSTPIS.pis75;
                    case 98: return CSTPIS.pis98;
                    case 99: return CSTPIS.pis99;
                    default:
                        throw new Exception("CST Pis inválido: " + cst);
                }
            }
            catch (FormatException)
            {
                throw new Exception("CST Pis não é um número válido.");
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao converter CST Pis: " + e.Message);
            }
        }


        private CSTIcms GetCst(ProdutoSaidum ps)
        {
            string cst = (ps.Cst ?? "").Substring(1);
            switch (cst)
            {
                case "00":
                    return CSTIcms.cst00;
                case "01":
                    return CSTIcms.cst01;
                case "10":
                    return CSTIcms.cst10;
                case "12":
                    return CSTIcms.cst12;
                case "13":
                    return CSTIcms.cst13;
                case "14":
                    return CSTIcms.cst14;
                case "20":
                    return CSTIcms.cst20;
                case "21":
                    return CSTIcms.cst21;
                case "30":
                    return CSTIcms.cst30;
                case "40":
                    return CSTIcms.cst40;
                case "41":
                    return CSTIcms.cst41;
                case "50":
                    return CSTIcms.cst50;
                case "51":
                    return CSTIcms.cst51;
                case "60":
                    return CSTIcms.cst60;
                case "70":
                    return CSTIcms.cst70;
                case "72":
                    return CSTIcms.cst72;
                case "73":
                    return CSTIcms.cst73;
                case "74":
                    return CSTIcms.cst74;
                case "80":
                    return CSTIcms.cst80;
                case "81":
                    return CSTIcms.cst81;
                case "90":
                    return CSTIcms.cst90;
                case "91":
                    return CSTIcms.cstICMSOutraUF;
                default:
                    return CSTIcms.cst00;
            }
            throw new NotImplementedException();
        }

        private async Task<bool> GetConfiguracaoRetido(Saida saida)
        {
            ConfiguracoesEmpresa? configuracoesEmpresa =
                                    await db.ConfiguracoesEmpresas.Where(c => c.Unity == saida.Unity && c.Chave == "RETNFE").FirstOrDefaultAsync();
            if (configuracoesEmpresa == null)
            {
                return false;
            }
            else if ((!string.IsNullOrEmpty(configuracoesEmpresa.Valor1)) && configuracoesEmpresa.Valor1.Equals("N"))
            {
                return false;
            }
            return true;
        }

        private CSOSNIcms GetCsosn(ProdutoSaidum ps)
        {
            string csosn = (ps.CdCsosn ?? "").Substring(1);
            switch (csosn)
            {
                case "101":
                    return CSOSNIcms.csosn101;
                case "102":
                    return CSOSNIcms.csosn102;
                case "103":
                    return CSOSNIcms.csosn103;
                case "201":
                    return CSOSNIcms.csosn201;
                case "202":
                    return CSOSNIcms.csosn202;
                case "203":
                    return CSOSNIcms.csosn203;
                case "300":
                    return CSOSNIcms.csosn300;
                case "400":
                    return CSOSNIcms.csosn400;
                case "500":
                    return CSOSNIcms.csosn500;
                case "900":
                    return CSOSNIcms.csosn900;
                default:
                    return CSOSNIcms.csosnVazio;
            }
        }

        private OrigemMercadoria GetOrigem(string origem)
        {
            switch (origem)
            {
                case "0":
                    return OrigemMercadoria.oeNacional;
                case "1":
                    return OrigemMercadoria.oeEstrangeiraImportacaoDireta;
                case "2":
                    return OrigemMercadoria.oeEstrangeiraAdquiridaBrasil;
                case "3":
                    return OrigemMercadoria.oeNacionalConteudoImportacaoSuperior40;
                case "4":
                    return OrigemMercadoria.oeNacionalProcessosBasicos;
                case "5":
                    return OrigemMercadoria.oeNacionalConteudoImportacaoInferiorIgual40;
                case "6":
                    return OrigemMercadoria.oeEstrangeiraImportacaoDiretaSemSimilar;
                case "7":
                    return OrigemMercadoria.oeEstrangeiraAdquiridaBrasilSemSimilar;
                case "8":
                    return OrigemMercadoria.oeNacionalConteudoImportacaoSuperior70;
                default:
                    return OrigemMercadoria.oeNacional;
            }
        }

        private bool VerificaAplicacaoCEST(ProdutoSaidum ps, Saida saida, Empresa empresa)
        {
            if (string.IsNullOrEmpty(ps.Cst) && string.IsNullOrEmpty(ps.CdCsosn))
            {
                return false;
            }
            else
            if (saida.TpSaida.Equals("V") && empresa.TipoRegime <= 1 && string.IsNullOrEmpty(ps.CdCsosn))
            {
                return false;
            }
            else
            if ((!saida.TpSaida.Equals("V")) && empresa.TipoRegime <= 1 && string.IsNullOrEmpty(ps.Cst))
            {
                return false;
            }
            else
            if (empresa.TipoRegime > 1 && string.IsNullOrEmpty(ps.Cst))
            {
                return false;
            }
            else if (empresa.TipoRegime > 1 || (!saida.TpSaida.Equals("V")))
            {
                string cstSemOrigem = (ps.Cst ?? "").Substring(1);
                List<string> listaCst = new List<string> { "10", "30", "60", "70", "90" };
                return listaCst.Contains(cstSemOrigem);
            }
            else if (empresa.TipoRegime <= 1)
            {
                string csosnSemOrigem = (ps.CdCsosn ?? "").Substring(1);
                List<string> listaCsosn = new List<string> { "500", "900", "201", "202", "203" };
                return listaCsosn.Contains(csosnSemOrigem);
            }
            else
            {
                return false;
            }
        }

        private bool eUltimoItem(int tOTAL_DE_ITENS, int i)
        {
            return tOTAL_DE_ITENS == i;
        }

        private async Task GrupoDestinatario(Saida saida, NotaFiscal notaFiscal)
        {
            notaFiscal.Destinatario.CNPJCPF = UtlStrings.OnlyInteger(saida.ClienteNavigation.NrDoc ?? "");
            if (!string.IsNullOrEmpty(saida.ClienteNavigation.InscricaoEstadual ?? ""))
            {
                if ((saida.ClienteNavigation.InscricaoEstadual ?? "").Trim().ToUpper().Equals("ISENTO"))
                {
                    var ListaUFNaoPermiteIsento = new List<string> { "AM", "BA", "BA", "GO", "MG", "MS", "MT", "PE", "RN", "SE", "SP" };
                    if (ListaUFNaoPermiteIsento.Contains(saida.ClienteNavigation.CdCidadeNavigation.Uf.ToUpper()))
                    {
                        notaFiscal.Destinatario.indIEDest = IndicadorIE.inNaoContribuinte;
                        notaFiscal.Destinatario.IE = "";
                    }
                    else
                    {
                        notaFiscal.Destinatario.indIEDest = IndicadorIE.inIsento;
                        notaFiscal.Destinatario.IE = "";
                    }
                }
                else
                {
                    notaFiscal.Destinatario.indIEDest = IndicadorIE.inContribuinte;
                    notaFiscal.Destinatario.IE = (saida.ClienteNavigation.InscricaoEstadual ?? "").Trim();
                }
            }
            else
            {
                notaFiscal.Destinatario.indIEDest = IndicadorIE.inNaoContribuinte;
                notaFiscal.Destinatario.IE = "";
            }

            notaFiscal.Destinatario.xNome = saida.ClienteNavigation.NmCliente;
            notaFiscal.Destinatario.Fone = UtlStrings.OnlyInteger(saida.ClienteNavigation.Telefone ?? "");
            notaFiscal.Destinatario.ISUF = "";
            // ***************************************** notaFiscal.Destinatario.IM = "";
            if ((saida.ClienteNavigation.EMail ?? "").Length > 10)
                notaFiscal.Destinatario.Email = saida.ClienteNavigation.EMail;
            notaFiscal.Destinatario.CEP = saida.ClienteNavigation.Cep;
            notaFiscal.Destinatario.xLgr = saida.ClienteNavigation.NmEndereco;
            notaFiscal.Destinatario.xCpl = saida.ClienteNavigation.Complemento;
            notaFiscal.Destinatario.nro = saida.ClienteNavigation.Numero ?? "";
            notaFiscal.Destinatario.xBairro = saida.ClienteNavigation.NmBairro;
            notaFiscal.Destinatario.cMun = Convert.ToInt32(saida.ClienteNavigation.CdCidade);
            notaFiscal.Destinatario.xMun = saida.ClienteNavigation.CdCidadeNavigation.NmCidade;
            notaFiscal.Destinatario.UF = saida.ClienteNavigation.CdCidadeNavigation.Uf;
            notaFiscal.Destinatario.cPais = 1058;
            notaFiscal.Destinatario.xPais = "BRASIL";

            EntregaNfe? entrega = await db.EntregaNves.Where(e => e.IdCliente == saida.Cliente).FirstOrDefaultAsync();
            if (entrega != null)
            {
                notaFiscal.Entrega.CNPJCPF = UtlStrings.OnlyInteger(entrega.Cnpjcpf ?? "");
                notaFiscal.Entrega.xNome = entrega.Xnome;
                notaFiscal.Entrega.IE = entrega.Ie;
                notaFiscal.Entrega.xLgr = entrega.Xlgr;
                notaFiscal.Entrega.nro = entrega.Nro ?? "";
                notaFiscal.Entrega.xCpl = entrega.Xcpl;
                notaFiscal.Entrega.xBairro = entrega.Xbairro;
                notaFiscal.Entrega.cMun = entrega.Cmun ?? 0;
                notaFiscal.Entrega.xMun = entrega.Xmun;
                notaFiscal.Entrega.UF = entrega.Uf;
                notaFiscal.Entrega.CEP = entrega.Cep;
                notaFiscal.Entrega.Fone = entrega.Fone;
                if ((entrega.Email ?? "").Length > 10)
                    notaFiscal.Entrega.Email = entrega.Email;
                notaFiscal.Entrega.PaisCod = 1058;
                notaFiscal.Entrega.Pais = "BRASIL";
            }
            RetiradaNfe? retirada = await db.RetiradaNves.Where(e => e.IdCliente == saida.Cliente).FirstOrDefaultAsync();
            if (retirada != null)
            {
                notaFiscal.Retirada.CNPJCPF = UtlStrings.OnlyInteger(retirada.Cnpjcpf ?? "");
                notaFiscal.Retirada.xNome = retirada.Xnome;
                notaFiscal.Retirada.IE = retirada.Ie;
                notaFiscal.Retirada.xLgr = retirada.Xlgr;
                notaFiscal.Retirada.nro = retirada.Nro ?? "";
                notaFiscal.Retirada.xCpl = retirada.Xcpl;
                notaFiscal.Retirada.xBairro = retirada.Xbairro;
                notaFiscal.Retirada.cMun = retirada.Cmun ?? 0;
                notaFiscal.Retirada.xMun = retirada.Xmun;
                notaFiscal.Retirada.UF = retirada.Uf;
                notaFiscal.Retirada.CEP = retirada.Cep;
                notaFiscal.Retirada.Fone = retirada.Fone;
                if ((retirada.Email ?? "").Length > 10)
                    notaFiscal.Retirada.Email = retirada.Email;
                notaFiscal.Retirada.PaisCod = 1058;
                notaFiscal.Retirada.Pais = "BRASIL";
            }
        }

        private async Task GrupoEmitente(Saida saida, NotaFiscal notaFiscal, Empresa empresa)
        {
            notaFiscal.Emitente.CNPJCPF = empresa.CdCnpj;
            notaFiscal.Emitente.IE = empresa.NrInscrEstadual;
            notaFiscal.Emitente.xNome = empresa.NmEmpresa;
            notaFiscal.Emitente.xFant = empresa.NmEmpresa;
            notaFiscal.Emitente.Fone = empresa.Telefone;
            notaFiscal.Emitente.CEP = empresa.CdCep;
            notaFiscal.Emitente.xLgr = empresa.NmEndereco;
            notaFiscal.Emitente.xCpl = empresa.Complemento;
            notaFiscal.Emitente.nro = empresa.Numero.ToString();
            notaFiscal.Emitente.xBairro = empresa.NmBairro;
            notaFiscal.Emitente.cMun = Convert.ToInt32(empresa.CdCidade);
            notaFiscal.Emitente.cMunFG = Convert.ToInt32(empresa.CdCidade);
            notaFiscal.Emitente.xMun = empresa.CdCidadeNavigation.NmCidade;
            notaFiscal.Emitente.UF = empresa.CdCidadeNavigation.Uf;
            notaFiscal.Emitente.cPais = 1058;
            notaFiscal.Emitente.xPais = "BRASIL";

            //notaFiscal.Emitente.IEST = "";

            switch (empresa.TipoRegime ?? 1)
            {
                case 1:
                    notaFiscal.Emitente.CRT = CRT.crtSimplesNacional;
                    break;
                case 2:
                    notaFiscal.Emitente.CRT = CRT.crtSimplesExcessoReceita;
                    break;
                case 3:
                    notaFiscal.Emitente.CRT = CRT.crtRegimeNormal;
                    break;
                default:
                    notaFiscal.Emitente.CRT = CRT.crtSimplesNacional;
                    break;
            }

            notaFiscal.Emitente.IM = empresa.NrInscrMunicipal;
            notaFiscal.Emitente.CNAE = empresa.Cnae;
            notaFiscal.Emitente.IEST = empresa.Iest;
            if (((empresa.CpfcnpfAutorizado ?? "").Length > 0) && ((empresa.AutorizoXml ?? "").Equals("S")))
            {
                var aut = new AutXML();
                aut.CNPJCPF = empresa.CpfcnpfAutorizado;
                notaFiscal.AutXML.Add(aut);
            }
        }

        private async Task GrupoIde(Saida saida, NotaFiscal notaFiscal, Empresa empresa, Certificado certificado, bool isContigencia = false)
        {
            notaFiscal.Identificacao.cNF = 400;

            string CFOP = saida.ProdutoSaida.First().Cfop ?? "";
            if (CFOP.Length < 4)
            {
                throw new Exception("Grupo Identificacao: CFOP inválido");
            }

            Cfop? cfop = await db.Cfops
                .Where(c => c.CdCfop == CFOP).FirstOrDefaultAsync();
            if (cfop == null)
            {
                throw new Exception("Grupo Identificacao: CFOP não encontrado");
            }
            if ((saida.NrCnf ?? 0) <= 0)
            {
                int codigoDFe = DFeUtils.GerarCodigoDFe(saida.NrLanc);
                notaFiscal.Identificacao.cNF = codigoDFe;

                saida.NrCnf = codigoDFe;
            }
            else
            {
                notaFiscal.Identificacao.cNF = saida.NrCnf ?? 0;
            }
            notaFiscal.Identificacao.natOp = cfop.Descricao;
            if (saida.TpPagt.Equals("V"))
                notaFiscal.Identificacao.indPag = IndicadorPagamento.ipVista;
            else notaFiscal.Identificacao.indPag = IndicadorPagamento.ipPrazo;
            notaFiscal.Identificacao.mod = ModeloNFe.moNFe;
            notaFiscal.Identificacao.Serie = saida.SerieNf;
            notaFiscal.Identificacao.nNF = Convert.ToInt32(saida.NrNotaFiscal);
            notaFiscal.Identificacao.dhEmi = DateTime.Now;
            notaFiscal.Identificacao.dhSaiEnt = DateTime.Now;

            string tipoSaida = saida.TpSaida ?? "";
            switch (tipoSaida)
            {
                case "V":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "DV":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnEntrada;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnDevolucao;
                    break;
                case "E":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnEntrada;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "TR":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    break;
                case "TP":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "DC":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnDevolucao;
                    break;
                case "RA":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "CO":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnComplementar;
                    break;
                case "BO":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "AG":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
                case "OU":
                    notaFiscal.Identificacao.tpNF = TipoNFe.tnSaida;
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
            }

            string destinoOperacaoSaida = saida.TpOperacao ?? "N";
            switch (destinoOperacaoSaida)
            {
                case "N":
                    notaFiscal.Identificacao.idDest =
                        ((saida.ClienteNavigation.CdCidadeNavigation.Uf ?? "")
                        .Equals(empresa.CdCidadeNavigation.Uf ?? "")) ? DestinoOperacao.doInterna : DestinoOperacao.doInterestadual;
                    break;
                case "E":
                    notaFiscal.Identificacao.idDest = DestinoOperacao.doInterestadual;
                    break;
                case "I":
                    notaFiscal.Identificacao.idDest = DestinoOperacao.doInterna;
                    break;
                default:
                    notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
                    break;
            }

            notaFiscal.Identificacao.idDest = DestinoOperacao.doInterna;
            if ((certificado.Tipo ?? "").Equals("H"))
                notaFiscal.Identificacao.tpAmb = TipoAmbiente.taHomologacao;
            else
                notaFiscal.Identificacao.tpAmb = TipoAmbiente.taProducao;
            notaFiscal.Identificacao.tpImp = TipoDANFE.tiRetrato;
            if (isContigencia)
            {
                notaFiscal.Identificacao.tpEmis = TipoEmissao.teSVCAN;
            }
            else
                notaFiscal.Identificacao.tpEmis = TipoEmissao.teNormal;
            notaFiscal.Identificacao.cUF = DFeUtils.GetCodigoUF(empresa.CdCidadeNavigation.Uf ?? "") ?? 0;
            // ****************** notaFiscal.Identificacao.cMunFG = Convert.ToInt32(empresa.CdCidadeNavigation.CdCidade);
            //notaFiscal.Identificacao.finNFe = FinalidadeNFe.fnNormal;
            if (saida.ClienteNavigation.ConsumidorFinal)
                notaFiscal.Identificacao.indFinal = ConsumidorFinal.cfConsumidorFinal;
            else
                notaFiscal.Identificacao.indFinal = ConsumidorFinal.cfNao;
            notaFiscal.Identificacao.indPres = PresencaComprador.pcPresencial;
            notaFiscal.Identificacao.procEmi = ProcessoEmissao.peAplicativoContribuinte;
            notaFiscal.Identificacao.indIntermed = IndIntermed.iiOperacaoSemIntermediador;
            notaFiscal.Identificacao.verProc = _config["Versao:Versao"];

            if (saida.SaidaNotasDevolucaos != null && saida.SaidaNotasDevolucaos.Count() > 0)
            {
                foreach (SaidaNotasDevolucao chaveRef in saida.SaidaNotasDevolucaos)
                {
                    NFRef nFRef = new NFRef();
                    nFRef.Tipo = TipoRef.NFe;
                    nFRef.refNFe = chaveRef.ChaveAcesso;
                    notaFiscal.Identificacao.NFref.Add(nFRef);
                }
            }
        }

        public async Task<string> ValidarNFeAsync(NotaFiscal notaFiscal, Saida saida, Empresa empresa, Certificado cer)
        {
            this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
            try
            {
                string nota = notaFiscal.ToString();
                nfe.LimparLista();
                nfe.CarregarINI(nota);
                nfe.Assinar();
                nfe.Validar();
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<ResponseGerarDto> GetXmlAsync(NotaFiscal notaFiscal, Saida saida, Empresa empresa, Certificado cer)
        {
            ResponseGerarDto responseGerarDto = new ResponseGerarDto();

            this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
            string nota = notaFiscal.ToString();
            string doc = $"S{saida.NrLanc}";
            responseGerarDto.pathPdf = System.IO.Path.Combine(nfe.Config.DANFe.PathPDF, doc + ".pdf");
            if (System.IO.File.Exists(responseGerarDto.pathPdf))
            {
                System.IO.File.Delete(responseGerarDto.pathPdf);
            }

            nfe.LimparLista();
            nfe.Config.DANFe.NomeDocumento = doc;
            nfe.CarregarINI(nota);
            nfe.Assinar();
            nfe.Validar();
            responseGerarDto.xml = nfe.ObterXml(0);
            nfe.ImprimirPDF();

            return responseGerarDto;
        }

        public async Task<ResponseConsultaAcbr> ConsultaNFe(Saida saida, Empresa empresa, Certificado cer, string sessionId)
        {
            ResponseConsultaAcbr response = new ResponseConsultaAcbr();
            this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
            nfe.LimparLista();
            ConsultaNFeResposta resposta = nfe.Consultar(saida.ChaveAcessoNfe);
            response.Message = $"Situação do CTe: {resposta.XMotivo}. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
            if (resposta != null)
            {
                try
                {
                    await TratarRespostaConsulta(resposta, Enum.EnumConverter.GetStatus(resposta.CStat), saida, empresa, cer);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Erro Consulta {saida.NrLanc}");
                    response.Message = e.Message;
                }
                return response;
            }
            else
            {
                throw new Exception("Ocorreu uma falha ao consulta situação.");
            }
        }

        private async Task TratarRespostaConsulta(ConsultaNFeResposta pResposta, Status status, Saida saida, Empresa empresa, Certificado cer)
        {
            switch (pResposta.CStat)
            {
                case (int)SituacaoNFe.AutorizadoUso:
                case (int)SituacaoNFe.Cancelado:
                    SalvarGrupoAutorizacao(pResposta, status, saida, empresa, cer);
                    break;
                case (int)SituacaoNFe.Inutilizado:
                    saida.CdSituacao = "70";
                    break;
                case (int)SituacaoNFe.Denegado:

                    break;
                case (int)SituacaoNFe.NaoConstaNaSEFAZ:
                    saida.CdSituacao = "01";
                    break;

            }
        }

        private void SalvarGrupoAutorizacao(ConsultaNFeResposta resposta, Status status, Saida saida, Empresa empresa, Certificado cer)
        {
            if (string.IsNullOrEmpty(saida.XmNf))
            {
                throw new Exception("XML da NFe não encontrado. Valide a nota para prosseguir.");
            }
            var xmlAntigo = saida.XmNf ?? "";

            if (!XmlPossuiGrupoAutorizacao(xmlAntigo))
            {
                saida.XmNf = XmlTools.AdicionarGrupoAutorizacao(xmlAntigo, resposta);
                string doc = $"S{saida.NrLanc}";
                string pathPdf = System.IO.Path.Combine(nfe.Config.DANFe.PathPDF, doc + ".pdf");
                if (System.IO.File.Exists(pathPdf))
                {
                    System.IO.File.Delete(pathPdf);
                }
                nfe.LimparLista();
                nfe.Config.DANFe.NomeDocumento = doc;
                nfe.CarregarXML(saida.XmNf);
                nfe.ImprimirPDF();

                if (System.IO.File.Exists(pathPdf))
                {
                    saida.Pdf = System.IO.File.ReadAllBytes(pathPdf);
                }
                else
                {
                    throw new Exception($"PDF não registrado (SAIDA: {saida.NrLanc})");
                }
            }
            saida.CdSituacao = GetCdSituacao(status);
            if (!saida.CdSituacao.Equals("11"))
            {
                saida.NrAutorizacaoNfe = resposta.NProt;
            }
            else
            {
                saida.NrProtoCancelamento = resposta.NProt;
            }
        }

        private string? GetCdSituacao(Status status)
        {
            switch (status)
            {
                case Status.Pendente:
                    return "01";
                case Status.Autorizado:
                    return "02";
                case Status.Cancelado:
                    return "11";
                default:
                    return "01";
            }
        }

        private bool XmlPossuiGrupoAutorizacao(string xmlAntigo)
        {
            return xmlAntigo.Contains("protNFe");
        }

        public async Task<ResponseConsultaAcbr> CancelarNFe(Saida saida, Empresa empresa, Certificado cer, PostCancelamentoDto sessionHubDto)
        {
            ResponseConsultaAcbr response = new ResponseConsultaAcbr();
            try
            {
                this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
                nfe.LimparLista();
                CancelamentoNFeResposta resposta = nfe.Cancelar(saida.ChaveAcessoNfe, sessionHubDto.justificativa, empresa.CdCnpj, 1);
                response.Message = $"Cancelamento do NFe: {resposta.XMotivo}. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                const int CSTAT_DUPLICIDADE_EVENTO = 631;
                if (resposta.CStat == CSTAT_DUPLICIDADE_EVENTO)
                {
                    saida.CdSituacao = "11";
                    if (!string.IsNullOrEmpty(resposta.nProt))
                    {
                        saida.NrProtoCancelamento = resposta.nProt;
                    }
                    response.Message = $"Cancelamento já efetuado. Data. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                }
                else if (!string.IsNullOrEmpty(resposta.nProt))
                {
                    string arquivo = resposta.Arquivo;

                    string xmlContent = File.ReadAllText(arquivo);
                    saida.XmNfCnc = xmlContent;

                    string xmlNFe = saida.XmNf ?? "";

                    // ...

                    string directoryPath = @"C:\Global\NFE\Temp\XMLs";
                    string fileName = $"{saida?.ChaveAcessoNfe}-nfe.xml";
                    string filePath = System.IO.Path.Combine(directoryPath, fileName);

                    Directory.CreateDirectory(directoryPath);
                    File.WriteAllText(filePath, xmlNFe);

                    nfe.Config.DANFe.NomeDocumento = $"EVENTO{saida.NrLanc}";
                    string pathCompletoPDFEvento = System.IO.Path.Combine(nfe.Config.DANFe.PathPDF, $"EVENTO{saida.NrLanc}.pdf");
                    nfe.ImprimirEventoPDF(filePath, resposta.Arquivo);
                    if(System.IO.File.Exists(pathCompletoPDFEvento))
                    {
                        saida.PdfCnc = System.IO.File.ReadAllBytes(pathCompletoPDFEvento);
                    }

                    saida.CdSituacao = "11";
                    saida.NrProtoCancelamento = resposta.nProt;
                    response.Message = $"Cancelamento efetuado com sucesso. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Erro cancelamento {saida.NrLanc}");
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ResponseConsultaAcbr> InutilizacaoNFe(Saida saida, Empresa empresa, Certificado cer, PostCancelamentoDto sessionHubDto)
        {
            ResponseConsultaAcbr response = new ResponseConsultaAcbr();
            var date = saida.Data ?? DateUtils.DateTimeToDateOnly(DateTime.Now);
            int year = date.Year;
            try
            {
                this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
                nfe.LimparLista();
                InutilizarNFeResposta resposta = nfe.Inutilizar(empresa.CdCnpj,sessionHubDto.justificativa, year, 55, Convert.ToInt32(saida.SerieNf), Convert.ToInt32(saida.NrNotaFiscal), Convert.ToInt32(saida.NrNotaFiscal));
                response.Message = $"Cancelamento do NFe: {resposta.XMotivo}. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                const int CSTAT_DUPLICIDADE_EVENTO = 631;
                if (resposta.CStat == CSTAT_DUPLICIDADE_EVENTO)
                {
                    saida.CdSituacao = "70";
                    if (!string.IsNullOrEmpty(resposta.NProt))
                    {
                        saida.NrProtoInu = resposta.NProt;
                    }
                    response.Message = $"Inutilização já efetuada. Data. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                }
                else if (!string.IsNullOrEmpty(resposta.NProt))
                {
                    string arquivo = resposta.NomeArquivo;

                    string xmlContent = File.ReadAllText(arquivo);
                    saida.XmNfInu = xmlContent;

                    string xmlNFe = saida.XmNf ?? "";

                    // ...

                    string directoryPath = @"C:\Global\NFE\Temp\XMLs";
                    string fileName = $"{saida?.ChaveAcessoNfe}-nfe.xml";
                    string filePath = System.IO.Path.Combine(directoryPath, fileName);

                    Directory.CreateDirectory(directoryPath);
                    File.WriteAllText(filePath, xmlNFe);

                    nfe.Config.DANFe.NomeDocumento = $"INU{saida.NrLanc}";
                    string pathCompletoPDFEvento = System.IO.Path.Combine(nfe.Config.DANFe.PathPDF, $"INU{saida.NrLanc}.pdf");
                    nfe.ImprimirInutilizacaoPDF(resposta.NomeArquivo);
                    if (System.IO.File.Exists(pathCompletoPDFEvento))
                    {
                        saida.PdfInu = System.IO.File.ReadAllBytes(pathCompletoPDFEvento);
                    }

                    saida.CdSituacao = "70";
                    saida.NrProtoInu = resposta.NProt;
                    response.Message = $"Inutilização efetuada com sucesso. Data/Hora Situação: {resposta.DhRecbto.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}";
                }
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Erro cancelamento {saida.NrLanc}");
                response.Message = e.Message;
            }
            return response;
        }
    }
}
