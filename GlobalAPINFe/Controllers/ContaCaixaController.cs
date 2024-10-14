using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
    public class ContaCaixaController : GenericPagedController<ContaDoCaixa, int, ContaCaixaDto>
    {
        public ContaCaixaController(IQueryRepository<ContaDoCaixa, int, ContaCaixaDto> repo, ILogger<GenericPagedController<ContaDoCaixa, int, ContaCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetContaDoCaixaPorEmpresa", Name = nameof(GetContaDoCaixaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContaDoCaixaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ContaCaixaRepository)repo).GetContaDoCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ContaDoCaixa>(pagedList);

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
