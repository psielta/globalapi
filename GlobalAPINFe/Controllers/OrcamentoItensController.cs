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
    public class OrcamentoItensController : GenericDtoController<OrcamentoIten, Guid, OrcamentoItensDto>
    {
        public OrcamentoItensController(IRepositoryDto<OrcamentoIten, Guid, OrcamentoItensDto> repo, ILogger<GenericDtoController<OrcamentoIten, Guid, OrcamentoItensDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrcamentoIten>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<OrcamentoIten>>> GetEntities()
        {
            return await base.GetEntities();
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrcamentoIten), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoIten>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrcamentoIten), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OrcamentoIten>> Create([FromBody] OrcamentoItensDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrcamentoIten), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoIten>> Update(Guid id, [FromBody] OrcamentoItensDto dto)
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
