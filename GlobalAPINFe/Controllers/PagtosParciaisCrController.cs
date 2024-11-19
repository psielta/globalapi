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
    public class PagtosParciaisCrController : GenericPagedController<PagtosParciaisCr, int, PagtosParciaisCrDto>
    {
        public PagtosParciaisCrController(IQueryRepository<PagtosParciaisCr, int, PagtosParciaisCrDto> repo, ILogger<GenericPagedController<PagtosParciaisCr, int, PagtosParciaisCrDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PagtosParciaisCr>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<PagtosParciaisCr>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PagtosParciaisCr), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagtosParciaisCr>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagtosParciaisCr), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PagtosParciaisCr>> Create([FromBody] PagtosParciaisCrDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<PagtosParciaisCr>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<PagtosParciaisCr>>> CreateBulk([FromBody] IEnumerable<PagtosParciaisCrDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PagtosParciaisCr), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagtosParciaisCr>> Update(int id, [FromBody] PagtosParciaisCrDto dto)
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

        [HttpGet("GetPagtosParciaisCrPorCR", Name = nameof(GetPagtosParciaisCrPorCR))]
        [ProducesResponseType(typeof(PagedResponse<PagtosParciaisCr>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<PagtosParciaisCr>>> GetPagtosParciaisCrPorCR(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PagtosParciaisCrRepository)repo).GetPagtosParciaisCrAsyncPorCR(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<PagtosParciaisCr>(pagedList);

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

        [HttpGet("GetPagtosParciaisCrPorCRALL", Name = nameof(GetPagtosParciaisCrPorCRALL))]
        [ProducesResponseType(typeof(IEnumerable<PagtosParciaisCr>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PagtosParciaisCr>>> GetPagtosParciaisCrPorCRALL(int idEmpresa)
        {
            try
            {
                var query = ((PagtosParciaisCrRepository)repo).GetPagtosParciaisCrAsyncPorCR(idEmpresa).Result.AsQueryable();
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
