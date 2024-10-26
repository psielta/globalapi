using ACBrLib.NFe;
using GlobalAPI_ACBrNFe.Models;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFe.Classes;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Estadual;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;
using NFe.Classes.Informacoes.Emitente;
using Npgsql;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using GlobalLib.Enum;
using AutoMapper;
using NFe.Classes.Informacoes;
using GlobalAPI_ACBrNFe.Lib;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImpNfeController : Controller
    {
        protected static ConcurrentDictionary<int, string>? ImportacaoProdutosProtect;
        protected GlobalErpFiscalBaseContext db;
        protected readonly ILogger<ImpNfeController> logger;
        protected IMapper mapper;
        private string ENDPOINT_POST_FORNECECOR;
        private string ENDPOINT_ENTRADA;
        private string ENDPOINT_PRODUTO_ENTRADA;
        private string ENDPOINT_CONTAS_PAGAR;
        private string ENDPOINT_PRODUTOS_FORN;
        private string ENDPOINT_PRODUTOS;
        private string ENDPOINT_UNIDADE_MEDIDA;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        private string ENDPOINT_TRANSPORTADORA;
        public ImpNfeController(GlobalErpFiscalBaseContext db, ILogger<ImpNfeController> logger,
            IMapper mapper,
            IHubContext<ImportProgressHub> hubContext)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            ENDPOINT_POST_FORNECECOR = Constants.URL_API_NFE + "/api/Fornecedor";
            ENDPOINT_ENTRADA = Constants.URL_API_NFE + "/api/Entrada";
            ENDPOINT_CONTAS_PAGAR = Constants.URL_API_NFE + "/api/ContasAPagar";
            ENDPOINT_PRODUTO_ENTRADA = Constants.URL_API_NFE + "/api/ProdutoEntrada";
            ENDPOINT_PRODUTOS_FORN = Constants.URL_API_NFE + "/api/ProdutosForn";
            ENDPOINT_PRODUTOS = Constants.URL_API_NFE + "/api/ProdutoEstoque";
            ENDPOINT_UNIDADE_MEDIDA = Constants.URL_API_NFE + "/api/UnidadeMedida";
            ENDPOINT_TRANSPORTADORA = Constants.URL_API_NFE + "/api/Transportadora";
            _hubContext = hubContext;

            if(ImportacaoProdutosProtect == null)
            {
                ImportacaoProdutosProtect = new ConcurrentDictionary<int, string>();
            }
        }

        [HttpPost]
        [Route("DesserializarXML")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DesserializarXML(IFormFile xml, int idEmpresa)
        {
            string _chNFe = string.Empty;
            var impNFeTemp = new ImpNFeTemp();
            using (var stream = new StreamReader(xml.OpenReadStream()))
            {
                string xmlContent = await stream.ReadToEndAsync();
                //var nfe = new NFe
                var nfe = new nfeProc().CarregarDeXmlString(xmlContent);
                if (nfe == null)
                {
                    return BadRequest(new ErrorMessage(400, "Erro ao desserializar XML"));
                }

                try
                {
                    _chNFe = GetChaveNFe(nfe.NFe);
                    var novaImpcabnfe = new Impcabnfe();
                    novaImpcabnfe.ChNfe = GetChaveNFe(nfe.NFe);

                    bool EntradaJaFoiFeita = await BuscarEntradaEmpresa(idEmpresa, GetChaveNFe(nfe.NFe));
                    if (EntradaJaFoiFeita)
                    {
                        return BadRequest(new ErrorMessage(400, "Entrada já realizada"));
                    }
                    var impcabnfe = await db.Impcabnves
                        .FirstOrDefaultAsync(i => i.ChNfe == novaImpcabnfe.ChNfe);
                    if (impcabnfe != null)
                    {
                        impNFeTemp.impcabnfe = impcabnfe;
                        var imptotalnfe = await db.Imptotalnves
                            .FirstOrDefaultAsync(i => i.ChNfe == novaImpcabnfe.ChNfe);
                        if (imptotalnfe != null)
                        {
                            impNFeTemp.imptotalnfe = imptotalnfe;
                        }
                        else
                        {
                            return NotFound(new ErrorMessage(404, "Encontrado cabeçalho, porém grupo total não foi encontrado (tbl public.imptotalnfe)."));
                        }
                        impNFeTemp.impitensnves = await db.Impitensnves.Where(imptotalnfe => imptotalnfe.ChNfe == novaImpcabnfe.ChNfe).ToListAsync();
                        if (impNFeTemp.impitensnves == null)
                        {
                            return NotFound(new ErrorMessage(404, "Encontrado cabeçalho, porém grupo itens não foi encontrado (tbl public.impitensnfe)."));
                        }
                        if (impNFeTemp.impitensnves.Count == 0)
                        {
                            return NotFound(new ErrorMessage(404, "Encontrado cabeçalho, porém grupo itens não foi encontrado (tbl public.impitensnfe)."));
                        }
                        impNFeTemp.impdupnfe = await db.Impdupnves.Where(imptotalnfe => imptotalnfe.ChNfe == novaImpcabnfe.ChNfe).ToListAsync();

                        //if (impNFeTemp.impdupnfe.Count == 0)
                        //{
                        //    return NotFound(
                        //        new ErrorMessage(404, "Encontrado cabeçalho, porém grupo duplicatas não foi encontrado (tbl public.impdupnfe)."));
                        //}

                        await GetAmarracoes(impNFeTemp, idEmpresa, nfe);

                        return Ok(impNFeTemp);
                    }

                    await InserirXmlDb(idEmpresa, GetChaveNFe(nfe.NFe), xmlContent);

                    novaImpcabnfe.Tipo = "E";
                    novaImpcabnfe.NrAutorizacao = nfe.protNFe.infProt.nProt;
                    novaImpcabnfe.Caminho = string.Empty;
                    novaImpcabnfe.XmlNota = xmlContent;
                    novaImpcabnfe.NrNf = nfe.NFe.infNFe.ide.nNF.ToString();
                    novaImpcabnfe.Modelo = nfe.NFe.infNFe.ide.mod.GetXmlEnumValue();
                    novaImpcabnfe.Serie = nfe.NFe.infNFe.ide.serie.ToString();
                    novaImpcabnfe.Cnfe = nfe.NFe.infNFe.ide.cNF.ToString();
                    novaImpcabnfe.TpPagt = nfe.NFe.infNFe.pag[0].detPag[0].indPag.GetXmlEnumValueNullable() ?? IndicadorPagamento.ipVista.GetXmlEnumValue();
                    novaImpcabnfe.TPag = nfe.NFe.infNFe.pag[0].detPag[0].tPag.GetXmlEnumValue() ?? FormaPagamento.fpDinheiro.GetXmlEnumValue();
                    novaImpcabnfe.DtEmissao = nfe.NFe.infNFe.ide.dhEmi.DateTime;
                    if (nfe.NFe.infNFe.ide.dhSaiEnt != null)
                        novaImpcabnfe.DtSaida = nfe.NFe.infNFe.ide.dhSaiEnt?.DateTime;
                    else
                        novaImpcabnfe.DtSaida = nfe.NFe.infNFe.ide.dhEmi.DateTime;
                    novaImpcabnfe.Cnpj = nfe.NFe.infNFe.emit.CNPJ;
                    novaImpcabnfe.Ie = nfe.NFe.infNFe.emit.IE;
                    novaImpcabnfe.Nome = nfe.NFe.infNFe.emit.xNome;
                    novaImpcabnfe.Fone = nfe.NFe.infNFe.emit.enderEmit.fone.ToString();
                    novaImpcabnfe.Cep = nfe.NFe.infNFe.emit.enderEmit.CEP;
                    novaImpcabnfe.Endereco = nfe.NFe.infNFe.emit.enderEmit.xLgr;
                    var nro = nfe.NFe.infNFe.emit.enderEmit.nro;
                    string NrEnd;
                    if (nro == "s/n" || nro == "S/N" || nro == "SN" || nro == "sn" || nro == ".")
                    {
                        NrEnd = "0";
                    }
                    else
                    {
                        NrEnd = nro;
                    }
                    if (NrEnd.Length > 0)
                    {
                        if (NrEnd.Length > 4)
                        {
                            novaImpcabnfe.Numero = NrEnd.Substring(0, 4);
                        }
                        else
                        {
                            novaImpcabnfe.Numero = NrEnd;
                        }
                    }
                    else
                    {
                        novaImpcabnfe.Numero = "0";
                    }
                    novaImpcabnfe.Bairro = nfe.NFe.infNFe.emit.enderEmit.xBairro;
                    novaImpcabnfe.CdCidade = nfe.NFe.infNFe.emit.enderEmit.cMun.ToString();
                    if (nfe.NFe.infNFe.avulsa != null)
                        novaImpcabnfe.CnpjAvulsa = nfe.NFe.infNFe.avulsa.CNPJ;
                    novaImpcabnfe.InfObs = nfe.NFe.infNFe.infAdic.infCpl;
                    if (nfe.NFe.infNFe.transp != null)
                    {
                        if (nfe.NFe.infNFe.transp.transporta != null)
                        {
                            if (nfe.NFe.infNFe.transp.transporta.CNPJ != null && nfe.NFe.infNFe.transp.transporta.CNPJ.Length > 0)
                                novaImpcabnfe.CnpjTransp = nfe.NFe.infNFe.transp.transporta.CNPJ;
                            else
                                novaImpcabnfe.CnpjTransp = nfe.NFe.infNFe.transp.transporta.CPF;
                            novaImpcabnfe.NomeTransp = nfe.NFe.infNFe.transp.transporta.xNome;
                            novaImpcabnfe.EndTransp = nfe.NFe.infNFe.transp.transporta.xEnder;
                            novaImpcabnfe.CidadeTransp = nfe.NFe.infNFe.transp.transporta.xMun;
                            novaImpcabnfe.UfTransp = nfe.NFe.infNFe.transp.transporta.UF;
                        }
                        else
                        {
                            novaImpcabnfe.CnpjTransp = string.Empty;
                            novaImpcabnfe.NomeTransp = string.Empty;
                            novaImpcabnfe.EndTransp = string.Empty;
                            novaImpcabnfe.CidadeTransp = string.Empty;
                            novaImpcabnfe.UfTransp = string.Empty;

                        }
                    }
                    else
                    {
                        novaImpcabnfe.CnpjTransp = string.Empty;
                        novaImpcabnfe.NomeTransp = string.Empty;
                        novaImpcabnfe.EndTransp = string.Empty;
                        novaImpcabnfe.CidadeTransp = string.Empty;
                        novaImpcabnfe.UfTransp = string.Empty;
                    }


                    await db.Impcabnves.AddAsync(novaImpcabnfe);
                    await db.SaveChangesAsync();
                    impNFeTemp.impcabnfe = novaImpcabnfe;

                    var novaImptotalsnfe = new Imptotalnfe();
                    novaImptotalsnfe.ChNfe = GetChaveNFe(nfe.NFe);
                    novaImptotalsnfe.IcmsVbc = nfe.NFe.infNFe.total.ICMSTot.vBC.ToString();
                    novaImptotalsnfe.IcmsValor = nfe.NFe.infNFe.total.ICMSTot.vICMS.ToString();
                    novaImptotalsnfe.IcmsVbcst = nfe.NFe.infNFe.total.ICMSTot.vBCST.ToString();
                    novaImptotalsnfe.IcmsSt = nfe.NFe.infNFe.total.ICMSTot.vST.ToString();
                    novaImptotalsnfe.IcmsVprod = nfe.NFe.infNFe.total.ICMSTot.vProd.ToString();
                    novaImptotalsnfe.IcmsFrete = nfe.NFe.infNFe.total.ICMSTot.vFrete.ToString();
                    novaImptotalsnfe.IcmsSeg = nfe.NFe.infNFe.total.ICMSTot.vSeg.ToString();
                    novaImptotalsnfe.IcmsDesc = nfe.NFe.infNFe.total.ICMSTot.vDesc.ToString();
                    novaImptotalsnfe.IcmsVii = nfe.NFe.infNFe.total.ICMSTot.vII.ToString();
                    novaImptotalsnfe.IcmsVipi = nfe.NFe.infNFe.total.ICMSTot.vIPI.ToString();
                    novaImptotalsnfe.IcmsVpis = nfe.NFe.infNFe.total.ICMSTot.vPIS.ToString();
                    novaImptotalsnfe.IcmsVconfins = nfe.NFe.infNFe.total.ICMSTot.vCOFINS.ToString();
                    novaImptotalsnfe.IcmsOutros = nfe.NFe.infNFe.total.ICMSTot.vOutro.ToString();
                    novaImptotalsnfe.IcmsVnf = nfe.NFe.infNFe.total.ICMSTot.vNF.ToString();
                    if (nfe.NFe.infNFe.total.retTrib != null)
                    {
                        novaImptotalsnfe.RetPis = nfe.NFe.infNFe.total.retTrib.vRetPIS.ToString();
                        novaImptotalsnfe.RetConfins = nfe.NFe.infNFe.total.retTrib.vRetCOFINS.ToString();
                        novaImptotalsnfe.RetCsll = nfe.NFe.infNFe.total.retTrib.vRetCSLL.ToString();
                        novaImptotalsnfe.RetIrrf = nfe.NFe.infNFe.total.retTrib.vIRRF.ToString();
                        novaImptotalsnfe.RetBirrf = nfe.NFe.infNFe.total.retTrib.vBCIRRF.ToString();
                        novaImptotalsnfe.RetBcvprev = nfe.NFe.infNFe.total.retTrib.vBCRetPrev.ToString();
                        novaImptotalsnfe.RetVprev = nfe.NFe.infNFe.total.retTrib.vRetPrev.ToString();
                    }
                    novaImptotalsnfe.Vicmsdeson = nfe.NFe.infNFe.total.ICMSTot.vICMSDeson.ToString();
                    novaImptotalsnfe.IcmsVfcpst = nfe.NFe.infNFe.total.ICMSTot.vFCPST.ToString();
                    novaImptotalsnfe.IcmsVipidevol = nfe.NFe.infNFe.total.ICMSTot.vIPIDevol.ToString();

                    await db.Imptotalnves.AddAsync(novaImptotalsnfe);
                    await db.SaveChangesAsync();
                    impNFeTemp.imptotalnfe = novaImptotalsnfe;

                    for (int i = 0; i < nfe.NFe.infNFe.det.Count; i++)
                    {
                        var det = nfe.NFe.infNFe.det[i];
                        var novoImpitensnfe = new Impitensnfe();
                        novoImpitensnfe.ChNfe = GetChaveNFe(nfe.NFe);
                        novoImpitensnfe.NrItem = det.nItem.ToString();
                        novoImpitensnfe.CProd = det.prod.cProd;
                        novoImpitensnfe.Cean = det.prod.cEANTrib;
                        novoImpitensnfe.Nome = det.prod.xProd;
                        novoImpitensnfe.Ncm = det.prod.NCM;
                        novoImpitensnfe.Cest = det.prod.CEST;
                        novoImpitensnfe.Extipi = det.prod.EXTIPI;
                        novoImpitensnfe.Cfop = det.prod.CFOP.ToString();
                        novoImpitensnfe.Un = det.prod.uCom;
                        novoImpitensnfe.Quant = det.prod.qCom.ToString();
                        novoImpitensnfe.VlUnit = det.prod.vUnCom.ToString();
                        novoImpitensnfe.Utrib = det.prod.uTrib;
                        novoImpitensnfe.Qtrib = det.prod.qTrib.ToString();
                        novoImpitensnfe.Vuntrib = det.prod.vUnTrib.ToString();
                        novoImpitensnfe.VlTotal = det.prod.vProd.ToString();
                        novoImpitensnfe.Vldesc = det.prod.vDesc.ToString();
                        novoImpitensnfe.FreteProduto = det.prod.vFrete.ToString();
                        novoImpitensnfe.Pis = "N";
                        novoImpitensnfe.Confins = "N";
                        novoImpitensnfe.Iss = "N";
                        novoImpitensnfe.Ipi = "N";
                        novoImpitensnfe.Ii = "N";
                        novoImpitensnfe.Cofinsst = "N";
                        novoImpitensnfe.Pisst = "N";

                        if (det.prod.ProdutoEspecifico != null)
                        {
                            for (int k = 0; k < det.prod.ProdutoEspecifico.Count; k++)
                            {
                                if (det.prod.ProdutoEspecifico[k] is veicProd)
                                {
                                    var veicProd = (veicProd)det.prod.ProdutoEspecifico[k];
                                    novoImpitensnfe.VeicTpop = veicProd.tpOp.ToString();
                                    novoImpitensnfe.VeicChassi = veicProd.chassi;
                                    novoImpitensnfe.VeicCcordenatran = veicProd.cCorDENATRAN;
                                    novoImpitensnfe.VeciCcor = veicProd.cCor;
                                    novoImpitensnfe.VeicXcor = veicProd.xCor;
                                    novoImpitensnfe.VeicPot = veicProd.pot;
                                    novoImpitensnfe.VeicCilin = veicProd.cilin;
                                    novoImpitensnfe.VeicPesol = veicProd.pesoL;
                                    novoImpitensnfe.VeicPesob = veicProd.pesoB;
                                    novoImpitensnfe.VeicNserie = veicProd.nSerie;
                                    novoImpitensnfe.VeicTpcomb = veicProd.tpComb;
                                    novoImpitensnfe.VeicCmt = veicProd.CMT;
                                    novoImpitensnfe.VeicDist = veicProd.dist;
                                    novoImpitensnfe.VeicAnomod = veicProd.anoMod.ToString();
                                    novoImpitensnfe.VeicAnofab = veicProd.anoFab.ToString();
                                    novoImpitensnfe.VeicTppint = veicProd.tpPint;
                                    novoImpitensnfe.VeicTpveic = veicProd.tpVeic.ToString();
                                    novoImpitensnfe.VeicEspveic = veicProd.espVeic.ToString();
                                    novoImpitensnfe.VeicCondveic = veicProd.condVeic.ToString();
                                    novoImpitensnfe.VeicCmod = veicProd.cMod;
                                    novoImpitensnfe.VeicLota = veicProd.lota.ToString();
                                    novoImpitensnfe.VeicTprest = veicProd.tpRest.ToString();
                                    novoImpitensnfe.VeicVin = veicProd.VIN.ToString();
                                    novoImpitensnfe.VeicNmotor = veicProd.nMotor.ToString();
                                }
                                if (det.prod.ProdutoEspecifico[k] is med)
                                {
                                    var medProd = (med)det.prod.ProdutoEspecifico[k];
                                    novoImpitensnfe.DtValid = medProd.dVal;
                                    novoImpitensnfe.Lote = medProd.nLote;
                                }
                            }
                        }

                        if (det.prod.rastro != null)
                        {
                            if (det.prod.rastro.Count > 0)
                            {
                                novoImpitensnfe.DtValid = det.prod.rastro[0].dVal;
                                novoImpitensnfe.Lote = det.prod.rastro[0].nLote;
                            }
                        }

                        novoImpitensnfe.VlOutros = det.prod.vOutro;

                        var imposto = det.imposto;

                        if (imposto != null)
                        {
                            if (imposto.ICMS != null)
                            {
                                var abstractICMS = imposto.ICMS.TipoICMS;

                                ICMS02? iCMS02 = null;
                                ICMS10? iCMS10 = null;
                                ICMS15? iCMS15 = null;
                                ICMS20? iCMS20 = null;
                                ICMS30? iCMS30 = null;
                                ICMS40? iCMS40 = null;
                                ICMS51? iCMS51 = null;
                                ICMS53? iCMS53 = null;
                                ICMS60? iCMS60 = null;
                                ICMS61? iCMS61 = null;
                                ICMS70? iCMS70 = null;
                                ICMS90? iCMS90 = null;
                                ICMSPart? iCMSPart = null;
                                ICMSST? iCMSST = null;
                                ICMSSN101? iCMSSN101 = null;
                                ICMSSN102? iCMSSN102 = null;
                                ICMSSN201? iCMSSN201 = null;
                                ICMSSN202? iCMSSN202 = null;
                                ICMSSN500? iCMSSN500 = null;
                                ICMSSN900? iCMSSN900 = null;

                                if (abstractICMS is ICMS02)
                                {
                                    iCMS02 = (ICMS02)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS02.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS02.orig.GetXmlEnumValue();
                                }
                                else if (abstractICMS is ICMS10)
                                {
                                    iCMS10 = (ICMS10)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS10.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS10.orig.GetXmlEnumValue();
                                    novoImpitensnfe.ModBc = iCMS10.modBC.GetXmlEnumValue();
                                    novoImpitensnfe.Predbc = iCMS10.pRedBCST.ToString();
                                    novoImpitensnfe.Vbc = iCMS10.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMS10.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMS10.vICMS.ToString();
                                    /* 
                                     TbItensNFEPCRED.AsCurrency := ICMS.pCredSN;
                                     TbItensNFEVL_CRED_ICMS.AsFloat := ICMS.vCredICMSSN;
                                     TbItensNFEpST.AsString := Currtostr(ICMS.pST);*/
                                    novoImpitensnfe.Modbcst = iCMS10.modBCST.GetXmlEnumValue();
                                    novoImpitensnfe.Pmvast = iCMS10.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMS10.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMS10.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMS10.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMS10.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMS10.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS10.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMS10.vFCPST.ToString();

                                }
                                else if (abstractICMS is ICMS15)
                                {
                                    iCMS15 = (ICMS15)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS15.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS15.orig.GetXmlEnumValue();
                                }
                                else if (abstractICMS is ICMS20)
                                {
                                    iCMS20 = (ICMS20)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS20.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS20.orig.GetXmlEnumValue();
                                    novoImpitensnfe.ModBc = iCMS20.modBC.GetXmlEnumValue();
                                    novoImpitensnfe.Predbc = iCMS20.pRedBC.ToString();
                                    novoImpitensnfe.Vbc = iCMS20.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMS20.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMS20.vICMS.ToString();

                                    novoImpitensnfe.Vicmsdeson = iCMS20.vICMSDeson.ToString();

                                    novoImpitensnfe.FcpBase = iCMS20.vBCFCP.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS20.pFCP.ToString();
                                    novoImpitensnfe.FcpValor = iCMS20.vFCP.ToString();

                                }
                                else if (abstractICMS is ICMS30)
                                {
                                    iCMS30 = (ICMS30)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS30.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS30.orig.GetXmlEnumValue();

                                    novoImpitensnfe.Vicmsdeson = iCMS30.vICMSDeson.ToString();

                                    novoImpitensnfe.Modbcst = iCMS30.modBCST.GetXmlEnumValue();
                                    novoImpitensnfe.Pmvast = iCMS30.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMS30.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMS30.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMS30.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMS30.vICMSST.ToString();

                                    novoImpitensnfe.FcpBase = iCMS30.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS30.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMS30.vFCPST.ToString();

                                }
                                else if (abstractICMS is ICMS40)
                                {
                                    iCMS40 = (ICMS40)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS40.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS40.orig.GetXmlEnumValue();

                                    novoImpitensnfe.Vicmsdeson = iCMS40.vICMSDeson.ToString();
                                }
                                else if (abstractICMS is ICMS51)
                                {
                                    iCMS51 = (ICMS51)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS51.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS51.orig.GetXmlEnumValue();

                                    novoImpitensnfe.ModBc = iCMS51.modBC.GetXmlEnumValueNullable();
                                    novoImpitensnfe.Predbc = iCMS51.pRedBC.ToString();

                                    novoImpitensnfe.Vbc = iCMS51.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMS51.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMS51.vICMS.ToString();

                                    novoImpitensnfe.FcpBase = iCMS51.vBCFCP.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS51.pFCP.ToString();
                                    novoImpitensnfe.FcpValor = iCMS51.vFCP.ToString();

                                }
                                else if (abstractICMS is ICMS53)
                                {
                                    iCMS53 = (ICMS53)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS53.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS53.orig.GetXmlEnumValue();
                                }
                                else if (abstractICMS is ICMS60)
                                {
                                    iCMS60 = (ICMS60)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS60.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS60.orig.GetXmlEnumValue();

                                    novoImpitensnfe.Pst = iCMS60.pST.ToString();
                                    novoImpitensnfe.Vbcstret = iCMS60.vBCSTRet.ToString();
                                    novoImpitensnfe.Vicmsstret = iCMS60.vICMSSTRet.ToString();


                                }
                                else if (abstractICMS is ICMS61)
                                {
                                    iCMS61 = (ICMS61)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS61.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS61.orig.GetXmlEnumValue();
                                }
                                else if (abstractICMS is ICMS70)
                                {
                                    iCMS70 = (ICMS70)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS70.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS70.orig.GetXmlEnumValue();

                                    novoImpitensnfe.ModBc = iCMS70.modBC.GetXmlEnumValue();
                                    novoImpitensnfe.Predbc = iCMS70.pRedBC.ToString();
                                    novoImpitensnfe.Vbc = iCMS70.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMS70.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMS70.vICMS.ToString();
                                    novoImpitensnfe.Pmvast = iCMS70.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMS70.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMS70.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMS70.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMS70.vICMSST.ToString();
                                    //dois tipos de fcp
                                    novoImpitensnfe.FcpBase = iCMS70.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS70.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMS70.vFCPST.ToString();

                                    novoImpitensnfe.Vicmsdeson = iCMS70.vICMSDeson.ToString();
                                }
                                else if (abstractICMS is ICMS90)
                                {
                                    iCMS90 = (ICMS90)abstractICMS;
                                    novoImpitensnfe.Cst = iCMS90.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMS90.orig.GetXmlEnumValue();

                                    novoImpitensnfe.ModBc = iCMS90.modBC.GetXmlEnumValueNullable();
                                    novoImpitensnfe.Predbc = iCMS90.pRedBC.ToString();
                                    novoImpitensnfe.Vbc = iCMS90.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMS90.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMS90.vICMS.ToString();
                                    novoImpitensnfe.Modbcst = iCMS90.modBCST.ToString();
                                    novoImpitensnfe.Pmvast = iCMS90.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMS90.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMS90.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMS90.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMS90.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMS90.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMS90.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMS90.vFCPST.ToString();
                                    novoImpitensnfe.Vicmsdeson = iCMS90.vICMSDeson.ToString();
                                }
                                else if (abstractICMS is ICMSPart)
                                {
                                    iCMSPart = (ICMSPart)abstractICMS;
                                    novoImpitensnfe.Cst = iCMSPart.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSPart.orig.GetXmlEnumValue();

                                    novoImpitensnfe.ModBc = iCMSPart.modBC.GetXmlEnumValue();
                                    novoImpitensnfe.Predbc = iCMSPart.pRedBC.ToString();
                                    novoImpitensnfe.Vbc = iCMSPart.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMSPart.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMSPart.vICMS.ToString();
                                    novoImpitensnfe.Modbcst = iCMSPart.modBCST.ToString();
                                    novoImpitensnfe.Pmvast = iCMSPart.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMSPart.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMSPart.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMSPart.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMSPart.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMSPart.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMSPart.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMSPart.vFCPST.ToString();
                                }
                                else if (abstractICMS is ICMSST)
                                {
                                    iCMSST = (ICMSST)abstractICMS;
                                    novoImpitensnfe.Cst = iCMSST.CST.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSST.orig.GetXmlEnumValue();

                                    novoImpitensnfe.Pst = iCMSST.pST.ToString();
                                    novoImpitensnfe.Vbcstret = iCMSST.vBCSTRet.ToString();
                                    novoImpitensnfe.Vicmsstret = iCMSST.vICMSSTRet.ToString();

                                }
                                else if (abstractICMS is ICMSSN101)
                                {
                                    iCMSSN101 = (ICMSSN101)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN101.CSOSN.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSSN101.orig.GetXmlEnumValue();
                                    novoImpitensnfe.Pcred = iCMSSN101.pCredSN.ToString();
                                    novoImpitensnfe.VlCredIcms = iCMSSN101.vCredICMSSN.ToString();
                                }
                                else if (abstractICMS is ICMSSN102)
                                {
                                    iCMSSN102 = (ICMSSN102)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN102.CSOSN.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSSN102.orig.GetXmlEnumValue();

                                }
                                else if (abstractICMS is ICMSSN201)
                                {
                                    iCMSSN201 = (ICMSSN201)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN201.CSOSN.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSSN201.orig.GetXmlEnumValue();
                                    novoImpitensnfe.Pmvast = iCMSSN201.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMSSN201.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMSSN201.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMSSN201.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMSSN201.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMSSN201.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMSSN201.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMSSN201.vFCPST.ToString();
                                    novoImpitensnfe.Pcred = iCMSSN201.pCredSN.ToString();
                                    novoImpitensnfe.VlCredIcms = iCMSSN201.vCredICMSSN.ToString();

                                }
                                else if (abstractICMS is ICMSSN202)
                                {
                                    iCMSSN202 = (ICMSSN202)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN202.CSOSN.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSSN202.orig.GetXmlEnumValue();
                                    novoImpitensnfe.Pmvast = iCMSSN202.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMSSN202.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMSSN202.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMSSN202.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMSSN202.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMSSN202.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMSSN202.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMSSN202.vFCPST.ToString();

                                }
                                else if (abstractICMS is ICMSSN500)
                                {
                                    iCMSSN500 = (ICMSSN500)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN500.CSOSN.ToString();
                                    novoImpitensnfe.ImpOrigem = iCMSSN500.orig.ToString();
                                    novoImpitensnfe.Pst = iCMSSN500.pST.ToString();
                                    novoImpitensnfe.Vbcstret = iCMSSN500.vBCSTRet.ToString();
                                    novoImpitensnfe.Vicmsstret = iCMSSN500.vICMSSTRet.ToString();
                                    //depois ver o que fazer com _vIcmsSubstituto

                                }
                                else if (abstractICMS is ICMSSN900)
                                {
                                    iCMSSN900 = (ICMSSN900)abstractICMS;
                                    novoImpitensnfe.Csosn = iCMSSN900.CSOSN.GetXmlEnumValue();
                                    novoImpitensnfe.ImpOrigem = iCMSSN900.orig.GetXmlEnumValue();
                                    novoImpitensnfe.ModBc = iCMSSN900.modBC.GetXmlEnumValueNullable();
                                    novoImpitensnfe.Predbc = iCMSSN900.pRedBC.ToString();
                                    novoImpitensnfe.Vbc = iCMSSN900.vBC.ToString();
                                    novoImpitensnfe.Picms = iCMSSN900.pICMS.ToString();
                                    novoImpitensnfe.Vicms = iCMSSN900.vICMS.ToString();
                                    novoImpitensnfe.Modbcst = iCMSSN900.modBCST.ToString();
                                    novoImpitensnfe.Pmvast = iCMSSN900.pMVAST.ToString();
                                    novoImpitensnfe.Predbcst = iCMSSN900.pRedBCST.ToString();
                                    novoImpitensnfe.Vbcst = iCMSSN900.vBCST.ToString();
                                    novoImpitensnfe.Picmsst = iCMSSN900.pICMSST.ToString();
                                    novoImpitensnfe.Vicmsst = iCMSSN900.vICMSST.ToString();
                                    novoImpitensnfe.FcpBase = iCMSSN900.vBCFCPST.ToString();
                                    novoImpitensnfe.FcpPorc = iCMSSN900.pFCPST.ToString();
                                    novoImpitensnfe.FcpValor = iCMSSN900.vFCPST.ToString();

                                }
                            };

                            if (imposto.ISSQN != null)
                            {
                                var issqn = imposto.ISSQN;
                                novoImpitensnfe.Iss = "S";
                                novoImpitensnfe.Bciss = issqn.vBC.ToString();
                                novoImpitensnfe.Valiq = issqn.vAliq.ToString();
                                novoImpitensnfe.Vissqn = issqn.vISSQN.ToString();
                                novoImpitensnfe.Cmunfg = issqn.cMunFG.ToString();
                                novoImpitensnfe.Clistserv = issqn.cListServ;
                            };
                            if (imposto.IPI != null)
                            {
                                var ipi = imposto.IPI;
                                novoImpitensnfe.Ipicnpjprod = ipi.CNPJProd;
                                novoImpitensnfe.Ipi = "S";
                                novoImpitensnfe.Ipicenq = ipi.cEnq.ToString();
                                novoImpitensnfe.Ipicselo = ipi.cSelo;
                                novoImpitensnfe.Ipiqselo = ipi.qSelo;
                                novoImpitensnfe.Ipiclenq = ipi.clEnq;


                                IPITrib? iPITrib = null;
                                IPINT? iPINT = null;
                                if (imposto.IPI != null)
                                {
                                    if (ipi.TipoIPI is IPINT)
                                    {
                                        iPINT = (IPINT)ipi.TipoIPI;
                                        novoImpitensnfe.Ipicst = iPINT.CST.GetXmlEnumValue();
                                    }
                                    else if (ipi.TipoIPI is IPITrib)
                                    {
                                        iPITrib = (IPITrib)ipi.TipoIPI;
                                        novoImpitensnfe.Ipicst = iPITrib.CST.GetXmlEnumValue();
                                        novoImpitensnfe.Ipivipi = iPITrib.vIPI.ToString();
                                        novoImpitensnfe.Ipivbc = iPITrib.vBC.ToString();
                                        novoImpitensnfe.Ipipipi = iPITrib.pIPI.ToString();
                                        novoImpitensnfe.Ipiqunid = iPITrib.qUnid.ToString();
                                        novoImpitensnfe.Ipivunid = iPITrib.vUnid.ToString();
                                        novoImpitensnfe.Ipiqunid = iPITrib.qUnid.ToString();
                                        novoImpitensnfe.Ipivunid = iPITrib.vUnid.ToString();

                                    }
                                }
                            }

                            if (imposto.II != null)
                            {
                                var ii = imposto.II;
                                novoImpitensnfe.Ii = "S";
                                novoImpitensnfe.Iivbc = ii.vBC.ToString();
                                novoImpitensnfe.Iivii = ii.vII.ToString();
                                novoImpitensnfe.Iiviof = ii.vIOF.ToString();
                                novoImpitensnfe.Iivdespadu = ii.vDespAdu.ToString();
                            }

                            if (imposto.PIS != null)
                            {
                                var pis = imposto.PIS;
                                novoImpitensnfe.Pis = "S";
                                PISAliq? pISAliq = null;
                                PISNT? pISNT = null;
                                PISOutr? pISOutr = null;
                                PISQtde? pISQtde = null;
                                if (pis.TipoPIS is PISAliq)
                                {
                                    pISAliq = (PISAliq)pis.TipoPIS;
                                    novoImpitensnfe.PisCst = pISAliq.CST.GetXmlEnumValue();
                                    novoImpitensnfe.Pisvbc = pISAliq.vBC.ToString();
                                    novoImpitensnfe.Pisppis = pISAliq.pPIS.ToString();
                                    novoImpitensnfe.Pisvpis = pISAliq.vPIS.ToString();
                                }
                                else if (pis.TipoPIS is PISNT)
                                {
                                    pISNT = (PISNT)pis.TipoPIS;
                                    novoImpitensnfe.PisCst = pISNT.CST.GetXmlEnumValue();
                                }
                                else if (pis.TipoPIS is PISOutr)
                                {
                                    pISOutr = (PISOutr)pis.TipoPIS;
                                    novoImpitensnfe.PisCst = pISOutr.CST.GetXmlEnumValue();
                                    novoImpitensnfe.Pisvbc = pISOutr.vBC.ToString();
                                    novoImpitensnfe.Pisppis = pISOutr.pPIS.ToString();
                                    novoImpitensnfe.Pisvpis = pISOutr.vPIS.ToString();
                                    novoImpitensnfe.Pisqbcprod = pISOutr.qBCProd.ToString();
                                    novoImpitensnfe.Pisvaliqprod = pISOutr.vAliqProd.ToString();
                                }
                                else if (pis.TipoPIS is PISQtde)
                                {
                                    pISQtde = (PISQtde)pis.TipoPIS;
                                    novoImpitensnfe.PisCst = pISQtde.CST.GetXmlEnumValue();
                                    novoImpitensnfe.Pisqbcprod = pISQtde.qBCProd.ToString();
                                    novoImpitensnfe.Pisvaliqprod = pISQtde.vAliqProd.ToString();
                                    novoImpitensnfe.Pisvpis = pISQtde.vPIS.ToString();
                                }
                            }
                            if (imposto.PISST != null)
                            {
                                var pisst = imposto.PISST;
                                novoImpitensnfe.Pisst = "S";
                                novoImpitensnfe.Pisstvbc = pisst.vBC.ToString();
                                novoImpitensnfe.Pisstppis = pisst.pPIS.ToString();
                                novoImpitensnfe.Pisstqbcprod = pisst.qBCProd.ToString();
                                novoImpitensnfe.Pisstvaliqprod = pisst.vAliqProd.ToString();
                                novoImpitensnfe.Pisstvpis = pisst.vPIS.ToString();
                            }
                            if (imposto.COFINS != null)
                            {
                                COFINSAliq? cOFINSAliq = null;
                                COFINSNT? cOFINSNT = null;
                                COFINSOutr? cOFINSOutr = null;
                                COFINSQtde? cOFINSQtde = null;

                                if (imposto.COFINS.TipoCOFINS is COFINSAliq)
                                {
                                    cOFINSAliq = (COFINSAliq)imposto.COFINS.TipoCOFINS;
                                    novoImpitensnfe.Confins = "S";
                                    novoImpitensnfe.CofCst = cOFINSAliq.CST.GetXmlEnumValue();
                                    novoImpitensnfe.CofVbc = cOFINSAliq.vBC.ToString();
                                    novoImpitensnfe.CofPcofins = cOFINSAliq.pCOFINS.ToString();
                                    novoImpitensnfe.CofVcofins = cOFINSAliq.vCOFINS.ToString();
                                }
                                else if (imposto.COFINS.TipoCOFINS is COFINSNT)
                                {
                                    cOFINSNT = (COFINSNT)imposto.COFINS.TipoCOFINS;
                                    novoImpitensnfe.Confins = "S";
                                    novoImpitensnfe.CofCst = cOFINSNT.CST.GetXmlEnumValue();
                                }
                                else if (imposto.COFINS.TipoCOFINS is COFINSOutr)
                                {
                                    cOFINSOutr = (COFINSOutr)imposto.COFINS.TipoCOFINS;
                                    novoImpitensnfe.Confins = "S";
                                    novoImpitensnfe.CofCst = cOFINSOutr.CST.GetXmlEnumValue();
                                    novoImpitensnfe.CofVbc = cOFINSOutr.vBC.ToString();
                                    novoImpitensnfe.CofPcofins = cOFINSOutr.pCOFINS.ToString();
                                    novoImpitensnfe.CofVcofins = cOFINSOutr.vCOFINS.ToString();
                                    novoImpitensnfe.CofQbcprod = cOFINSOutr.qBCProd.ToString();
                                    novoImpitensnfe.CofstValiqprod = cOFINSOutr.vAliqProd.ToString();
                                }
                                else if (imposto.COFINS.TipoCOFINS is COFINSQtde)
                                {
                                    cOFINSQtde = (COFINSQtde)imposto.COFINS.TipoCOFINS;
                                    novoImpitensnfe.Confins = "S";
                                    novoImpitensnfe.CofCst = cOFINSQtde.CST.GetXmlEnumValue();
                                    novoImpitensnfe.CofQbcprod = cOFINSQtde.qBCProd.ToString();
                                    novoImpitensnfe.CofstValiqprod = cOFINSQtde.vAliqProd.ToString();
                                    novoImpitensnfe.CofVcofins = cOFINSQtde.vCOFINS.ToString();
                                }
                            }
                            if (imposto.COFINSST != null)
                            {
                                var cofinsst = imposto.COFINSST;
                                novoImpitensnfe.Cofinsst = "S";
                                novoImpitensnfe.CofstVbc = cofinsst.vBC.ToString();
                                novoImpitensnfe.CofstPcofins = cofinsst.pCOFINS.ToString();
                                novoImpitensnfe.CofstQbcprod = cofinsst.qBCProd.ToString();
                                novoImpitensnfe.CofstValiqprod = cofinsst.vAliqProd.ToString();
                                novoImpitensnfe.CofstVcofins = cofinsst.vCOFINS.ToString();
                            }

                        }

                        await db.Impitensnves.AddAsync(novoImpitensnfe);
                        await db.SaveChangesAsync();
                        if (impNFeTemp.impitensnves == null)
                        {
                            impNFeTemp.impitensnves = new List<Impitensnfe>();
                        }
                        impNFeTemp.impitensnves.Add(novoImpitensnfe);


                    }
                    var cobr = nfe.NFe.infNFe.cobr;
                    if (cobr != null)
                    {
                        if (cobr.dup != null)
                        {
                            for (int d = 0; d < cobr.dup.Count; d++)
                            {
                                var dup = cobr.dup[d];
                                var novaDuplNfe = new Impdupnfe();
                                novaDuplNfe.ChNfe = novaImpcabnfe.ChNfe;
                                novaDuplNfe.NrDup = dup.nDup.ToString();
                                novaDuplNfe.DtVenc = dup.dVenc;
                                novaDuplNfe.Valor = dup.vDup.ToString();

                                await db.Impdupnves.AddAsync(novaDuplNfe);
                                await db.SaveChangesAsync();
                                if (impNFeTemp.impdupnfe == null)
                                {
                                    impNFeTemp.impdupnfe = new List<Impdupnfe>();
                                }
                                impNFeTemp.impdupnfe.Add(novaDuplNfe);
                            }
                        }
                    }


                    await GetAmarracoes(impNFeTemp, idEmpresa, nfe);

                    return Ok(impNFeTemp);
                }
                catch (DbUpdateException dbEx)
                {
                    logger.LogError(dbEx, "Erro ao salvar dessarializar XML para banco de dados 'public.impcabnfe'");
                    await TentaExcluirAsync(_chNFe);
                    return StatusCode(500, new ErrorMessage(500, dbEx.InnerException?.Message ?? "An unknown database error occurred."));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro ao salvar dessarializar XML para banco de dados 'public.impcabnfe'");
                    await TentaExcluirAsync(_chNFe);
                    return StatusCode(500, new ErrorMessage(500, ex.Message));

                }
            }
        }

        private async Task InserirXmlDb(int idEmpresa, string v, string xmlContent)
        {
            string SQL = $@"SELECT 
                              id_empresa,
                              chave_acesso,
                              type,
                              xml
                            FROM 
                              public.impxml
                            WHERE
                              id_empresa = {idEmpresa}
                              AND chave_acesso = '{v}'
                              AND type = 0
                              ";
            var impXml = await db.Impxmls.FromSqlRaw(SQL).FirstOrDefaultAsync();
            if (impXml == null)
            {
                Impxml novoImpXml = new Impxml();
                novoImpXml.IdEmpresa = idEmpresa;
                novoImpXml.ChaveAcesso = v;
                novoImpXml.Type = 0;
                novoImpXml.Xml = xmlContent;
                await db.Impxmls.AddAsync(novoImpXml);
                await db.SaveChangesAsync();
            }
            else
            {
                impXml.Xml = xmlContent;
                db.Impxmls.Update(impXml);
                await db.SaveChangesAsync();
            }
        }

        private async Task<bool> BuscarEntradaEmpresa(int idEmpresa, string chaveAcesso)
        {

            string SQL = $@"
                SELECT 
                  *
                FROM 
                  public.entradas e
                WHERE
                  e.cd_chave_nfe = '{chaveAcesso}'
                  AND e.cd_empresa = {idEmpresa}
                ";
            var entrada = await db.Entradas.FromSqlRaw(SQL).FirstOrDefaultAsync();
            if (entrada != null)
            {
                return true;
            }
            return false;
        }

        private async Task GetAmarracoes(ImpNFeTemp impNFeTemp, int idEmpresa, nfeProc nfe)
        {
            if (impNFeTemp.impitensnves == null)
                throw new Exception("Não foi possível salvar os itens da nota fiscal, verifique se o XML está correto.");

            if (impNFeTemp.amarracoes == null)
            {
                impNFeTemp.amarracoes = new List<Amarracao>();
            }

            bool existeFornecedor = await FornecedorExiste(idEmpresa, nfe.NFe.infNFe.emit.CNPJ, nfe.NFe.infNFe.emit.CPF);
            if (!existeFornecedor)
            {
                int CdForn = await CadastrarFornecedor(nfe.NFe.infNFe.emit, idEmpresa);
                foreach (var item in impNFeTemp.impitensnves)
                {
                    Amarracao amarracao = new Amarracao();
                    amarracao.NrItem = item.NrItem;
                    amarracao.CdForn = CdForn;

                    impNFeTemp.amarracoes.Add(amarracao);
                }
            }
            else
            {
                int CdForn = await GetFornecedor(idEmpresa, nfe.NFe.infNFe.emit.CNPJ, nfe.NFe.infNFe.emit.CPF);
                foreach (var item in impNFeTemp.impitensnves)
                {
                    Amarracao amarracao = new Amarracao();
                    amarracao.NrItem = item.NrItem;
                    amarracao.CdForn = CdForn;
                    ProdutosForn? produtosForn = await GetProdutoForn(idEmpresa, CdForn, item.CProd, item.Cean);
                    if (produtosForn != null)
                    {
                        if (produtosForn.ProdutoEstoque != null)
                        {
                            amarracao.CdProduto = produtosForn.CdProduto.ToString();
                            amarracao.NmProduto = produtosForn.ProdutoEstoque.NmProduto;
                            amarracao.FatorConversao = produtosForn.ProdutoEstoque.QtTotal;
                            amarracao.produto = produtosForn.ProdutoEstoque;
                            amarracao.CdBarra = produtosForn.ProdutoEstoque.CdBarra;
                        }
                        else
                        {
                            ProdutoEstoque? produtoEstoque = await db.ProdutoEstoques.FirstOrDefaultAsync(x => x.CdProduto == produtosForn.CdProduto);
                            if (produtoEstoque != null)
                            {
                                amarracao.NmProduto = produtoEstoque.NmProduto;
                                amarracao.CdProduto = produtoEstoque.CdProduto.ToString();
                                amarracao.FatorConversao = produtoEstoque.QtTotal;
                                amarracao.produto = produtoEstoque;
                                amarracao.CdBarra = produtoEstoque.CdBarra;
                            }
                        }
                    }
                    impNFeTemp.amarracoes.Add(amarracao);
                }
            }
        }

        private async Task<ProdutosForn?> GetProdutoForn(int idEmpresa, int cdForn, string? cProd, string? cean)
        {
            ConfiguracoesEmpresa? configuracoesEmpresa = await db.ConfiguracoesEmpresas
               .Where((conf) => conf.Chave.Equals("IMPNFE") && conf.CdEmpresa == idEmpresa)
               .FirstOrDefaultAsync();

            if (configuracoesEmpresa == null)
            {
                return db.ProdutosForns.Where(x => x.CdForn == cdForn && x.IdProdutoExterno == cProd).FirstOrDefault();
            }
            else if (configuracoesEmpresa.Valor1 != null && configuracoesEmpresa.Valor1.Equals("EAN"))
            {
                return db.ProdutosForns.Where(x => x.CdForn == cdForn && x.CdBarra.Equals(cean)).FirstOrDefault();
            }
            else
            {
                return db.ProdutosForns.Where(x => x.CdForn == cdForn && x.IdProdutoExterno == cProd).FirstOrDefault();
            }
        }

        private async Task<int> GetFornecedor(int idEmpresa, string cNPJ, string cPF)
        {
            Fornecedor? fornecedor;
            if (!string.IsNullOrEmpty(cNPJ))
            {
                string cnpjOnlyNumbers = Regex.Replace(cNPJ, @"\D", "");
                fornecedor = await db.Fornecedors
                    .FromSqlRaw("SELECT * FROM public.fornecedor WHERE regexp_replace(cnpj, '\\D', '', 'g') = {0} AND id_empresa = {1}", cnpjOnlyNumbers, idEmpresa)
                    .FirstOrDefaultAsync();
            }
            else
            {
                string cpfOnlyNumbers = Regex.Replace(cPF, @"\D", "");
                fornecedor = await db.Fornecedors
                    .FromSqlRaw("SELECT * FROM public.fornecedor WHERE regexp_replace(cpf, '\\D', '', 'g') = {0} AND id_empresa = {1}", cpfOnlyNumbers, idEmpresa)
                    .FirstOrDefaultAsync();
            }
            if (fornecedor == null)
            {
                return 0;
            }
            return fornecedor.CdForn;
        }



        private async Task<int> CadastrarFornecedor(emit emit, int idEmpresa)
        {
            FornecedorDto novoFornecedor = new FornecedorDto();
            novoFornecedor.IdEmpresa = idEmpresa;
            novoFornecedor.Cnpj = emit.CNPJ;
            novoFornecedor.Cpf = emit.CPF;
            novoFornecedor.NmForn = emit.xNome;
            novoFornecedor.NrInscrEstadual = emit.IE;
            novoFornecedor.CdCep = emit.enderEmit.CEP;
            novoFornecedor.NmEndereco = emit.enderEmit.xLgr;
            var nro = emit.enderEmit.nro;
            string NrEnd;
            if (nro == "s/n" || nro == "S/N" || nro == "SN" || nro == "sn" || nro == ".")
            {
                NrEnd = "0";
            }
            else
            {
                NrEnd = UtlStrings.OnlyInteger(nro);
            }
            if (NrEnd.Length > 0)
            {
                if (NrEnd.Length > 4)
                {
                    novoFornecedor.Numero = Convert.ToInt32(NrEnd.Substring(0, 4));
                }
                else
                {
                    try
                    {
                        novoFornecedor.Numero = Convert.ToInt32(NrEnd);
                    }
                    catch (Exception)
                    {

                        novoFornecedor.Numero = 0;
                    }
                }
            }
            else
            {
                novoFornecedor.Numero = 0;
            }
            novoFornecedor.Complemento = emit.enderEmit.xCpl;
            novoFornecedor.Bairro = emit.enderEmit.xBairro;
            novoFornecedor.CdCidade = emit.enderEmit.cMun.ToString();
            novoFornecedor.TelefoneEmpresa = emit.enderEmit.fone.ToString();

            /*await db.Fornecedors.AddAsync(novoFornecedor);
            await db.SaveChangesAsync();
            return novoFornecedor.CdForn;
            */
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ENDPOINT_POST_FORNECECOR);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_POST_FORNECECOR, novoFornecedor);
            if (response.IsSuccessStatusCode)
            {
                var fornecedor = await response.Content.ReadFromJsonAsync<Fornecedor>();
                if (fornecedor != null)
                {
                    return fornecedor.CdForn;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        private async Task<bool> FornecedorExiste(int idEmpresa, string cnpj, string cpf)
        {
            // Função para remover qualquer caractere não numérico
            string RemoveNonNumeric(string value)
            {
                return Regex.Replace(value, "[^0-9]", "");
            }

            // Remover caracteres não numéricos do CNPJ e CPF fornecidos
            string cnpjNumerico = string.IsNullOrEmpty(cnpj) ? null : RemoveNonNumeric(cnpj);
            string cpfNumerico = string.IsNullOrEmpty(cpf) ? null : RemoveNonNumeric(cpf);

            // Construir a query SQL bruta para comparar CNPJ/CPF no banco de dados
            string sqlQuery = string.Empty;

            if (!string.IsNullOrEmpty(cnpjNumerico))
            {
                sqlQuery = @"
            SELECT *
            FROM fornecedor
            WHERE id_empresa = {0}
            AND REGEXP_REPLACE(cnpj, '[^0-9]', '', 'g') = {1}";

                var count = await db.Fornecedors.FromSqlRaw(sqlQuery, idEmpresa, cnpjNumerico)
                                                 .CountAsync();
                return count > 0;
            }
            else
            {
                sqlQuery = @"
            SELECT *
            FROM fornecedor
            WHERE id_empresa = {0}
            AND REGEXP_REPLACE(cpf, '[^0-9]', '', 'g') = {1}";

                var count = await db.Fornecedors.FromSqlRaw(sqlQuery, idEmpresa, cpfNumerico)
                                                 .CountAsync();
                return count > 0;
            }
        }
        private async Task TentaExcluirAsync(string chNFe)
        {
            try
            {
                var impcabnfe = await db.Impcabnves.FirstOrDefaultAsync(x => x.ChNfe == chNFe);
                if (impcabnfe != null)
                {
                    db.Impcabnves.Remove(impcabnfe);
                }

                var impitensnves = await db.Impitensnves.Where(x => x.ChNfe == chNFe).ToListAsync();
                if (impitensnves.Any())
                {
                    db.Impitensnves.RemoveRange(impitensnves);
                }

                var impdupnves = await db.Impdupnves.Where(x => x.ChNfe == chNFe).ToListAsync();
                if (impdupnves.Any())
                {
                    db.Impdupnves.RemoveRange(impdupnves);
                }

                var imptotalnfe = await db.Imptotalnves.FirstOrDefaultAsync(x => x.ChNfe == chNFe);
                if (imptotalnfe != null)
                {
                    db.Imptotalnves.Remove(imptotalnfe);
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao excluir registros (importação XML NFe) do banco de dados (chNfe={chNFe})");
            }
        }

        private string GetChaveNFe(NFe.Classes.NFe nfe)
        {
            return nfe.infNFe.Id.Substring(3, 44);
        }

        [HttpPost]
        [Route("CadastrarProdutosFaltantes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CadastrarProdutosFaltantes([FromBody] List<Amarracao> amarracoes, int idEmpresa, int cdForn, string chaveNfe)
        {

            try
            {
                if (amarracoes == null || amarracoes.Count == 0)
                {
                    return BadRequest(
                        new ErrorMessage(400, "Nenhum produto foi informado para cadastro"));
                }

                ImpNFeTemp impNFeTemp = new ImpNFeTemp();
                impNFeTemp.amarracoes = new List<Amarracao>();
                impNFeTemp.impitensnves = await db.Impitensnves.Where(x => x.ChNfe == chaveNfe).ToListAsync();
                impNFeTemp.imptotalnfe = await db.Imptotalnves.FirstOrDefaultAsync(x => x.ChNfe == chaveNfe);
                impNFeTemp.impdupnfe = await db.Impdupnves.Where(x => x.ChNfe == chaveNfe).ToListAsync();
                impNFeTemp.impcabnfe = await db.Impcabnves.FirstOrDefaultAsync(x => x.ChNfe == chaveNfe);

                if (impNFeTemp.impitensnves == null || impNFeTemp.impitensnves.Count == 0)
                {
                    return BadRequest(new ErrorMessage(500,
                        "Itens da nota fiscal não encontrados"));
                }
                if (impNFeTemp.imptotalnfe == null)
                {
                    return BadRequest(
                        new ErrorMessage(500, "Total da nota fiscal não encontrado"));
                }
                if (impNFeTemp.impdupnfe == null || impNFeTemp.impdupnfe.Count == 0)
                {
                    return BadRequest(
                        new ErrorMessage(500, "Duplicatas da nota fiscal não encontradas"));
                }
                if (impNFeTemp.impcabnfe == null)
                {
                    return BadRequest(
                        new ErrorMessage(500, "Cabeçalho da nota fiscal não encontrado"));
                }

                foreach (var amarracao in amarracoes)
                {
                    if (amarracao.produto == null)
                    {
                        Impitensnfe? impitensnfe = impNFeTemp.impitensnves.FirstOrDefault(x => x.NrItem == amarracao.NrItem);
                        if (impitensnfe == null)
                        {
                            return BadRequest(
                                new ErrorMessage(500, "Item da nota fiscal não encontrado"));
                        }
                        ProdutosForn? produtosForn_pes = await GetProdutoForn(idEmpresa, cdForn, impitensnfe.CProd, impitensnfe.Cean);
                        if (produtosForn_pes != null)
                        {
                            ProdutoEstoque? produtoEstoque1 = await db.ProdutoEstoques.FirstOrDefaultAsync(x => x.CdProduto == produtosForn_pes.CdProduto);
                            if (produtoEstoque1 != null)
                            {
                                amarracao.NrItem = impitensnfe.NrItem;
                                amarracao.produto = produtoEstoque1;
                                amarracao.NmProduto = produtoEstoque1.NmProduto;
                                amarracao.CdProduto = produtoEstoque1.CdProduto.ToString();
                                amarracao.FatorConversao = produtoEstoque1.QtTotal;
                                amarracao.CdBarra = produtoEstoque1.CdBarra;
                                impNFeTemp.amarracoes.Add(amarracao);
                                await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"Produto '{produtoEstoque1.NmProduto}' amarrado com sucesso...");
                                continue;
                            }
                            else
                            {
                                //delete o registro do produtosForn
                                //db.ProdutosForns.Remove(produtosForn_pes);
                                //await db.SaveChangesAsync();
                                string urlDelete = ENDPOINT_PRODUTOS_FORN + "/" + produtosForn_pes.Id;
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri(urlDelete);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                HttpResponseMessage response = await client.DeleteAsync(urlDelete);
                                if (response.IsSuccessStatusCode)
                                {
                                    var _response = await response.Content.ReadAsStringAsync();
                                    if (_response != null)
                                    {
                                        logger.LogInformation($"ProdutosForn id [{produtosForn_pes.Id}] DELETADO com sucesso.");

                                    }
                                    else
                                    {
                                        logger.LogError($"Erro ao importar DELETAR produtosForn ({produtosForn_pes.Id}).");
                                        throw new Exception("Erro ao DELETAR produtosForn");
                                    }
                                }
                                else
                                {
                                    logger.LogError($"Erro ao importar DELETAR produtosForn ({produtosForn_pes.Id}).");
                                    throw new Exception("Erro ao importar DELETAR produtosForn");
                                }
                            }
                        }
                        ProdutoEstoqueDto produtoEstoque = new ProdutoEstoqueDto();
                        produtoEstoque.Ativo = "S";
                        produtoEstoque.LancLivro = "S";
                        produtoEstoque.TpItem = "00";
                        produtoEstoque.IdEmpresa = idEmpresa;
                        produtoEstoque.QtUnitario = 1;
                        produtoEstoque.NmProduto = impitensnfe.Nome;
                        produtoEstoque.CdTribt = 3;
                        produtoEstoque.QuantMinima = 1;
                        produtoEstoque.CdClassFiscal = UtlStrings.OnlyInteger(impitensnfe.Ncm);
                        if (impitensnfe.Cest != null && impitensnfe.Cest.Length > 0)
                        {
                            produtoEstoque.Cest = impitensnfe.Cest;
                        }
                        if (impitensnfe.Cean != null && impitensnfe.Cean.Length > 10)
                        {
                            produtoEstoque.CdBarra = impitensnfe.Cean;
                        }
                        else
                        {
                            produtoEstoque.CdBarra = "SEM GTIN";
                        }

                        GrupoEstoque? grupo = await db.GrupoEstoques.FirstOrDefaultAsync(x => x.CdEmpresa == idEmpresa
                        && x.NmGrupo.ToUpper().Trim().Equals("DIVERSOS")
                        );
                        if (grupo != null)
                        {
                            produtoEstoque.CdGrupo = grupo.CdGrupo;
                        }
                        else
                        {
                            GrupoEstoqueDto grupoEstoqueDto = new GrupoEstoqueDto();
                            grupoEstoqueDto.CdEmpresa = idEmpresa;
                            grupoEstoqueDto.NmGrupo = "DIVERSOS";
                            const string END_POINT_GRUPO = Constants.URL_API_NFE + "/api/GrupoEstoque";
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri(END_POINT_GRUPO);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage response = await client.PostAsJsonAsync(END_POINT_GRUPO, grupoEstoqueDto);
                            if (response.IsSuccessStatusCode)
                            {
                                var _grupoEstoque = await response.Content.ReadFromJsonAsync<GrupoEstoque>();
                                if (_grupoEstoque != null)
                                {
                                    produtoEstoque.CdGrupo = _grupoEstoque.CdGrupo;
                                }
                                else
                                {
                                    return StatusCode(500,
                                        new ErrorMessage(500, "Erro ao cadastrar grupo"));
                                }
                            }
                            else
                            {
                                return StatusCode(500,

                                    new ErrorMessage(500, response.Content.ReadAsStringAsync().Result));
                            }
                        }
                        ReferenciaEstoque? referenciaEstoque = await db.ReferenciaEstoques.FirstOrDefaultAsync(x => x.CdEmpresa == idEmpresa && x.NmRef.ToUpper().Trim().Equals("DIVERSOS"));
                        if (referenciaEstoque != null)
                        {
                            produtoEstoque.CdRef = referenciaEstoque.CdRef;
                        }
                        else
                        {
                            ReferenciaEstoqueDto referenciaEstoqueDto = new ReferenciaEstoqueDto();
                            referenciaEstoqueDto.CdEmpresa = idEmpresa;
                            referenciaEstoqueDto.NmRef = "DIVERSOS";
                            const string END_POINT_REFERENCIA = Constants.URL_API_NFE + "/api/ReferenciaEstoque";
                            HttpClient clientReferencia = new HttpClient();
                            clientReferencia.BaseAddress = new Uri(END_POINT_REFERENCIA);
                            clientReferencia.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage responseReferencia = await clientReferencia.PostAsJsonAsync(END_POINT_REFERENCIA, referenciaEstoqueDto);
                            if (responseReferencia.IsSuccessStatusCode)
                            {
                                var _referenciaEstoque = await responseReferencia.Content.ReadFromJsonAsync<ReferenciaEstoque>();
                                if (_referenciaEstoque != null)
                                {
                                    produtoEstoque.CdRef = _referenciaEstoque.CdRef;
                                }
                                else
                                {
                                    return StatusCode(500, new ErrorMessage(500, "Erro ao cadastrar referencia"));
                                }
                            }
                            else
                            {
                                return StatusCode(500,
                                        new ErrorMessage(500, responseReferencia.Content.ReadAsStringAsync().Result));
                            }
                        }

                        UnidadeMedida? unidadeMedida = await db.UnidadeMedidas.FirstOrDefaultAsync(x => x.IdEmpresa == idEmpresa && x.CdUnidade.Equals(impitensnfe.Un));
                        if (unidadeMedida != null)
                        {
                            produtoEstoque.CdUni = unidadeMedida.CdUnidade;
                        }
                        else
                        {
                            UnidadeMedidaDto unidadeMedidaDto = new UnidadeMedidaDto();
                            unidadeMedidaDto.IdEmpresa = idEmpresa;
                            unidadeMedidaDto.CdUnidade = impitensnfe.Un;
                            unidadeMedidaDto.Descricao = impitensnfe.Un;
                            const string END_POINT_UNIDADE = Constants.URL_API_NFE + "/api/UnidadeMedida";
                            HttpClient clientUnidade = new HttpClient();
                            clientUnidade.BaseAddress = new Uri(END_POINT_UNIDADE);
                            clientUnidade.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage responseUnidade = await clientUnidade.PostAsJsonAsync(END_POINT_UNIDADE, unidadeMedidaDto);
                            if (responseUnidade.IsSuccessStatusCode)
                            {
                                var _unidadeMedida = await responseUnidade.Content.ReadFromJsonAsync<UnidadeMedida>();
                                if (_unidadeMedida != null)
                                {
                                    produtoEstoque.CdUni = _unidadeMedida.CdUnidade;
                                }
                                else
                                {
                                    return StatusCode(500, new ErrorMessage(500, "Erro ao cadastrar unidade de medida"));
                                }
                            }
                            else
                            {
                                return StatusCode(500,
                                new ErrorMessage(500, responseUnidade.Content.ReadAsStringAsync().Result));
                            }
                        }

                        const string END_POINT_PRODUTO = Constants.URL_API_NFE + "/api/ProdutoEstoque";
                        HttpClient clientProduto = new HttpClient();
                        clientProduto.BaseAddress = new Uri(END_POINT_PRODUTO);
                        clientProduto.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage responseProduto = await clientProduto.PostAsJsonAsync(END_POINT_PRODUTO, produtoEstoque);
                        if (responseProduto.IsSuccessStatusCode)
                        {
                            var _produtoEstoque = await responseProduto.Content.ReadFromJsonAsync<ProdutoEstoque>();
                            if (_produtoEstoque != null)
                            {
                                if (_produtoEstoque.CdProduto <= 0)
                                {
                                    return StatusCode(500,
                                        new ErrorMessage(500, "Erro ao cadastrar produto"));
                                }

                                await _hubContext.Clients.All.SendAsync("ReceiveProgress", $"Produto '{_produtoEstoque.NmProduto}' cadastrado com sucesso...");

                                ProdutosForn produtosForn = new ProdutosForn();
                                produtosForn.CdProduto = _produtoEstoque.CdProduto;
                                produtosForn.CdForn = cdForn;
                                produtosForn.IdProdutoExterno = impitensnfe.CProd;
                                produtosForn.CdBarra = impitensnfe.Cean;
                                produtosForn.IdEmpresa = idEmpresa;

                                ProdutosFornDto produtosFornDto = mapper.Map<ProdutosFornDto>(produtosForn);

                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri(ENDPOINT_PRODUTOS_FORN);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_PRODUTOS_FORN, produtosFornDto);
                                if (response.IsSuccessStatusCode)
                                {
                                    var _response = await response.Content.ReadFromJsonAsync<ProdutosForn>();
                                    if (_response != null)
                                    {
                                        produtosForn = _response;
                                        logger.LogInformation($"ProdutosForn id [{idEmpresa},{_response.Id}] criado com sucesso.");

                                    }
                                    else
                                    {
                                        logger.LogError($"Erro ao importar ProdutosForn ({impNFeTemp?.impcabnfe?.ChNfe}).");
                                        throw new Exception("Erro ao importar ProdutosForn");
                                    }
                                }
                                else
                                {
                                    logger.LogError($"Erro ao importar ProdutosForn ({impNFeTemp?.impcabnfe?.ChNfe}).");
                                    throw new Exception("Erro ao importar ProdutosForn");
                                }

                                amarracao.NrItem = impitensnfe.NrItem;
                                amarracao.produto = _produtoEstoque;
                                amarracao.NmProduto = _produtoEstoque.NmProduto;
                                amarracao.CdProduto = _produtoEstoque.CdProduto.ToString();
                                amarracao.FatorConversao = _produtoEstoque.QtTotal;
                                amarracao.CdBarra = _produtoEstoque.CdBarra;
                                impNFeTemp.amarracoes.Add(amarracao);
                            }
                            else
                            {
                                return StatusCode(500,
                                    new ErrorMessage(500, "Erro ao cadastrar produto"));
                            }
                        }
                        else
                        {
                            return StatusCode(500,

                                new ErrorMessage(500, responseProduto.Content.ReadAsStringAsync().Result));
                        }
                    }
                    else
                    {
                        impNFeTemp.amarracoes.Add(amarracao);
                    }
                }
                if (impNFeTemp.amarracoes.Count == 0)
                {
                    return BadRequest(
                        new ErrorMessage(400, "Nenhum produto foi cadastrado"));
                }
                else if (impNFeTemp.amarracoes.Count != amarracoes.Count)
                {
                    return StatusCode(500,
                        new ErrorMessage(500, "Erro ao cadastrar produtos faltantes NFe"));
                }
                else if (impNFeTemp.amarracoes.Count != impNFeTemp.impitensnves.Count)
                {
                    return StatusCode(500,
                        new ErrorMessage(500, "Erro ao cadastrar produtos faltantes NFe"));
                }


                return Ok(impNFeTemp);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao cadastrar produtos faltantes NFe");
                return StatusCode(500,
                    new ErrorMessage(500, ex.Message));
            }
        }

        [HttpPost]
        [Route("CadastrarProduto")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CadastrarProduto(int idEmpresa, int cdForn, string chaveNfe, string nrItem)
        {
            try
            {
                Impitensnfe? impitensnfe = await db.Impitensnves.FirstOrDefaultAsync(x => x.ChNfe == chaveNfe && x.NrItem == nrItem);
                if (impitensnfe == null)
                {
                    return NotFound(
                        new ErrorMessage(404, "Item da nota fiscal não encontrado"));

                }
                ProdutosForn? produtoFornOld = await db.ProdutosForns.FirstOrDefaultAsync(x => x.IdEmpresa == idEmpresa && x.CdForn == cdForn && x.IdProdutoExterno == impitensnfe.CProd && x.CdBarra.Equals(impitensnfe.Cean));
                if (produtoFornOld != null)
                {
                    ProdutoEstoque? produtoEstoqueOld = await db.ProdutoEstoques.FirstOrDefaultAsync(x => x.IdEmpresa == idEmpresa && x.CdProduto == produtoFornOld.CdProduto && x.CdBarra.Equals(produtoFornOld.CdBarra));
                    if (produtoEstoqueOld != null)
                    {
                        return Ok(produtoEstoqueOld);
                    }
                }

                ProdutoEstoqueDto produtoEstoque = new ProdutoEstoqueDto();
                produtoEstoque.Ativo = "S";
                produtoEstoque.LancLivro = "S";
                produtoEstoque.TpItem = "00";
                produtoEstoque.IdEmpresa = idEmpresa;
                produtoEstoque.QtUnitario = 1;
                produtoEstoque.NmProduto = impitensnfe.Nome;
                produtoEstoque.CdTribt = 3;
                produtoEstoque.QuantMinima = 1;
                produtoEstoque.CdClassFiscal = UtlStrings.OnlyInteger(impitensnfe.Ncm);
                if (impitensnfe.Cest != null && impitensnfe.Cest.Length > 0)
                {
                    produtoEstoque.Cest = impitensnfe.Cest;
                }
                if (impitensnfe.Cean != null && impitensnfe.Cean.Length > 10)
                {
                    produtoEstoque.CdBarra = impitensnfe.Cean;
                }
                else
                {
                    produtoEstoque.CdBarra = "SEM GTIN";
                }

                GrupoEstoque? grupo = await db.GrupoEstoques.FirstOrDefaultAsync(x => x.CdEmpresa == idEmpresa
                && x.NmGrupo.ToUpper().Trim().Equals("DIVERSOS")
                );
                if (grupo != null)
                {
                    produtoEstoque.CdGrupo = grupo.CdGrupo;
                }
                else
                {
                    GrupoEstoqueDto grupoEstoqueDto = new GrupoEstoqueDto();
                    grupoEstoqueDto.CdEmpresa = idEmpresa;
                    grupoEstoqueDto.NmGrupo = "DIVERSOS";
                    const string END_POINT_GRUPO = Constants.URL_API_NFE + "/api/GrupoEstoque";
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(END_POINT_GRUPO);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync(END_POINT_GRUPO, grupoEstoqueDto);
                    if (response.IsSuccessStatusCode)
                    {
                        var _grupoEstoque = await response.Content.ReadFromJsonAsync<GrupoEstoque>();
                        if (_grupoEstoque != null)
                        {
                            produtoEstoque.CdGrupo = _grupoEstoque.CdGrupo;
                        }
                        else
                        {
                            return StatusCode(500,
                                new ErrorMessage(500, "Erro ao cadastrar grupo"));
                        }
                    }
                    else
                    {
                        return StatusCode(500,

                            new ErrorMessage(500, response.Content.ReadAsStringAsync().Result));
                    }
                }

                ReferenciaEstoque? referenciaEstoque = await db.ReferenciaEstoques.FirstOrDefaultAsync(x => x.CdEmpresa == idEmpresa && x.NmRef.ToUpper().Trim().Equals("DIVERSOS"));
                if (referenciaEstoque != null)
                {
                    produtoEstoque.CdRef = referenciaEstoque.CdRef;
                }
                else
                {
                    ReferenciaEstoqueDto referenciaEstoqueDto = new ReferenciaEstoqueDto();
                    referenciaEstoqueDto.CdEmpresa = idEmpresa;
                    referenciaEstoqueDto.NmRef = "DIVERSOS";
                    const string END_POINT_REFERENCIA = Constants.URL_API_NFE + "/api/ReferenciaEstoque";
                    HttpClient clientReferencia = new HttpClient();
                    clientReferencia.BaseAddress = new Uri(END_POINT_REFERENCIA);
                    clientReferencia.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseReferencia = await clientReferencia.PostAsJsonAsync(END_POINT_REFERENCIA, referenciaEstoqueDto);
                    if (responseReferencia.IsSuccessStatusCode)
                    {
                        var _referenciaEstoque = await responseReferencia.Content.ReadFromJsonAsync<ReferenciaEstoque>();
                        if (_referenciaEstoque != null)
                        {
                            produtoEstoque.CdRef = _referenciaEstoque.CdRef;
                        }
                        else
                        {
                            return StatusCode(500,
                                new ErrorMessage(500, "Erro ao cadastrar referencia"));
                        }
                    }
                    else
                    {
                        return StatusCode(500,

                            new ErrorMessage(500, responseReferencia.Content.ReadAsStringAsync().Result));
                    }
                }

                UnidadeMedida? unidadeMedida = await db.UnidadeMedidas.FirstOrDefaultAsync(x => x.IdEmpresa == idEmpresa && x.CdUnidade.Equals(impitensnfe.Un));
                if (unidadeMedida != null)
                {
                    produtoEstoque.CdUni = unidadeMedida.CdUnidade;
                }
                else
                {
                    UnidadeMedidaDto unidadeMedidaDto = new UnidadeMedidaDto();
                    unidadeMedidaDto.IdEmpresa = idEmpresa;
                    unidadeMedidaDto.CdUnidade = impitensnfe.Un;
                    unidadeMedidaDto.Descricao = impitensnfe.Un;
                    const string END_POINT_UNIDADE = Constants.URL_API_NFE + "/api/UnidadeMedida";
                    HttpClient clientUnidade = new HttpClient();
                    clientUnidade.BaseAddress = new Uri(END_POINT_UNIDADE);
                    clientUnidade.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseUnidade = await clientUnidade.PostAsJsonAsync(END_POINT_UNIDADE, unidadeMedidaDto);
                    if (responseUnidade.IsSuccessStatusCode)
                    {
                        var _unidadeMedida = await responseUnidade.Content.ReadFromJsonAsync<UnidadeMedida>();
                        if (_unidadeMedida != null)
                        {
                            produtoEstoque.CdUni = _unidadeMedida.CdUnidade;
                        }
                        else
                        {
                            return StatusCode(500,
                                new ErrorMessage(500, "Erro ao cadastrar unidade de medida"));
                        }
                    }
                    else
                    {
                        return StatusCode(500,

                            new ErrorMessage(500, responseUnidade.Content.ReadAsStringAsync().Result));
                    }
                }

                const string END_POINT_PRODUTO = Constants.URL_API_NFE + "/api/ProdutoEstoque";
                HttpClient clientProduto = new HttpClient();
                clientProduto.BaseAddress = new Uri(END_POINT_PRODUTO);
                clientProduto.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseProduto = await clientProduto.PostAsJsonAsync(END_POINT_PRODUTO, produtoEstoque);
                if (responseProduto.IsSuccessStatusCode)
                {
                    var _produtoEstoque = await responseProduto.Content.ReadFromJsonAsync<ProdutoEstoque>();
                    if (_produtoEstoque != null)
                    {
                        if (_produtoEstoque.CdProduto <= 0)
                        {
                            return StatusCode(500,
                                new ErrorMessage(500, "Erro ao cadastrar produto"));
                        }

                        ProdutosForn produtosForn = new ProdutosForn();
                        produtosForn.CdProduto = _produtoEstoque.CdProduto;
                        produtosForn.CdForn = cdForn;
                        produtosForn.IdProdutoExterno = impitensnfe.CProd;
                        produtosForn.CdBarra = impitensnfe.Cean;
                        produtosForn.IdEmpresa = idEmpresa;

                        ProdutosFornDto produtosFornDto = mapper.Map<ProdutosFornDto>(produtosForn);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(ENDPOINT_PRODUTOS_FORN);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_PRODUTOS_FORN, produtosFornDto);
                        if (response.IsSuccessStatusCode)
                        {
                            var _response = await response.Content.ReadFromJsonAsync<ProdutosForn>();
                            if (_response != null)
                            {
                                produtosForn = _response;
                                logger.LogInformation($"ProdutosForn id [{idEmpresa},{_response.Id}] criado com sucesso.");

                            }
                            else
                            {
                                logger.LogError($"Erro ao importar ProdutosForn.");
                                throw new Exception("Erro ao importar ProdutosForn");
                            }
                        }
                        else
                        {
                            logger.LogError($"Erro ao importar ProdutosForn.");
                            throw new Exception("Erro ao importar ProdutosForn");
                        }

                        return Ok(_produtoEstoque);
                    }
                    else
                    {
                        return StatusCode(500,
                            new ErrorMessage(500, "Erro ao cadastrar produto"));
                    }
                }
                else
                {
                    return StatusCode(500,
                        new ErrorMessage(500, responseProduto.Content.ReadAsStringAsync().Result));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao cadastrar produto NFe");
                return StatusCode(500,
                    new ErrorMessage(500, ex.Message));
            }
        }
    }
}
