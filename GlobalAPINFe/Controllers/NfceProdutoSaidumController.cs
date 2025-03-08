using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfceProdutoSaidumController : GenericPagedControllerMultiKey<NfceProdutoSaidum, int, int, NfceProdutoSaidumDto>
    {
        public NfceProdutoSaidumController(IQueryRepositoryMultiKey<NfceProdutoSaidum, int, int, NfceProdutoSaidumDto> repo, ILogger<GenericPagedControllerMultiKey<NfceProdutoSaidum, int, int, NfceProdutoSaidumDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NfceProdutoSaidum>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NfceProdutoSaidum>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceProdutoSaidum), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceProdutoSaidum>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NfceProdutoSaidum), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NfceProdutoSaidum>> Create([FromBody] NfceProdutoSaidumDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceProdutoSaidum), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceProdutoSaidum>> Update(int idEmpresa, int idCadastro, [FromBody] NfceProdutoSaidumDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, int idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }
    }
}
