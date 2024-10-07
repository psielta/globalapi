using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosFornController : GenericPagedController<ProdutosForn, int, ProdutosFornDto>
    {
        public ProdutosFornController(IQueryRepository<ProdutosForn, int, ProdutosFornDto> repo, ILogger<GenericPagedController<ProdutosForn, int, ProdutosFornDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("GetProdutosFornPorEmpresa", Name = nameof(GetProdutosFornPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProdutosFornPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cdForn = null,
            [FromQuery] int? cdProduto = null
            )
        {
            try
            {
                var query = ((ProdutosFornRepository)repo).GetProdutosFornAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();


                if (cdForn.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdForn == cdForn.Value);
                }
                
                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ProdutosForn>(pagedList);

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
