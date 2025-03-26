using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoServicosController : GenericDtoController<OrcamentoServico, Guid, OrcamentoServicosDto>
    {
        public OrcamentoServicosController(IRepositoryDto<OrcamentoServico, Guid, OrcamentoServicosDto> repo, ILogger<GenericDtoController<OrcamentoServico, Guid, OrcamentoServicosDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrcamentoServico>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<OrcamentoServico>>> GetEntities()
        {
            return await base.GetEntities();
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrcamentoServico), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoServico>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrcamentoServico), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OrcamentoServico>> Create([FromBody] OrcamentoServicosDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrcamentoServico), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoServico>> Update(Guid id, [FromBody] OrcamentoServicosDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
