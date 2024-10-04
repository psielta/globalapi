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
    public class PlanoEstoqueController : GenericPagedController<PlanoEstoque, int, PlanoEstoqueDto>
    {
        public PlanoEstoqueController(IQueryRepository<PlanoEstoque, int, PlanoEstoqueDto> repo, ILogger<GenericPagedController<PlanoEstoque, int, PlanoEstoqueDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetPlanoEstoquePorEmpresa", Name = nameof(GetPlanoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPlanoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PlanoEstoquePagedRepository)repo).GetPlanoEstoquePorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                

                filteredQuery = filteredQuery.OrderBy(p => p.CdPlano);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<PlanoEstoque>(pagedList);

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
