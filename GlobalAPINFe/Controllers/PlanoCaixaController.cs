using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoCaixaController : GenericPagedController<PlanoDeCaixa, int, PlanoCaixaDto>
    {
        public PlanoCaixaController(IQueryRepository<PlanoDeCaixa, int, PlanoCaixaDto> repo, ILogger<GenericPagedController<PlanoDeCaixa, int, PlanoCaixaDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PlanoDeCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<PlanoDeCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlanoDeCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoDeCaixa>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlanoDeCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PlanoDeCaixa>> Create([FromBody] PlanoCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PlanoDeCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoDeCaixa>> Update(int id, [FromBody] PlanoCaixaDto dto)
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

        [HttpGet("GetPlanoDeCaixaPorEmpresa", Name = nameof(GetPlanoDeCaixaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<PlanoDeCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<PlanoDeCaixa>>> GetPlanoDeCaixaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PlanoCaixaRepository)repo).GetPlanoDeCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<PlanoDeCaixa>(pagedList);

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

        [HttpGet("GetPlanoDeCaixaPorEmpresa_ALL", Name = nameof(GetPlanoDeCaixaPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<PlanoDeCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PlanoDeCaixa>>> GetPlanoDeCaixaPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((PlanoCaixaRepository)repo).GetPlanoDeCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
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
