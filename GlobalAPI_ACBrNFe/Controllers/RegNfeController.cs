using GlobalAPI_ACBrNFe.Models;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Entrada>> Registrar([FromBody] ImpNFeTemp impNFeTemp, int idEmpresa, int cdPlanoEstoque, int cdHistorico, string chaveAcesso)
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
                fornecedor = await db.Fornecedors.FindAsync(IdFornecedor);
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
            #endregion

            throw new NotImplementedException();
        }

        private async Task ImportarItens(ImpNFeTemp impNFeTemp, Entrada entrada, int idEmpresa, int cdPlanoEstoque)
        {
            foreach (var item in impNFeTemp.impitensnves)
            {
                Amarracao? amarracao = impNFeTemp.amarracoes.FirstOrDefault(x => x.NrItem == item.NrItem);
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
                    ppp.Quant = Convert.ToDecimal(item.Qtrib);
                    ppp.VlUnitario = Convert.ToDecimal(item.Vuntrib);
                    await ImportarUnidadeMedida(item, amarracao, idEmpresa);
                    ppp.Unidade = item.Utrib;
                    await AtualizarDadosFiscais(item, ppp, amarracao, entrada, idEmpresa, produtoEstoque);
                }
            }
        }

        private async Task AtualizarDadosFiscais(Impitensnfe item, ProdutoEntradum ppp, Amarracao amarracao, Entrada entrada, int idEmpresa, ProdutoEstoque produtoEstoque)
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
                
                if (string.IsNullOrEmpty(cfopImportacao.CdCfopE) && cfopImportacao.CdCfopE.Length == 4)
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
                if(!string.IsNullOrEmpty(cfopImportacao.Csosn) && cfopImportacao.Csosn.Length >= 3)
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

        private async Task ImportarUnidadeMedida(Impitensnfe item, Amarracao amarracao, int idEmpresa)
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

        private async Task<Entrada?> ImportarEntrada(int idFornecedor, Transportadora? transportadora, ImpNFeTemp impNFeTemp, int idEmpresa, int cdPlanoEstoque)
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
                DateOnly data = DateUtils.DateTimeToDateOnly(impNFeTemp.impcabnfe?.DtEmissao ?? DateTime.Now);
                entrada.Data = data;
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
                    vIcmsD = Convert.ToDecimal(impNFeTemp.imptotalnfe.Vicmsdeson ?? "0");
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
                entrada.VlFrete = Convert.ToDecimal(impNFeTemp.imptotalnfe.IcmsFrete ?? "0");
                entrada.VlSeguro = Convert.ToDecimal(impNFeTemp.imptotalnfe.IcmsSeg ?? "0");
                entrada.VlOutras = Convert.ToDecimal(impNFeTemp.imptotalnfe.IcmsOutros ?? "0");
                entrada.VlDescontoNf = Convert.ToDecimal(impNFeTemp.imptotalnfe.IcmsDesc ?? "0");
                entrada.VlStNf = Convert.ToDecimal(impNFeTemp.imptotalnfe.IcmsSt ?? "0");
                entrada.CdGrupoEstoque = cdPlanoEstoque;

                db.Entradas.Add(entrada);
                await db.SaveChangesAsync();
            }
            return entrada;
        }

        private async Task<string?> GetCfop(int idEmpresa, ImpNFeTemp impNFeTemp)
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

        private async Task<Transportadora?> ImportarTransportadora(ImpNFeTemp impNFeTemp, int idEmpresa)
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
                transportadora.CdCidade = impNFeTemp.impcabnfe?.CidadeTransp;
                transportadora.IdEmpresa = idEmpresa;

                db.Transportadoras.Add(transportadora);
                await db.SaveChangesAsync();
            }
            return transportadora;
        }

    }
}
