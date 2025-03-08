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
    public class NfceFormaPgtController : GenericPagedControllerMultiKey<NfceFormaPgt, int, int, NfceFormaPgtDto>
    {
        public NfceFormaPgtController(IQueryRepositoryMultiKey<NfceFormaPgt, int, int, NfceFormaPgtDto> repo, ILogger<GenericPagedControllerMultiKey<NfceFormaPgt, int, int, NfceFormaPgtDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NfceFormaPgt>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NfceFormaPgt>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceFormaPgt), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceFormaPgt>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NfceFormaPgt), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NfceFormaPgt>> Create([FromBody] NfceFormaPgtDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceFormaPgt), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceFormaPgt>> Update(int idEmpresa, int idCadastro, [FromBody] NfceFormaPgtDto dto)
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
