using ACBrLib.Core;
using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils;
using GlobalErpData.Data;
using GlobalErpData.Models;
using GlobalErpData.Services;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NFe.Classes;
using System;

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
            EnvioRetornoResposta? rep = nfe.Enviar(saida.NrLanc);
            responseGerarDto.envioRetornoResposta = rep;
            if (rep.Envio.CStat.Equals(100))
            {
                responseGerarDto.xml = nfe.ObterXml(0);
                nfe.ImprimirPDF();
            }

            return responseGerarDto;
        }

        public async Task<NotaFiscal> MontarNFeAsync(Saida saida, Empresa empresa, Certificado cer, bool isContingencia = false)
        {
            var notaFiscal = new NotaFiscal();

            //infNFe
            notaFiscal.InfNFe.Versao = "4.0";

            //Identificação
            await Ide(saida, notaFiscal, empresa, cer, isContingencia);

            //Emitente
            await Emitente(saida, notaFiscal, empresa);

            //Destinatario
            await Destinatario(saida, notaFiscal);

            //Observação
            notaFiscal.DadosAdicionais.infCpl = saida.TxtObsNf ?? "";

            //Produto
            await Produto_Totais(saida, notaFiscal, empresa);



            var pagamento = new PagamentoNFe();
            pagamento.indPag = IndicadorPagamento.ipVista;
            pagamento.tPag = FormaPagamento.fpDinheiro;
            pagamento.xPag = "";
            pagamento.vPag = 100;

            notaFiscal.Pagamentos.Add(pagamento);

            return notaFiscal;
        }

        private async Task Produto_Totais(Saida saida, NotaFiscal notaFiscal, Empresa empresa)
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
                produto.vProd = ps.VlVenda;
                produto.cEANTrib = ps.CdBarra;
                produto.uTrib = ps.Un;
                produto.qTrib = ps.Quant;
                produto.vUnTrib = ps.VlVenda;
                produto.vOutro = vlOutroItem;
                produto.vFrete = vlFreteItem;
                produto.vSeg = vlSeguroItem;
                produto.vDesc = descontoItem;
                produto.infAdProd = "";
                produto.indTot = IndicadorTotal.itSomaTotalNFe;

                if (pe.ExTipi.Length > 0)
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
                produto.ICMS.orig = 0;
                produto.ICMS.CSOSN = CSOSNIcms.csosn900;
                produto.ICMS.modBC = DeterminacaoBaseIcms.dbiPrecoTabelado;
                produto.ICMS.pRedBC = 0;
                produto.ICMS.vBC = 100;
                produto.ICMS.pICMS = 18;
                produto.ICMS.vICMS = 18;
                produto.ICMS.modBCST = DeterminacaoBaseIcmsST.dbisMargemValorAgregado;

                //PIS
                produto.PIS.CST = CSTPIS.pis98;

                //COFINS
                produto.COFINS.CST = CSTCofins.cof98;

                notaFiscal.Produtos.Add(produto);
                i++;
            }
            notaFiscal.Total.vBC = 100;
            notaFiscal.Total.vICMS = 18;
            notaFiscal.Total.vBCST = 0;
            notaFiscal.Total.vST = 0;
            notaFiscal.Total.vProd = 100;
            notaFiscal.Total.vFrete = 0;
            notaFiscal.Total.vSeg = 0;
            notaFiscal.Total.vDesc = 0;
            notaFiscal.Total.vII = 0;
            notaFiscal.Total.vIPI = 0;
            notaFiscal.Total.vPIS = 0;
            notaFiscal.Total.vCOFINS = 0;
            notaFiscal.Total.vOutro = 0;
            notaFiscal.Total.vNF = 100;

            // lei da transparencia de impostos
            notaFiscal.Total.vTotTrib = 0;

            // partilha do icms e fundo de probreza
            notaFiscal.Total.vFCPUFDest = 0;
            notaFiscal.Total.vICMSUFDest = 0;
            notaFiscal.Total.vICMSUFRemet = 0;
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

        private async Task Destinatario(Saida saida, NotaFiscal notaFiscal)
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
            notaFiscal.Destinatario.IM = "";
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

        private async Task Emitente(Saida saida, NotaFiscal notaFiscal, Empresa empresa)
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

        private async Task Ide(Saida saida, NotaFiscal notaFiscal, Empresa empresa, Certificado certificado, bool isContigencia = false)
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
            notaFiscal.Identificacao.modelo = ModeloNFe.moNFe;
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
            notaFiscal.Identificacao.cMunFG = Convert.ToInt32(empresa.CdCidadeNavigation.CdCidade);
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
    }


}
