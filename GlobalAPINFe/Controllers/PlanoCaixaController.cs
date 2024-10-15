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
    public class PlanoCaixaController : GenericPagedController<PlanoDeCaixa, int, PlanoCaixaDto>
    {
        public PlanoCaixaController(IQueryRepository<PlanoDeCaixa, int, PlanoCaixaDto> repo, ILogger<GenericPagedController<PlanoDeCaixa, int, PlanoCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetPlanoDeCaixaPorEmpresa", Name = nameof(GetPlanoDeCaixaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPlanoDeCaixaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPlanoDeCaixaPorEmpresa_ALL(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PlanoCaixaRepository)repo).GetPlanoDeCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.ToListAsync();

                if (list  == null || list.Count == 0)
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
