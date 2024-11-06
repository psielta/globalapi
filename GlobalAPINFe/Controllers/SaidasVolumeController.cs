using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    public class SaidasVolumeController : GenericPagedController<SaidasVolume, int, SaidasVolumeDto>
    {
        public SaidasVolumeController(IQueryRepository<SaidasVolume, int, SaidasVolumeDto> repo, ILogger<GenericPagedController<SaidasVolume, int, SaidasVolumeDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<SaidasVolume>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<SaidasVolume>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaidasVolume), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaidasVolume>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaidasVolume), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<SaidasVolume>> Create([FromBody] SaidasVolumeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaidasVolume), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaidasVolume>> Update(int id, [FromBody] SaidasVolumeDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<SaidasVolume>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<SaidasVolume>>> CreateBulk([FromBody] IEnumerable<SaidasVolumeDto> dtos)
        {
            return await base.CreateBulk(dtos);
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

        [HttpGet("GetSaidasVolumePorEmpresa", Name = nameof(GetSaidasVolumePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<SaidasVolume>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<SaidasVolume>>> GetSaidasVolumePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((SaidasVolumeRepository)repo).GetSaidasVolumePorEmpresa(idEmpresa);
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(c => c.Id);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<SaidasVolume>(pagedList);

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

        [HttpGet("GetAllSaidasVolumePorEmpresa/{idEmpresa}", Name = nameof(GetAllSaidasVolumePorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<SaidasVolume>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SaidasVolume>>> GetAllSaidasVolumePorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((SaidasVolumeRepository)repo).GetSaidasVolumePorEmpresa(idEmpresa);

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
