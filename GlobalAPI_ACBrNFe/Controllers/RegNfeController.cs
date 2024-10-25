using GlobalAPI_ACBrNFe.Models;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegNfeController : Controller
    {
        protected GlobalErpFiscalBaseContext db;
        protected readonly ILogger<ImpNfeController> logger;
        private string ENDPOINT_POST_FORNECECOR;
        public RegNfeController(GlobalErpFiscalBaseContext db, ILogger<ImpNfeController> logger)
        {
            this.db = db;
            this.logger = logger;
            ENDPOINT_POST_FORNECECOR = Constants.URL_API_NFE + "/api/Fornecedor";
        }

        [HttpPost("Registrar/{idEmpresa}/{chaveAcesso}/{cdPlanoEstoque}/{cdHistorico}", Name = nameof(Registrar))]
        [ProducesResponseType(typeof(Entrada), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Entrada>> Registrar([FromBody] ImpNFeTemp2 impNFeTemp, int idEmpresa, int cdPlanoEstoque, int cdHistorico, string chaveAcesso)
        {
            #region Validações
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
            #endregion
            #region Busca Fornecedor
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
            Transportadora? transportadora;
            try
            {
                transportadora = await ImportarTransportadora(impNFeTemp, idEmpresa);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro importação de transportadora ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing Transp"));
            }
            #endregion
            #region Entrada
            Entrada? entrada;
            try
            {
                entrada = await ImportarEntrada(IdFornecedor, transportadora, impNFeTemp, idEmpresa, cdPlanoEstoque);
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
            #endregion
            #region Importa XML
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
            try
            {
                await ImportarItens(impNFeTemp, entrada, idEmpresa, cdPlanoEstoque);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar itens ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing items"));
            }
            #endregion
            #region Duplicata
            try
            {
                await ImportarDuplicatas(impNFeTemp, entrada, idEmpresa, cdHistorico);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro ao importar duplicatas ({chaveAcesso}).");
                return BadRequest(new ErrorMessage(500, "Error importing Duplicatas"));
            }
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
            db.Update(entrada);
            await db.SaveChangesAsync();
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
                ContasAPagar contasAPagar = new ContasAPagar();
                contasAPagar.DtLancamento = DateUtils.DateTimeToDateOnly(DateTime.Now);
                contasAPagar.DtVencimento = dataVencimento;
                contasAPagar.CdFornecedor = entrada.CdForn;
                contasAPagar.NrDuplicata = $"ENT{entrada.Nr}";
                contasAPagar.VlCp = ConvertToDecimal(item.Valor);
                contasAPagar.VlDesconto = ConvertToDecimal(0);
                contasAPagar.VlTotal = ConvertToDecimal(item.Valor);
                contasAPagar.Pagou = "N";
                contasAPagar.CdEmpresa = idEmpresa;
                contasAPagar.NrNf = entrada.Nr.ToString();
                contasAPagar.CdPlanoCaixa = historicoCaixa.CdPlano;
                contasAPagar.CdHistoricoCaixa = historicoCaixa.CdSubPlano;
                contasAPagar.NrEntrada = entrada.Nr;
                contasAPagar.TpFormaPagt = entrada.TPag;
                contasAPagar.Rate = 0;
                contasAPagar.NumberOfPayments = count;
                contasAPagar.TypeRegister = (count > 0) ? 1 : 0;
                contasAPagar.Type = 1;

                db.ContasAPagars.Add(contasAPagar);
                await db.SaveChangesAsync();
            }

        }

        private async Task ImportarItens(ImpNFeTemp2 impNFeTemp, Entrada entrada, int idEmpresa, int cdPlanoEstoque)
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
                    await ImportarUnidadeMedida(item, amarracao, idEmpresa);
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

                    db.ProdutoEntrada.Add(ppp);
                    await db.SaveChangesAsync();
                    await GravarAmarracao(idEmpresa, ppp, item, amarracao, entrada);
                }
            }
        }

        private async Task GravarAmarracao(int idEmpresa, ProdutoEntradum ppp, Impitensnfe item, Amarracao2 amarracao, Entrada entrada)
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
                produtosForn.IdEmpresa = idEmpresa;
                produtosForn.CdForn = entrada.CdForn;
                produtosForn.IdProdutoExterno = item.CProd;
                db.ProdutosForns.Add(produtosForn);
                await db.SaveChangesAsync();
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
                  id = {idEmpresa}
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
                    string ret = string.Empty;
                    string p = item.Cfop;
                    if (p.Equals("6101") || p.Equals("6102"))
                        ret = "2102";
                    else if (p.Equals("5101") || p.Equals("5102"))
                        ret = "1102";
                    else if (p.Equals("5401") || p.Equals("5402") || p.Equals("5403") || p.Equals("5404") || p.Equals("5405"))
                        ret = "1403";
                    else if (p.Equals("6401") || p.Equals("6402") || p.Equals("6403") || p.Equals("6404") || p.Equals("6405"))
                        ret = "2403";
                    else if (p[0] == '6')
                        ret = "2102";
                    else if (p[0] == '5')
                        ret = "1102";
                    ppp.CdCfop = ret;
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
            produtoEstoque.CdClassFiscal = item.Ncm;
            if (!string.IsNullOrEmpty(item.Cest) && item.Cest.Length > 0)
            {
                produtoEstoque.Cest = item.Cest;
            }
            if (!(produtoEstoque.CdBarra.Length > 11 && item.Cean.Equals("SEM GTIN")))
            {
                produtoEstoque.CdBarra = item.Cean;
            }
            db.Update(produtoEstoque);
            await db.SaveChangesAsync();
        }

        private async Task ImportarUnidadeMedida(Impitensnfe item, Amarracao2 amarracao, int idEmpresa)
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
                unidade.IdEmpresa = idEmpresa;
                unidade.CdUnidade = item.Utrib;
                unidade.Descricao = item.Utrib;
                db.UnidadeMedidas.Add(unidade);
                await db.SaveChangesAsync();
            }
        }

        private async Task<Entrada?> ImportarEntrada(int idFornecedor, Transportadora? transportadora, ImpNFeTemp2 impNFeTemp, int idEmpresa, int cdPlanoEstoque)
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
                entrada.TpEntrada = "C";
                entrada.TpPagt = (impNFeTemp.impcabnfe.TPag ?? "").Equals("01") ? "V" : "P";
                entrada.HrSaida = DateUtils.DateTimeToTimeOnly(impNFeTemp.impcabnfe.DtSaida ?? DateTime.Now).TruncateToMinutes();
                entrada.HrChegada = DateUtils.DateTimeToTimeOnly(DateTime.Now).TruncateToMinutes();

                db.Entradas.Add(entrada);
                await db.SaveChangesAsync();
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

        private async Task<Transportadora?> ImportarTransportadora(ImpNFeTemp2 impNFeTemp, int idEmpresa)
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
                transportadora.IdEmpresa = idEmpresa;

                db.Transportadoras.Add(transportadora);
                await db.SaveChangesAsync();
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
