using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenciaEstoqueController : GenericPagedController<ReferenciaEstoque, int, ReferenciaEstoqueDto>
    {
        public ReferenciaEstoqueController(IQueryRepository<ReferenciaEstoque, int, ReferenciaEstoqueDto> repo, ILogger<GenericPagedController<ReferenciaEstoque, int, ReferenciaEstoqueDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetReferenciaEstoquePorEmpresa", Name = nameof(GetReferenciaEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReferenciaEstoquePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ReferenciaEstoquePagedRepository)repo).GetReferenciaEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ReferenciaEstoque>(pagedList);

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
        [HttpGet("GetAllReferenciaEstoquePorEmpresa/{idEmpresa}", Name = nameof(GetAllReferenciaEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllReferenciaEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((ReferenciaEstoquePagedRepository)repo).GetReferenciaEstoqueAsyncPorEmpresa(idEmpresa);
                
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

        [HttpGet("GetReferenciaEstoqueByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReferenciaEstoqueByName(string nome, int idEmpresa)
        {
            var referencias = await (repo as ReferenciaEstoquePagedRepository).RetrieveAllAsync();
            if (referencias == null)
            {
                return NotFound("Referência não encontrada.");
            }
            var referenciasFilteredByEmpresa = referencias.AsEnumerable().Where(u => u.CdEmpresa == idEmpresa);

            if (referenciasFilteredByEmpresa == null)
            {
                return NotFound("Referências não encontradas para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = referenciasFilteredByEmpresa.AsEnumerable().Where(c => UtlStrings.RemoveDiacritics(c.NmRef.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmRef)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Cidade não encontrada.");
            }
            return Ok(filter);
        }
    }
}
