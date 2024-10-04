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
    public class SaldoEstoqueController : GenericPagedController<SaldoEstoque, int, SaldoEstoqueDto>
    {
        public SaldoEstoqueController(IQueryRepository<SaldoEstoque, int, SaldoEstoqueDto> repo, ILogger<GenericPagedController<SaldoEstoque, int, SaldoEstoqueDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("GetSaldoEstoquePorEmpresa", Name = nameof(GetSaldoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSaldoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cdProduto = null,
            [FromQuery] int? cdPlano = null)
        {
            try
            {
                var query = ((SaldoEstoquePagedRepository)repo).GetSaldoEstoquePorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                if (cdPlano.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdPlano == cdPlano.Value);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<SaldoEstoque>(pagedList);

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
