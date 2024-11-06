using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    public class ObsNfController : GenericPagedController<ObsNf, int, ObsNfDto>
    {
        public ObsNfController(IQueryRepository<ObsNf, int, ObsNfDto> repo, ILogger<GenericPagedController<ObsNf, int, ObsNfDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ObsNf>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ObsNf>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ObsNf), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ObsNf>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ObsNf), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ObsNf>> Create([FromBody] ObsNfDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ObsNf>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ObsNf>>> CreateBulk([FromBody] IEnumerable<ObsNfDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ObsNf), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ObsNf>> Update(int id, [FromBody] ObsNfDto dto)
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

        [HttpGet("GetObsNfPorEmpresa", Name = nameof(GetObsNfPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ObsNf>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ObsNf>>> GetObsNfPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ObsNfRepository)repo).GetObsNfAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(e => e.NrLanc);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ObsNf>(pagedList);

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

        [HttpGet("GetObsNfPorEmpresa_ALL", Name = nameof(GetObsNfPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<ObsNf>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ObsNf>>> GetObsNfPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((ObsNfRepository)repo).GetObsNfAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
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
