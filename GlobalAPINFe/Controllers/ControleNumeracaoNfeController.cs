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
    public class ControleNumeracaoNfeController : GenericPagedController<ControleNumeracaoNfe, int, ControleNumeracaoNfeDto>
    {
        public ControleNumeracaoNfeController(IQueryRepository<ControleNumeracaoNfe, int, ControleNumeracaoNfeDto> repo, ILogger<GenericPagedController<ControleNumeracaoNfe, int, ControleNumeracaoNfeDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ControleNumeracaoNfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ControleNumeracaoNfe>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ControleNumeracaoNfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ControleNumeracaoNfe>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ControleNumeracaoNfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ControleNumeracaoNfe>> Create([FromBody] ControleNumeracaoNfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ControleNumeracaoNfe>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ControleNumeracaoNfe>>> CreateBulk([FromBody] IEnumerable<ControleNumeracaoNfeDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ControleNumeracaoNfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ControleNumeracaoNfe>> Update(int id, [FromBody] ControleNumeracaoNfeDto dto)
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

        [HttpGet("GetControleNumeracaoNfePorEmpresa", Name = nameof(GetControleNumeracaoNfePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ControleNumeracaoNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ControleNumeracaoNfe>>> GetControleNumeracaoNfePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ControleNumeracaoNfeRepository)repo).GetControleNumeracaoNfeAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ControleNumeracaoNfe>(pagedList);

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

        [HttpGet("GetControleNumeracaoNfePorEmpresa_ALL", Name = nameof(GetControleNumeracaoNfePorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<ControleNumeracaoNfe>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ControleNumeracaoNfe>>> GetControleNumeracaoNfePorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((ControleNumeracaoNfeRepository)repo).GetControleNumeracaoNfeAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.AsNoTracking().ToListAsync();

                bool hasPadraoTrue = list.Any(x => x.Padrao == true);

                if (list == null || list.Count == 0 || hasPadraoTrue == false)
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
