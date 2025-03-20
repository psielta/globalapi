using AutoMapper;
using GlobalAPI_ACBrNFe.Lib;
using GlobalAPI_ACBrNFe.Models;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NFe.Classes.Informacoes;
using System.Net.Http.Headers;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegNfeController : Controller
    {
        protected GlobalErpFiscalBaseContext db;
        protected readonly ILogger<ImpNfeController> logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        private string ENDPOINT_POST_FORNECECOR;
        private string ENDPOINT_ENTRADA;
        private string ENDPOINT_PRODUTO_ENTRADA;
        private string ENDPOINT_CONTAS_PAGAR;
        private string ENDPOINT_PRODUTOS_FORN;
        private string ENDPOINT_PRODUTOS;
        private string ENDPOINT_UNIDADE_MEDIDA;
        private string ENDPOINT_TRANSPORTADORA;
        public RegNfeController(GlobalErpFiscalBaseContext db, ILogger<ImpNfeController> logger
            , IMapper mapper,
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
        }

        [HttpPost("Registrar/{idEmpresa}/{chaveAcesso}/{cdPlanoEstoque}/{cdHistorico}", Name = nameof(Registrar))]
        [ProducesResponseType(typeof(Entrada), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Entrada>> Registrar([FromBody] ImpNFeTemp2 impNFeTemp, int idEmpresa, int cdPlanoEstoque, int cdHistorico, string chaveAcesso, string sessionId, string tipoEntrada)
        {
           var empresa = await db.Empresas.AsNoTracking().Where(c => c.CdEmpresa == idEmpresa).FirstOrDefaultAsync();

            #region Validações
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Iniciando validações...");
            if (idEmpresa == 0 || string.IsNullOrEmpty(chaveAcesso) || chaveAcesso.Length < 44)
            {
                logger.LogError($"Erro ao buscar NFe ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Is missing parameters"));
            }

            if (impNFeTemp == null)
            {
                logger.LogError($"Erro ao buscar NFe ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Is missing body"));
            }
            if (impNFeTemp.amarracoes == null)
            {
                logger.LogError($"Erro ao buscar amarrações ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Is missing NFe"));
            }
            foreach (var item in impNFeTemp.amarracoes)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.CdProduto) || Convert.ToInt32(item.CdProduto) <= 0)
                    {
                        logger.LogError($"Erro ao buscar amarrações ({chaveAcesso}).");
                        return BadRequest(new ErrorMessage(500, "Is missing amarracao"));
                    }
                }
                catch
                {
                    logger.LogError($"Erro ao buscar amarrações ({chaveAcesso}).");
                    return BadRequest(new ErrorMessage(500, "Is missing amarracao"));
                }
            }
            #endregion
            #region Busca Fornecedor
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando fornecedor...");
            int IdFornecedor = 0;
            Fornecedor? fornecedor = null;

            try
            {
                IdFornecedor = impNFeTemp.amarracoes.FirstOrDefault().CdForn;
                fornecedor = await db.Fornecedors.FindAsync(IdFornecedor, idEmpresa);
                if (IdFornecedor <= 0 || fornecedor == null)
                {
                    return BadRequest(new ErrorMessage(500, "Is missing Fornecedor"));
                }
            }
            catch
            {
                logger.LogError($"Erro ao buscar fornecedor ({IdFornecedor}, {chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Is missing Fornecedor"));
            }
            #endregion
            #region Busca Transportadora
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando Transportadora...");
            Transportadora? transportadora;
            try
            {
                transportadora = await ImportarTransportadora(impNFeTemp, idEmpresa, empresa.Unity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro importação de transportadora ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing Transp"));
            }
            #endregion
            #region Entrada
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando entrada.");


            Entrada? entrada;
            try
            {
                entrada = await ImportarEntrada(IdFornecedor, transportadora, impNFeTemp, idEmpresa, cdPlanoEstoque, tipoEntrada);
                if (entrada == null)
                {
                    logger.LogError($"Erro ao importar entrada ({chaveAcesso}).");
                    return BadRequest(new ErrorMessage(500, "Error importing Entrada"));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar entrada ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing Entrada"));
            }
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Entrada importada com sucesso.");

            #endregion
            #region Importa XML
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando conteudo xml.");
            try
            {
                await ImportarXml(impNFeTemp, entrada, idEmpresa);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar XML ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing XML"));
            }
            #endregion
            #region Itens
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando itens do XML.");
            try
            {
                await ImportarItens(impNFeTemp, entrada, idEmpresa, cdPlanoEstoque, sessionId, empresa.Unity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar itens ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing items"));
            }
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Itens do XML importados com sucesso.");
            #endregion
            #region Duplicata
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Importando duplicatas do XML.");
            try
            {
                await ImportarDuplicatas(impNFeTemp, entrada, idEmpresa, cdHistorico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar duplicatas ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing Duplicatas"));
            }
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Finalizado com sucesso.");
            #endregion

            return Ok(entrada);
        }

        private async Task ImportarXml(ImpNFeTemp2 impNFeTemp, Entrada entrada, int idEmpresa)
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
                              AND chave_acesso = '{impNFeTemp.impcabnfe.ChNfe}'
                              AND type = 0
                              ";
            Impxml? impxml = await db.Impxmls.FromSqlRaw(SQL).FirstOrDefaultAsync();
            if (impxml == null)
            {
                throw new Exception($"Erro ao buscar XML ({impNFeTemp.impcabnfe.ChNfe})");
            }
            entrada.XmlNf = impxml.Xml;
            string url = ENDPOINT_ENTRADA + $"/{idEmpresa}/{entrada.Nr}";

            EntradaDto entradaDto = mapper.Map<EntradaDto>(entrada);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PutAsJsonAsync(url, entradaDto);
            if (response.IsSuccessStatusCode)
            {
                var _response = await response.Content.ReadFromJsonAsync<Entrada>();
                if (_response != null)
                {
                    logger.LogInformation($"Entrada id [{idEmpresa},{_response.Nr}] criado com sucesso.");
                    entrada = _response;
                }
                else
                {
                    logger.LogError($"Erro ao importar XML entrada ({entrada.Nr}).");
                    throw new Exception("Erro ao importar XML entrada");
                }
            }
            else
            {
                logger.LogError($"Erro ao importar XML entrada ({entrada.Nr}).");
                throw new Exception("Erro ao importar XML entrada");
            }
        }

        private async Task ImportarDuplicatas(ImpNFeTemp2 impNFeTemp, Entrada entrada, int idEmpresa, int cdHistorico)
        {
            if (impNFeTemp.impdupnfe == null || impNFeTemp.impdupnfe.Count == 0)
            {
                return;
            }
            var count = impNFeTemp.impdupnfe.Count;
            HistoricoCaixa historicoCaixa = await db.HistoricoCaixas.FindAsync(cdHistorico);
            if (historicoCaixa == null)
            {
                throw new Exception($"Erro ao buscar histórico ({cdHistorico})");
            }

            List<ContasAPagar> contasAPagars = await db.ContasAPagars.Where((x) => x.NrEntrada == entrada.Nr &&
            x.CdEmpresa == idEmpresa).ToListAsync();
            if (contasAPagars != null && contasAPagars.Count > 0)
            {
                return;
            }

            foreach (var item in impNFeTemp.impdupnfe)
            {
                DateOnly dataVencimento = DateUtils.DateTimeToDateOnly(item.DtVenc ?? DateTime.Now);
                ContasAPagarDto contasAPagarDto = new ContasAPagarDto();
                contasAPagarDto.DtLancamento = DateUtils.DateTimeToDateOnly(DateTime.Now);
                contasAPagarDto.DtVencimento = dataVencimento;
                contasAPagarDto.CdFornecedor = entrada.CdForn;
                contasAPagarDto.NrDuplicata = $"ENT{entrada.Nr}";
                contasAPagarDto.VlCp = ConvertToDecimal(item.Valor);
                contasAPagarDto.VlDesconto = ConvertToDecimal(0);
                contasAPagarDto.VlTotal = ConvertToDecimal(item.Valor);
                contasAPagarDto.Pagou = "N";
                contasAPagarDto.CdEmpresa = idEmpresa;
                contasAPagarDto.NrNf = entrada.Nr.ToString();
                contasAPagarDto.CdPlanoCaixa = historicoCaixa.CdPlano;
                contasAPagarDto.CdHistoricoCaixa = historicoCaixa.CdSubPlano;
                contasAPagarDto.NrEntrada = entrada.Nr;
                contasAPagarDto.TpFormaPagt = entrada.TPag;
                contasAPagarDto.Rate = 0;
                contasAPagarDto.NumberOfPayments = count;
                contasAPagarDto.TypeRegister = (count > 0) ? 1 : 0;
                contasAPagarDto.Type = 1;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ENDPOINT_CONTAS_PAGAR);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_CONTAS_PAGAR, contasAPagarDto);
                if (response.IsSuccessStatusCode)
                {
                    var _response = await response.Content.ReadFromJsonAsync<ContasAPagar>();
                    if (_response != null)
                    {
                        logger.LogInformation($"Contas a pagar id [{idEmpresa},{_response.Id}] criado com sucesso.");
                    }
                    else
                    {
                        logger.LogError($"Erro ao importar Contas a Pagar ({entrada.CdChaveNfe}).");
                        throw new Exception("Erro ao importar Contas a Pagar");
                    }
                }
                else
                {
                    logger.LogError($"Erro ao importar Contas a Pagar ({entrada.CdChaveNfe}).");
                    throw new Exception("Erro ao importar Contas a Pagar");
                }

            }

        }

        private async Task ImportarItens(ImpNFeTemp2 impNFeTemp, Entrada entrada, int idEmpresa, int cdPlanoEstoque, string sessionId, int unity)
        {
            int cont = impNFeTemp.impitensnves.Count;
            foreach (var item in impNFeTemp.impitensnves)
            {
                Amarracao2? amarracao = impNFeTemp.amarracoes.FirstOrDefault(x => x.NrItem == item.NrItem);
                if (amarracao == null)
                {
                    throw new Exception($"Erro ao buscar amarração ({item.NrItem})");
                }
                string SQLitem = $@"
                    SELECT 
                      *
                    FROM 
                      public.produto_entrada
                    WHERE 
                      cd_empresa = {idEmpresa} AND
                      nr_entrada = '{entrada.Nr}' AND
                      cd_produto = {amarracao.CdProduto} AND
                      nr_item = {item.NrItem}";

                ProdutoEntradum? ppp = await db.ProdutoEntrada.FromSqlRaw(SQLitem).FirstOrDefaultAsync();
                if (ppp == null)
                {
                    ppp = new ProdutoEntradum();
                    ppp.CdEmpresa = idEmpresa;
                    ppp.NrEntrada = entrada.Nr;
                    ppp.NrItem = Convert.ToInt16(item.NrItem);
                    ppp.CdBarra = (string.IsNullOrEmpty(amarracao.CdBarra) ? "SEM GTIN" : amarracao.CdBarra);
                    ppp.CdProduto = Convert.ToInt32(amarracao.CdProduto);
                    ProdutoEstoque? produtoEstoque = await db.ProdutoEstoques.FromSqlRaw($"SELECT * from produto_estoque where cd_produto = {amarracao.CdProduto} and id_empresa = {idEmpresa}").FirstOrDefaultAsync();
                    if (produtoEstoque == null)
                    {
                        throw new Exception($"Erro ao buscar produto ({amarracao.CdProduto})");
                    }
                    ppp.CdPlano = cdPlanoEstoque;
                    ppp.CdClassFiscal = item.Ncm;
                    ppp.Lote = item.Lote ?? "-1";
                    ppp.DtValidade = (item.DtValid != null) ? (DateUtils.DateTimeToDateOnly(item.DtValid ?? DateTime.Now)) : (
                        new DateOnly(9999, 01, 01));
                    ppp.Quant = Math.Round(ConvertToDecimal(item.Qtrib) * (amarracao.FatorConversao ?? 1), 4);
                    //ConvertToDecimal(item.Qtrib);
                    ppp.VlUnitario = Math.Round(ConvertToDecimal(item.Vuntrib) / (amarracao.FatorConversao ?? 1), 4);
                    // ConvertToDecimal(item.Vuntrib);
                    await ImportarUnidadeMedida(item, amarracao, idEmpresa, unity);
                    ppp.Unidade = produtoEstoque.CdUni;//item.Utrib;
                    await AtualizarDadosFiscais(item, ppp, amarracao, entrada, idEmpresa, produtoEstoque);
                    if ((!string.IsNullOrEmpty(item.Csosn)) && item.Csosn.Length > 0)
                    {
                        if (item.Csosn.Length >= 3)
                        {
                            ppp.Cst = item.ImpOrigem + item.Csosn;
                        }
                        else
                        {
                            ppp.Cst = item.ImpOrigem + item.Cst;
                        }
                    }
                    else
                    {
                        ppp.Cst = item.ImpOrigem + item.Cst;
                    }

                    ppp.BIcms = ConvertToDecimal(ConvertToDecimal(item.Vbc));
                    ppp.PorcIcms = ConvertToDecimal(item.Picms);
                    ppp.VlIcms = ConvertToDecimal(item.Vicms);
                    ppp.VlIcmsSt = ConvertToDecimal(item.Vicmsst);
                    ppp.CstConfins = item.ImpOrigem + item.CofCst;
                    ppp.CstPis = item.ImpOrigem + item.PisCst;
                    ppp.FreteProduto = ConvertToDecimal(item.FreteProduto);
                    ppp.VlSeguro = ConvertToDecimal(item.Seguro);

                    if (!string.IsNullOrEmpty(item.Ipivbc) && item.Ipivbc.Length > 0)
                    {
                        ppp.BaseIpi = ConvertToDecimal(item.Ipivbc);
                    }
                    else
                    {
                        ppp.BaseIpi = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Ipipipi) && item.Ipipipi.Length > 0)
                    {
                        ppp.PorcIpi = ConvertToDecimal(item.Ipipipi);
                    }
                    else
                    {
                        ppp.PorcIpi = 0;
                    }
                    if (cont == 1)
                    {
                        if (!string.IsNullOrEmpty(item.Ipivipi) && item.Ipivipi.Length > 0)
                        {
                            ppp.VlIpi = ConvertToDecimal(item.Ipivipi);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(impNFeTemp.imptotalnfe.IcmsVipi) && impNFeTemp.imptotalnfe.IcmsVipi.Length > 0)
                            {
                                ppp.VlIpi = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVipi);
                            }
                            else
                            {
                                ppp.VlIpi = 0;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item.Ipivipi) && item.Ipivipi.Length > 0)
                        {
                            ppp.VlIpi = ConvertToDecimal(item.Ipivipi);
                        }
                        else
                        {
                            ppp.VlIpi = 0;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Vldesc) && item.Vldesc.Length > 0)
                    {
                        ppp.VlOutras = ConvertToDecimal(item.Vldesc);
                    }
                    else
                    {
                        ppp.VlOutras = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Pisvpis) && item.Pisvpis.Length > 0)
                    {
                        ppp.VlPis = ConvertToDecimal(item.Pisvpis);
                    }
                    else
                    {
                        ppp.VlPis = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofVcofins) && item.CofVcofins.Length > 0)
                    {
                        ppp.VlConfins = ConvertToDecimal(item.CofVcofins);
                    }
                    else
                    {
                        ppp.VlConfins = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Vbcst) && item.Vbcst.Length > 0)
                    {
                        ppp.VlBaseSt = ConvertToDecimal(item.Vbcst);
                    }
                    else
                    {
                        ppp.VlBaseSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Pisvbc) && item.Pisvbc.Length > 0)
                    {
                        ppp.VlBasePis = ConvertToDecimal(item.Pisvbc);
                    }
                    else
                    {
                        ppp.VlBasePis = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofVbc) && item.CofVbc.Length > 0)
                    {
                        ppp.VlBaseConfins = ConvertToDecimal(item.CofVbc);
                    }
                    else
                    {
                        ppp.VlBaseConfins = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Picmsst) && item.Picmsst.Length > 0)
                    {
                        ppp.PorcSt = ConvertToDecimal(item.Picmsst);
                    }
                    else
                    {
                        ppp.PorcSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Pisppis) && item.Pisppis.Length > 0)
                    {
                        ppp.PorcPis = ConvertToDecimal(item.Pisppis);
                    }
                    else
                    {
                        ppp.PorcPis = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofPcofins) && item.CofPcofins.Length > 0)
                    {
                        ppp.PorcConfins = ConvertToDecimal(item.CofPcofins);
                    }
                    else
                    {
                        ppp.PorcConfins = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofstPcofins) && item.CofstPcofins.Length > 0)
                    {
                        ppp.PorcCofinsSt = ConvertToDecimal(item.CofstPcofins);
                    }
                    else
                    {
                        ppp.PorcCofinsSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofstVbc) && item.CofstVbc.Length > 0)
                    {
                        ppp.VlBaseCofinsSt = ConvertToDecimal(item.CofstVbc);
                    }
                    else
                    {
                        ppp.VlBaseCofinsSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.CofstVcofins) && item.CofstVcofins.Length > 0)
                    {
                        ppp.VlCofinsSt = ConvertToDecimal(item.CofstVcofins);
                    }
                    else
                    {
                        ppp.VlCofinsSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Pisstppis) && item.Pisstppis.Length > 0)
                    {
                        ppp.PorcPisSt = ConvertToDecimal(item.Pisstppis);
                    }
                    else
                    {
                        ppp.PorcPisSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Pisstvbc) && item.Pisstvbc.Length > 0)
                    {
                        ppp.VlBasePisSt = ConvertToDecimal(item.Pisstvbc);
                    }
                    else
                    {
                        ppp.VlBasePisSt = 0;
                    }

                    if (!string.IsNullOrEmpty(item.Pisstvpis) && item.Pisstvpis.Length > 0)
                    {
                        ppp.VlPisSt = ConvertToDecimal(item.Pisstvpis);
                    }
                    else
                    {
                        ppp.VlPisSt = 0;
                    }
                    if (!string.IsNullOrEmpty(item.Vicmsdeson) && item.Vicmsdeson.Length > 0)
                    {
                        ppp.VIcmsDeson = ConvertToDecimal(item.Vicmsdeson);
                    }
                    else
                    {
                        ppp.VIcmsDeson = 0;
                    }
                    ppp.VlDespAcess = ConvertToDecimal(item.VlOutros);
                    ppp.FcpBase = ConvertToDecimal(item.FcpBase);
                    ppp.FcpPorc = ConvertToDecimal(item.FcpPorc);
                    ppp.FcpValor = ConvertToDecimal(item.FcpValor);
                    ppp.ImpBaseIcmsStRet = ConvertToDecimal(item.Vbcstret);
                    ppp.ImpBaseIcmsStRet = ConvertToDecimal(item.Vicmsstret);
                    ppp.ImpPst = ConvertToDecimal(item.Pst);
                    ppp.QtTotal = Math.Round(ConvertToDecimal(item.Qtrib) * (amarracao.FatorConversao ?? 1), 4);

                    if (!string.IsNullOrEmpty(item.VeicChassi) && item.VeicChassi.Length > 0)
                    {
                        ppp.TpOperacaoVeic = Convert.ToInt32(item.VeicTpop);
                        ppp.ChasiVeic = item.VeicChassi;
                        ppp.CorVeic = item.VeicCcordenatran;
                        ppp.DescCorVeic = item.VeicXcor;
                        ppp.PotenciaMotorVeic = item.VeicPot;
                        ppp.CilindradasVeic = item.VeicCilin;
                        ppp.PesoLiquidoVeic = ConvertToDecimal(item.VeicPesol);
                        ppp.PesoBrutoVeic = ConvertToDecimal(item.VeicPesob);
                        ppp.SerialVeic = item.VeicNserie;
                        ppp.TpCombustVeic = item.VeicTpcomb;
                        ppp.NrMotorVeic = item.VeicNmotor;
                        ppp.CapcMaxTracVeic = ConvertToDecimal(item.VeicCmt);
                        ppp.DistEixosVeic = item.VeicDist;
                        ppp.AnoVeic = item.VeicAnomod;
                        ppp.AnoFabVeic = item.VeicAnofab;
                        ppp.TpPinturaVeic = item.VeicTppint;
                        ppp.TpVeic = item.VeicTpveic;
                        ppp.EspecVeic = item.VeicEspveic;
                        ppp.IdVinVeic = item.VeicVin;
                        ppp.CondVeic = item.VeicCondveic;
                        ppp.IdMarcaVeic = item.VeicCmod;
                        ppp.IdCorVeic = item.VeciCcor;
                        ppp.CapcMaxLotVeic = item.VeicLota;
                        ppp.RestricaoVeic = item.VeicTprest;
                    }

                    ProdutoEntradaDto produtoEntradaDto = mapper.Map<ProdutoEntradaDto>(ppp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(ENDPOINT_PRODUTO_ENTRADA);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_PRODUTO_ENTRADA, produtoEntradaDto);
                    if (response.IsSuccessStatusCode)
                    {
                        var _response = await response.Content.ReadFromJsonAsync<ProdutoEntradum>();
                        if (_response != null)
                        {
                            ppp = _response;
                            logger.LogInformation($"Produto entrada id [{idEmpresa},{_response.NrEntrada},{_response.NrItem}] criado com sucesso.");
                            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", $"Item {ppp.NrItem} - {produtoEstoque.NmProduto} importado com sucesso...");
                        }
                        else
                        {
                            logger.LogError($"Erro ao importar Produto entrada ({entrada.CdChaveNfe}).");
                            throw new Exception("Erro ao importar Produto entrada");
                        }
                    }
                    else
                    {
                        logger.LogError($"Erro ao importar Produto entrada ({entrada.CdChaveNfe}).");
                        throw new Exception("Erro ao importar Produto entrada");
                    }


                    await GravarAmarracao(idEmpresa, ppp, item, amarracao, entrada, unity);
                }
            }
        }

        private async Task GravarAmarracao(int idEmpresa, ProdutoEntradum ppp, Impitensnfe item, Amarracao2 amarracao, Entrada entrada, int unity)
        {
            string vsql = $@" 
                        SELECT 
                            *
                        FROM 
                            produtos_forn 
                        WHERE 
                            cd_produto = {ppp.CdProduto} 
                            AND id_empresa = {idEmpresa} 
                            AND cd_forn = {entrada.CdForn} 
                            AND id_produto_externo = '{item.CProd}'";
            ProdutosForn? produtosForn = await db.ProdutosForns.FromSqlRaw(vsql).FirstOrDefaultAsync();
            if (produtosForn == null)
            {
                produtosForn = new ProdutosForn();
                produtosForn.CdProduto = ppp.CdProduto;
                produtosForn.Unity = unity;
                produtosForn.CdForn = entrada.CdForn;
                produtosForn.IdProdutoExterno = item.CProd;

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
                        logger.LogInformation($"ProdutosForn id [{idEmpresa},{_response.Id}] criado com sucesso.");

                    }
                    else
                    {
                        logger.LogError($"Erro ao importar ProdutosForn ({entrada.CdChaveNfe}).");
                        throw new Exception("Erro ao importar ProdutosForn");
                    }
                }
                else
                {
                    logger.LogError($"Erro ao importar ProdutosForn ({entrada.CdChaveNfe}).");
                    throw new Exception("Erro ao importar ProdutosForn");
                }
            }
        }

        private async Task AtualizarDadosFiscais(Impitensnfe item, ProdutoEntradum ppp, Amarracao2 amarracao, Entrada entrada, int idEmpresa, ProdutoEstoque produtoEstoque)
        {
            CfopImportacao? cfopImportacao = await db.CfopImportacaos.FromSqlRaw(
                $@"
                SELECT 
                  id,
                  cd_cfop_s,
                  cd_cfop_e,
                  cfop_dentro,
                  cfop_fora,
                  csosn,
                  id_empresa
                FROM 
                  public.cfop_importacao 
                WHERE
                  id_empresa = {idEmpresa}
                AND cd_cfop_s = '{item.Cfop}'
                 "
                ).FirstOrDefaultAsync();
            if (cfopImportacao != null)
            {
                if (!string.IsNullOrEmpty(cfopImportacao.CdCfopE) && cfopImportacao.CdCfopE.Length == 4)
                {
                    ppp.CdCfop = cfopImportacao.CdCfopE;
                }
                else
                {
                    string cfopNotaDeEntrada = item.Cfop ?? "";

                    ppp.CdCfop = GetCfopPadrao(cfopNotaDeEntrada);
                }

                if (!string.IsNullOrEmpty(cfopImportacao.CfopDentro) && cfopImportacao.CfopDentro.Length == 4)
                {
                    produtoEstoque.CfoDentro = cfopImportacao.CfopDentro;
                }
                if (!string.IsNullOrEmpty(cfopImportacao.CfopFora) && cfopImportacao.CfopFora.Length == 4)
                {
                    produtoEstoque.CfoFora = cfopImportacao.CfopFora;
                }
                if (!string.IsNullOrEmpty(cfopImportacao.Csosn) && cfopImportacao.Csosn.Length >= 3)
                {
                    produtoEstoque.CdCsosn = cfopImportacao.Csosn;
                }
            }
            else
            {
                ppp.CdCfop = GetCfopPadrao(item.Cfop ?? "");
            }
            produtoEstoque.CdClassFiscal = item.Ncm;
            if (!string.IsNullOrEmpty(item.Cest) && item.Cest.Length > 0)
            {
                produtoEstoque.Cest = item.Cest;
            }
            if (!(produtoEstoque.CdBarra.Length > 11 && item.Cean.Equals("SEM GTIN")))
            {
                produtoEstoque.CdBarra = item.Cean;
            }

            ProdutoEstoqueDto produtoEstoqueDto = mapper.Map<ProdutoEstoqueDto>(produtoEstoque);
            string url = ENDPOINT_PRODUTOS + $"/{idEmpresa}/{produtoEstoque.CdProduto}";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PutAsJsonAsync(url, produtoEstoqueDto);
            if (response.IsSuccessStatusCode)
            {
                var _response = await response.Content.ReadFromJsonAsync<ProdutoEstoque>();
                if (_response != null)
                {
                    logger.LogInformation($"Produto id [{idEmpresa},{_response.CdProduto}] ATUALIZADO com sucesso.");
                    produtoEstoque = _response;
                }
                else
                {
                    logger.LogError($"Erro ao ATUALIZAR Produto ({entrada.CdChaveNfe}).");
                    throw new Exception("Erro ao ATUALIZAR Produto");
                }
            }
            else
            {
                logger.LogError($"Erro ao importar ATUALIZAR ({entrada.CdChaveNfe}).");
                throw new Exception("Erro ao importar ATUALIZAR");
            }
        }

        private string? GetCfopPadrao(string cfopNotaDeEntrada)
        {
            string ret = string.Empty;

            // Converte a entrada em um padrão de quatro dígitos, se necessário
            if (cfopNotaDeEntrada.Length < 4)
                return null; // ou um valor de fallback

            string p = cfopNotaDeEntrada;

            // Regras para operações estaduais e interestaduais
            if (p.Equals("6101") || p.Equals("6102") || p.Equals("6103")) // Venda estadual ou interestadual
                ret = "2102"; // Entrada padrão para revenda
            else if (p.Equals("5101") || p.Equals("5102") || p.Equals("5103"))
                ret = "1102";
            else if (p.Equals("5401") || p.Equals("5402") || p.Equals("5403") || p.Equals("5404") || p.Equals("5405"))
                ret = "1403";
            else if (p.Equals("6401") || p.Equals("6402") || p.Equals("6403") || p.Equals("6404") || p.Equals("6405"))
                ret = "2403";

            // Regras para devoluções
            else if (p.Equals("5202")) // Devolução de venda
                ret = "1202"; // Entrada de devolução
            else if (p.Equals("6202"))
                ret = "2202"; // Entrada de devolução
            else if (p.Equals("5411")) // Devolução de bonificação
                ret = "1411";
            else if (p.Equals("6411"))
                ret = "2411";

            // Regras gerais para operações interestaduais e estaduais
            else if (p.StartsWith("6")) // Interestaduais
                ret = "2102"; // Exemplo: Entrada de mercadoria para revenda de fora do estado
            else if (p.StartsWith("5")) // Internas
                ret = "1102"; // Exemplo: Entrada de mercadoria para revenda dentro do estado

            // Se nenhum caso anterior for atendido
            else
                ret = "1102"; // Define um CFOP padrão ou retorna null para tratamento externo

            return ret;
        }


        private async Task ImportarUnidadeMedida(Impitensnfe item, Amarracao2 amarracao, int idEmpresa, int unity)
        {
            string SQLunidade = $@"
                SELECT 
                  *
                FROM 
                  public.unidade_medida
                WHERE 
                  id_empresa = {idEmpresa} AND
                  cd_unidade = '{item.Utrib}'";

            UnidadeMedida? unidade = await db.UnidadeMedidas.FromSqlRaw(SQLunidade).FirstOrDefaultAsync();
            if (unidade == null)
            {
                unidade = new UnidadeMedida();
                unidade.Unity = unity;
                unidade.CdUnidade = item.Utrib;
                unidade.Descricao = item.Utrib;

                UnidadeMedidaDto unidadeMedidaDto = mapper.Map<UnidadeMedidaDto>(unidade);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ENDPOINT_UNIDADE_MEDIDA);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_UNIDADE_MEDIDA, unidadeMedidaDto);
                if (response.IsSuccessStatusCode)
                {
                    var _response = await response.Content.ReadFromJsonAsync<UnidadeMedida>();
                    if (_response != null)
                    {
                        logger.LogInformation($"UnidadeMedida id [{idEmpresa},{_response.Id}] criado com sucesso.");

                    }
                    else
                    {
                        logger.LogError($"Erro ao importar UnidadeMedida.");
                        throw new Exception("Erro ao importar UnidadeMedida");
                    }
                }
                else
                {
                    logger.LogError($"Erro ao importar UnidadeMedida.");
                    throw new Exception("Erro ao importar UnidadeMedida");
                }
            }
        }

        private async Task<Entrada?> ImportarEntrada(int idFornecedor, Transportadora? transportadora, ImpNFeTemp2 impNFeTemp, int idEmpresa, int cdPlanoEstoque, string tipoEntrada)
        {
            Entrada? entrada = db.Entradas.FromSqlRaw($@"
                SELECT 
                    * 
                FROM 
                    public.entradas 
                WHERE 
                    cd_empresa = {idEmpresa} 
                    AND cd_chave_nfe = '{impNFeTemp.impcabnfe?.ChNfe}'").FirstOrDefault();

            if (entrada == null)
            {
                entrada = new Entrada();
                DateOnly data = DateUtils.DateTimeToDateOnly(DateTime.Now);
                entrada.Data = data;
                entrada.DtEmissao = DateUtils.DateTimeToDateOnly(impNFeTemp.impcabnfe.DtEmissao ?? DateTime.Now);
                if (impNFeTemp.impcabnfe.DtSaida == new DateTime(1899, 12, 30))
                {
                    entrada.DtSaida = data;
                }
                else
                {
                    entrada.DtSaida = DateUtils.DateTimeToDateOnly(impNFeTemp.impcabnfe.DtSaida ?? DateTime.Now);
                }
                entrada.CdForn = idFornecedor;
                entrada.CdCfop = await GetCfop(idEmpresa, impNFeTemp);
                entrada.CdEmpresa = idEmpresa;
                if (transportadora != null)
                {
                    entrada.Transp = transportadora.CdTransportadora;
                }
                entrada.CdTipoNf = "55";
                entrada.CdChaveNfe = impNFeTemp.impcabnfe.ChNfe;
                entrada.SerieNf = impNFeTemp.impcabnfe.Serie;
                entrada.NrNf = impNFeTemp.impcabnfe.NrNf;
                entrada.ModeloNf = impNFeTemp.impcabnfe.Modelo;
                entrada.TxtObs = impNFeTemp.impcabnfe.InfObs;
                entrada.TPag = impNFeTemp.impcabnfe.TPag;

                decimal vIcmsD = 0;
                try
                {
                    vIcmsD = ConvertToDecimal(impNFeTemp.imptotalnfe.Vicmsdeson ?? "0");
                }
                catch
                {
                    vIcmsD = 0;
                }
                entrada.VIcmsDeson = vIcmsD;

                if ((impNFeTemp.impcabnfe.TpPagt ?? "").Equals("2"))
                    impNFeTemp.impcabnfe.TpPagt = "P";
                else impNFeTemp.impcabnfe.TpPagt = "V";

                int tpFrete;
                try
                {
                    tpFrete = Convert.ToInt32(impNFeTemp.impcabnfe.TpFrete);
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao converter frete (tpFrete)");
                }
                switch (tpFrete)
                {
                    case 0:
                        entrada.TpFrete = 1;
                        break;
                    case 1:
                        entrada.TpFrete = 2;
                        break;
                    case 3:
                        entrada.TpFrete = 0;
                        break;
                    case 4:
                        entrada.TpFrete = 0;
                        break;
                }
                entrada.VlFrete = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsFrete ?? "0");
                entrada.VlSeguro = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsSeg ?? "0");
                entrada.VlOutras = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsOutros ?? "0");
                entrada.VlDescontoNf = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsDesc ?? "0");
                entrada.VlStNf = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsSt ?? "0");
                entrada.VlAcrescimoNf = 0;
                entrada.CdGrupoEstoque = cdPlanoEstoque;
                entrada.TpEntrada = tipoEntrada;
                entrada.TpPagt = (impNFeTemp.impcabnfe.TPag ?? "").Equals("01") ? "V" : "P";
                entrada.HrSaida = DateUtils.DateTimeToTimeOnly(impNFeTemp.impcabnfe.DtSaida ?? DateTime.Now).TruncateToMinutes();
                entrada.HrChegada = DateUtils.DateTimeToTimeOnly(DateTime.Now).TruncateToMinutes();

                entrada.IcmstotVBc = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVbc ?? "0");
                entrada.IcmstotVIcms = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsValor ?? "0");
                entrada.IcmstotVIcmsDeson = ConvertToDecimal(impNFeTemp.imptotalnfe.Vicmsdeson ?? "0");
                entrada.IcmstotVFcp = ConvertToDecimal( "0");
                entrada.IcmstotVBcst = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVbcst ?? "0");
                entrada.IcmstotVSt = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsSt ?? "0");
                entrada.IcmstotVFcpst = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVfcpst ?? "0");
                entrada.IcmstotVFcpstRet = ConvertToDecimal( "0");
                entrada.IcmstotVProd = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVprod ?? "0");
                entrada.IcmstotVFrete = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsFrete ?? "0");
                entrada.IcmstotVSeg = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsSeg ?? "0");
                entrada.IcmstotVDesc = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsDesc ?? "0");
                entrada.IcmstotVOutro = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsOutros ?? "0");
                entrada.IcmstotVIi = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVii ?? "0");
                entrada.IcmstotVIpi = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVipi ?? "0");
                entrada.IcmstotVIpiDevol = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVipidevol ?? "0");
                entrada.IcmstotVPis = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVpis ?? "0");
                entrada.IcmstotVCofins = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVconfins ?? "0");
                entrada.IcmstotVOutro = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsOutros ?? "0");
                entrada.IcmstotVNf = ConvertToDecimal(impNFeTemp.imptotalnfe.IcmsVnf ?? "0");
                    

                EntradaDto entradaDto = mapper.Map<EntradaDto>(entrada);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ENDPOINT_ENTRADA);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_ENTRADA, entradaDto);
                if (response.IsSuccessStatusCode)
                {
                    var _response = await response.Content.ReadFromJsonAsync<Entrada>();
                    if (_response != null)
                    {
                        logger.LogInformation($"Entrada id [{idEmpresa},{_response.Nr}] criado com sucesso.");
                        entrada = _response;
                    }
                    else
                    {
                        logger.LogError($"Erro ao importar Entrada ({entrada.CdChaveNfe}).");
                        throw new Exception("Erro ao importar Entrada");
                    }
                }
                else
                {
                    logger.LogError($"Erro ao importar Entrada ({entrada.CdChaveNfe}).");
                    throw new Exception("Erro ao importar Entrada");
                }
            }
            return entrada;
        }

        private async Task<string?> GetCfop(int idEmpresa, ImpNFeTemp2 impNFeTemp)
        {
            string cfop = impNFeTemp.impitensnves.First().Cfop;
            if (string.IsNullOrEmpty(cfop))
            {
                return string.Empty;
            }
            string SQLcfop = $@"
                SELECT 
                  *
                FROM 
                  public.cfop_importacao
                WHERE 
                  id = {idEmpresa} AND
                  cd_cfop_s = '{cfop}'";

            CfopImportacao? cfopImportacao = await db.CfopImportacaos.FromSqlRaw(SQLcfop).FirstOrDefaultAsync();
            if (cfopImportacao == null)
            {
                return null;
            }
            return cfopImportacao.CdCfopE;
        }

        private async Task<Transportadora?> ImportarTransportadora(ImpNFeTemp2 impNFeTemp, int idEmpresa, int unity)
        {
            if (string.IsNullOrEmpty(impNFeTemp?.impcabnfe?.CnpjTransp))
            {
                return null;
            }
            string cnpj = UtlStrings.OnlyInteger(impNFeTemp?.impcabnfe?.CnpjTransp);
            string SQLtransp = $@"
                        SELECT 
                          *
                        FROM 
                          public.transportadora
                        WHERE 
                          id_empresa = {idEmpresa} AND
                          regexp_replace(cd_cnpj, '[^0-9]', '', 'g') = '{cnpj}'";

            Transportadora? transportadora = await db.Transportadoras.FromSqlRaw(SQLtransp).FirstOrDefaultAsync();
            if (transportadora == null)
            {
                transportadora = new Transportadora();
                transportadora.NmTransportadora = impNFeTemp.impcabnfe?.NomeTransp;
                transportadora.CdCnpj = cnpj;
                transportadora.Numero = 0;
                transportadora.NmEndereco = impNFeTemp.impcabnfe?.EndTransp;
                transportadora.CdCidade = await GetCdCidade(idEmpresa, impNFeTemp.impcabnfe?.CidadeTransp);
                transportadora.Unity = unity;

                TransportadoraDto transportadoraDto = mapper.Map<TransportadoraDto>(transportadora);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(ENDPOINT_TRANSPORTADORA);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync(ENDPOINT_TRANSPORTADORA, transportadoraDto);
                if (response.IsSuccessStatusCode)
                {
                    var _response = await response.Content.ReadFromJsonAsync<Transportadora>();
                    if (_response != null)
                    {
                        logger.LogInformation($"Transportadora id [{idEmpresa},{_response.CdTransportadora}] criado com sucesso.");
                        transportadora = _response;
                    }
                    else
                    {
                        logger.LogError($"Erro ao importar Transportadora ({impNFeTemp?.impcabnfe?.ChNfe}).");
                        throw new Exception("Erro ao importar Transportadora");
                    }
                }
                else
                {
                    logger.LogError($"Erro ao importar Transportadora ({impNFeTemp?.impcabnfe?.ChNfe}).");
                    throw new Exception("Erro ao importar Transportadora");
                }
            }
            return transportadora;
        }

        private async Task<string?> GetCdCidade(int idEmpresa, string? cidadeTransp)
        {
            string SQL = $@"
                SELECT 
                  cd_cidade,
                  nm_cidade,
                  uf
                FROM 
                  public.cidade
                WHERE upper(trim(nm_cidade)) LIKE '%{(cidadeTransp ?? "SAO SEBASTIAO DO PARAISO").Trim().ToUpper()}%'
";
            Cidade? cidade = await db.Cidades.FromSqlRaw(SQL).FirstOrDefaultAsync();
            if (cidade == null)
            {
                return "3164704";
            }
            return cidade.CdCidade;

        }
        private decimal ConvertToDecimal(string? Decimal)
        {
            if (Decimal.IsNullOrEmpty())
            {
                Decimal = "0";
            }
            return Convert.ToDecimal(Decimal);
        }
        private decimal ConvertToDecimal(decimal? Decimal)
        {
            if (Decimal == null)
            {
                Decimal = 0;
            }
            if (Decimal < 0)
            {
                Decimal = 0;
            }
            return Convert.ToDecimal(Decimal);
        }
    }
}
