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
    public class SangriaCaixaController : GenericPagedControllerMultiKey<SangriaCaixa, int, int, SangriaCaixaDto>
    {
        public SangriaCaixaController(IQueryRepositoryMultiKey<SangriaCaixa, int, int, SangriaCaixaDto> repo, ILogger<GenericPagedControllerMultiKey<SangriaCaixa, int, int, SangriaCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<SangriaCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<SangriaCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(SangriaCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SangriaCaixa>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SangriaCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<SangriaCaixa>> Create([FromBody] SangriaCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(SangriaCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SangriaCaixa>> Update(int idEmpresa, int idCadastro, [FromBody] SangriaCaixaDto dto)
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
