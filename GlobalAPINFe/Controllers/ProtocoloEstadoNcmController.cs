using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtocoloEstadoNcmController : GenericPagedController<ProtocoloEstadoNcm, int, ProtocoloEstadoNcmDto>
    {
        public ProtocoloEstadoNcmController(IQueryRepository<ProtocoloEstadoNcm, int, ProtocoloEstadoNcmDto> repo, ILogger<GenericPagedController<ProtocoloEstadoNcm, int, ProtocoloEstadoNcmDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProtocoloEstadoNcm>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ProtocoloEstadoNcm>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProtocoloEstadoNcm), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProtocoloEstadoNcm>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProtocoloEstadoNcm), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProtocoloEstadoNcm>> Create([FromBody] ProtocoloEstadoNcmDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ProtocoloEstadoNcm>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ProtocoloEstadoNcm>>> CreateBulk([FromBody] IEnumerable<ProtocoloEstadoNcmDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProtocoloEstadoNcm), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProtocoloEstadoNcm>> Update(int id, [FromBody] ProtocoloEstadoNcmDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        // Métodos personalizados ajustados

        [HttpGet("GetAllProtocoloEstadoNcmPorEmpresa/{idEmpresa}", Name = nameof(GetAllProtocoloEstadoNcmPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<ProtocoloEstadoNcm>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProtocoloEstadoNcm>>> GetAllProtocoloEstadoNcmPorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((ProtocoloEstadoNcmRepository)repo).GetProtocoloEstadoNcmAsyncPorEmpresa(idEmpresa);

                if (entitysFilterByEmpresa == null)
                {
                    return NotFound("Entities not found.");
                }
                if (entitysFilterByEmpresa.Count() == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(entitysFilterByEmpresa);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
