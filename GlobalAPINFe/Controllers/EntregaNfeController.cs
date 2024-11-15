using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    public class EntregaNfeController : GenericPagedController<EntregaNfe, int, EntregaNfeDto>
    {
        public EntregaNfeController(IQueryRepository<EntregaNfe, int, EntregaNfeDto> repo, ILogger<GenericPagedController<EntregaNfe, int, EntregaNfeDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<EntregaNfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<EntregaNfe>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntregaNfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<EntregaNfe>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EntregaNfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<EntregaNfe>> Create([FromBody] EntregaNfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<EntregaNfe>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<EntregaNfe>>> CreateBulk([FromBody] IEnumerable<EntregaNfeDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EntregaNfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<EntregaNfe>> Update(int id, [FromBody] EntregaNfeDto dto)
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

        [HttpGet("GetEntregaNfePorCliente", Name = nameof(GetEntregaNfePorCliente))]
        [ProducesResponseType(typeof(PagedResponse<EntregaNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<EntregaNfe>>> GetEntregaNfePorCliente(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((EntregaNfeRepository)repo).GetEntregaNfeAsyncPorCliente(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<EntregaNfe>(pagedList);

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

        [HttpGet("GetEntregaNfePorCliente_ALL", Name = nameof(GetEntregaNfePorCliente_ALL))]
        [ProducesResponseType(typeof(IEnumerable<EntregaNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<EntregaNfe>>> GetEntregaNfePorCliente_ALL(int idEmpresa)
        {
            try
            {
                var query = ((EntregaNfeRepository)repo).GetEntregaNfeAsyncPorCliente(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.AsNoTracking().ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(list); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
