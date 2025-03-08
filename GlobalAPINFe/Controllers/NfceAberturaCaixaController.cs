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
    public class NfceAberturaCaixaController : GenericPagedControllerMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto>
    {
        public NfceAberturaCaixaController(IQueryRepositoryMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto> repo, ILogger<GenericPagedControllerMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NfceAberturaCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NfceAberturaCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceAberturaCaixa>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NfceAberturaCaixa>> Create([FromBody] NfceAberturaCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceAberturaCaixa>> Update(int idEmpresa, int idCadastro, [FromBody] NfceAberturaCaixaDto dto)
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
