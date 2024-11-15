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
    public class RetiradaNfeController : GenericPagedController<RetiradaNfe, int, RetiradaNfeDto>
    {
        public RetiradaNfeController(IQueryRepository<RetiradaNfe, int, RetiradaNfeDto> repo, ILogger<GenericPagedController<RetiradaNfe, int, RetiradaNfeDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<RetiradaNfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<RetiradaNfe>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RetiradaNfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<RetiradaNfe>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(RetiradaNfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<RetiradaNfe>> Create([FromBody] RetiradaNfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<RetiradaNfe>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<RetiradaNfe>>> CreateBulk([FromBody] IEnumerable<RetiradaNfeDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RetiradaNfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<RetiradaNfe>> Update(int id, [FromBody] RetiradaNfeDto dto)
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

        [HttpGet("GetRetiradaNfePorCliente", Name = nameof(GetRetiradaNfePorCliente))]
        [ProducesResponseType(typeof(PagedResponse<RetiradaNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<RetiradaNfe>>> GetRetiradaNfePorCliente(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((RetiradaNfeRepository)repo).GetRetiradaNfeAsyncPorCliente(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<RetiradaNfe>(pagedList);

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

        [HttpGet("GetRetiradaNfePorCliente_ALL", Name = nameof(GetRetiradaNfePorCliente_ALL))]
        [ProducesResponseType(typeof(IEnumerable<RetiradaNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RetiradaNfe>>> GetRetiradaNfePorCliente_ALL(int idEmpresa)
        {
            try
            {
                var query = ((RetiradaNfeRepository)repo).GetRetiradaNfeAsyncPorCliente(idEmpresa).Result.AsQueryable();
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
