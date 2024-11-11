using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NcmProtocoloEstadoController : GenericPagedController<NcmProtocoloEstado, int, NcmProtocoloEstadoDto>
    {
        public NcmProtocoloEstadoController(IQueryRepository<NcmProtocoloEstado, int, NcmProtocoloEstadoDto> repo, ILogger<GenericPagedController<NcmProtocoloEstado, int, NcmProtocoloEstadoDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NcmProtocoloEstado>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NcmProtocoloEstado>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NcmProtocoloEstado), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NcmProtocoloEstado>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NcmProtocoloEstado), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NcmProtocoloEstado>> Create([FromBody] NcmProtocoloEstadoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<NcmProtocoloEstado>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<NcmProtocoloEstado>>> CreateBulk([FromBody] IEnumerable<NcmProtocoloEstadoDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NcmProtocoloEstado), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NcmProtocoloEstado>> Update(int id, [FromBody] NcmProtocoloEstadoDto dto)
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

        [HttpGet("GetAllNcmProtocoloEstadoPorEmpresa/{idEmpresa}", Name = nameof(GetAllNcmProtocoloEstadoPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<NcmProtocoloEstado>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<NcmProtocoloEstado>>> GetAllNcmProtocoloEstadoPorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((NcmProtocoloEstadoRepository)repo).GetNcmProtocoloEstadoAsyncPorEmpresa(idEmpresa);

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

        [HttpGet("GetNcmProtocoloEstadoPorEmpresa", Name = nameof(GetNcmProtocoloEstadoPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<NcmProtocoloEstado>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<NcmProtocoloEstado>>> GetNcmProtocoloEstadoPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((NcmProtocoloEstadoRepository)repo).GetNcmProtocoloEstadoAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(e => e.Id);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<NcmProtocoloEstado>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
