using ACBrLib.Core;
using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils;
using GlobalErpData.Data;
using GlobalErpData.Models;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public NFeGlobalService(ACBrNFe aCBrNFe,
            ILogger<NFeGlobalService> logger,
            IMapper mapper,
            IHubContext<ImportProgressHub> hubContext,
            GlobalErpFiscalBaseContext db,
            IConfiguration config)
        {
            nfe = aCBrNFe;
            _logger = logger;
            this.mapper = mapper;
            _hubContext = hubContext;
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

        public async Task GerarNFeAsync(NotaFiscal notaFiscal, Saida saida, Empresa empresa, Certificado cer)
        {
            this.SetConfiguracaoNfe(saida.Empresa, empresa, cer);
            string nota = notaFiscal.ToString();
            nfe.LimparLista();
            nfe.CarregarINI(nota);
            nfe.Enviar(1);
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

            //Produto
            var produto = new ProdutoNFe();

            produto.nItem = 1;
            produto.cProd = "123456";
            produto.cEAN = "7896523206646";
            produto.xProd = "Camisa Polo ACBr";
            produto.NCM = "61051000";
            produto.EXTIPI = "";
            produto.CFOP = "5101";
            produto.uCom = "UN";
            produto.qCom = 1;
            produto.vUnCom = 100;
            produto.vProd = 100;
            produto.cEANTrib = "7896523206646";
            produto.uTrib = "UN";
            produto.qTrib = 1;
            produto.vUnTrib = 100;
            produto.vOutro = 0;
            produto.vFrete = 0;
            produto.vSeg = 0;
            produto.vDesc = 0;
            produto.infAdProd = "Informacao Adicional do Produto";
            produto.indTot = IndicadorTotal.itSomaTotalNFe;

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

            var pagamento = new PagamentoNFe();
            pagamento.indPag = IndicadorPagamento.ipVista;
            pagamento.tPag = FormaPagamento.fpDinheiro;
            pagamento.xPag = "";
            pagamento.vPag = 100;

            notaFiscal.Pagamentos.Add(pagamento);

            return notaFiscal;
        }

        private async Task Destinatario(Saida saida, NotaFiscal notaFiscal)
        {
            notaFiscal.Destinatario.idEstrangeiro = "";
            notaFiscal.Destinatario.CNPJCPF = "99999999999999";
            notaFiscal.Destinatario.xNome = "Nome Destinatario";
            notaFiscal.Destinatario.indIEDest = IndicadorIE.inIsento;
            notaFiscal.Destinatario.IE = "ISENTO";
            notaFiscal.Destinatario.ISUF = "";
            notaFiscal.Destinatario.Email = "acbr@projetoacbr.com.br";
            notaFiscal.Destinatario.xLgr = "Rua das Flores";
            notaFiscal.Destinatario.nro = "973";
            notaFiscal.Destinatario.xCpl = "";
            notaFiscal.Destinatario.xBairro = "Centro";
            notaFiscal.Destinatario.cMun = 3550308;
            notaFiscal.Destinatario.xMun = "São Paulo";
            notaFiscal.Destinatario.UF = "SP";
            notaFiscal.Destinatario.CEP = "04615000";
            notaFiscal.Destinatario.cPais = 1058;
            notaFiscal.Destinatario.xPais = "BRASIL";
            notaFiscal.Destinatario.Fone = "(11)9999-9999";

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
            notaFiscal.Destinatario.Fone = UtlStrings.OnlyInteger(saida.ClienteNavigation.Telefone??"");
            notaFiscal.Destinatario.ISUF = "";
            notaFiscal.Destinatario.IM = "";
            if ((saida.ClienteNavigation.EMail ?? "").Length > 10)
                notaFiscal.Destinatario.Email = saida.ClienteNavigation.EMail;
            notaFiscal.Destinatario.CEP = saida.ClienteNavigation.Cep;
            notaFiscal.Destinatario.xLgr = saida.ClienteNavigation.NmEndereco;
            //notaFiscal.Destinatario.xCpl = saida.ClienteNavigation.Com;
            notaFiscal.Destinatario.nro = saida.ClienteNavigation.Numero ?? "";
            notaFiscal.Destinatario.xBairro = saida.ClienteNavigation.NmBairro;
            notaFiscal.Destinatario.cMun = Convert.ToInt32(saida.ClienteNavigation.CdCidade);
            notaFiscal.Destinatario.xMun = saida.ClienteNavigation.CdCidadeNavigation.NmCidade;
            notaFiscal.Destinatario.UF = saida.ClienteNavigation.CdCidadeNavigation.Uf;
            notaFiscal.Destinatario.cPais = 1058;
            notaFiscal.Destinatario.xPais = "BRASIL";
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
