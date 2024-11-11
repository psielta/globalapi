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
    public class IcmController : GenericPagedController<Icm, int, IcmDto>
    {
        public IcmController(IQueryRepository<Icm, int, IcmDto> repo, ILogger<GenericPagedController<Icm, int, IcmDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Icm>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Icm>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Icm), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Icm>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Icm), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Icm>> Create([FromBody] IcmDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Icm>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Icm>>> CreateBulk([FromBody] IEnumerable<IcmDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Icm), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Icm>> Update(int id, [FromBody] IcmDto dto)
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

        [HttpGet("GetAllIcmPorEmpresa/{idEmpresa}", Name = nameof(GetAllIcmPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<Icm>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Icm>>> GetAllIcmPorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((IcmRepository)repo).GetIcmAsyncPorEmpresa(idEmpresa);

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

        [HttpGet("GetIcmPorEmpresa", Name = nameof(GetIcmPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Icm>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Icm>>> GetIcmPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((IcmRepository)repo).GetIcmAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(e => e.NrLanc);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<Icm>(pagedList);

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
