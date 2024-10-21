using GlobalAPI_ACBrNFe.Models;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("Registrar/{idEmpresa}/{chaveAcesso}", Name = nameof(Registrar))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Entrada>> Registrar([FromBody] ImpNFeTemp impNFeTemp, int idEmpresa, string chaveAcesso)
        {
            if (idEmpresa == 0 || string.IsNullOrEmpty(chaveAcesso)||  chaveAcesso.Length < 44 )
            {
                return BadRequest(new ErrorMessage(500, "Is missing parameters"));
            }
            if (impNFeTemp == null)
            {
                return BadRequest(new ErrorMessage(500, "Is missing body"));
            }
            if (impNFeTemp.amarracoes == null)
            {
                return BadRequest(new ErrorMessage(500, "Is missing NFe"));
            }

            throw new NotImplementedException();
        }
    }
}
