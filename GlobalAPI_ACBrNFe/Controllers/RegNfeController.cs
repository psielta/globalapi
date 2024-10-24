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

        [HttpPost("Registrar/{idEmpresa}/{chaveAcesso}/{cdPlanoEstoque}", Name = nameof(Registrar))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Entrada>> Registrar([FromBody] ImpNFeTemp impNFeTemp, int idEmpresa, int cdPlanoEstoque, string chaveAcesso)
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
            try
            {
                Entrada? entrada = await ImportarEntrada(IdFornecedor, transportadora, impNFeTemp, idEmpresa, cdPlanoEstoque);
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

            throw new NotImplementedException();
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
