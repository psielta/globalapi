using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagtosParciaisCpController : GenericPagedController<PagtosParciaisCp, int, PagtosParciaisCpDto>
    {
        public PagtosParciaisCpController(IQueryRepository<PagtosParciaisCp, int, PagtosParciaisCpDto> repo, ILogger<GenericPagedController<PagtosParciaisCp, int, PagtosParciaisCpDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PagtosParciaisCp>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<PagtosParciaisCp>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PagtosParciaisCp), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagtosParciaisCp>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagtosParciaisCp), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PagtosParciaisCp>> Create([FromBody] PagtosParciaisCpDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<PagtosParciaisCp>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<PagtosParciaisCp>>> CreateBulk([FromBody] IEnumerable<PagtosParciaisCpDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PagtosParciaisCp), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagtosParciaisCp>> Update(int id, [FromBody] PagtosParciaisCpDto dto)
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

        [HttpGet("GetPagtosParciaisCpPorCP", Name = nameof(GetPagtosParciaisCpPorCP))]
        [ProducesResponseType(typeof(PagedResponse<PagtosParciaisCp>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<PagtosParciaisCp>>> GetPagtosParciaisCpPorCP(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PagtosParciaisCpRepository)repo).GetPagtosParciaisCpAsyncPorCP(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<PagtosParciaisCp>(pagedList);

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

        [HttpGet("GetPagtosParciaisCpPorCPALL", Name = nameof(GetPagtosParciaisCpPorCPALL))]
        [ProducesResponseType(typeof(IEnumerable<PagtosParciaisCp>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PagtosParciaisCp>>> GetPagtosParciaisCpPorCPALL(int idEmpresa)
        {
            try
            {
                var query = ((PagtosParciaisCpRepository)repo).GetPagtosParciaisCpAsyncPorCP(idEmpresa).Result.AsQueryable();
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
