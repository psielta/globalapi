using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    public class ProdutoEstoqueController : GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>
    {
        public ProdutoEstoqueController(IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto> repo, ILogger<GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>> logger) : base(repo, logger)
        {
        }

        //[HttpGet("GetProdutoEstoquePorEmpresa", Name = nameof(GetProdutoEstoquePorEmpresa))]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> GetProdutoEstoquePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    try
        //    {
        //        var query = ((ProdutoEstoquePagedRepositoryMultiKey)repo).GetProdutoEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
        //        if (query == null)
        //        {
        //            return NotFound("Entities not found."); // 404 Resource not found
        //        }
        //        query = query.OrderBy(p => p.CdProduto);
        //        var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
        //        var response = new PagedResponse<ProdutoEstoque>(pagedList);

        //        if (response.Items == null || response.Items.Count == 0)
        //        {
        //            return NotFound("Entities not found."); // 404 Resource not found
        //        }
        //        return Ok(response); // 200 OK
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error occurred while retrieving paged entities.");
        //        return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
        //    }
        //}
        [HttpGet("GetProdutoEstoquePorEmpresa", Name = nameof(GetProdutoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProdutoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmProduto = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] string? cdClassFiscal = null,
            [FromQuery] string? cest = null,
            [FromQuery] string? cdInterno = null,
            [FromQuery] string? cdBarra = null)
        {
            try
            {
                var query = ((ProdutoEstoquePagedRepositoryMultiKey)repo).GetProdutoEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmProduto))
                {
                    var normalizedNmProduto = UtlStrings.RemoveDiacritics(nmProduto.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmProduto == null) ? "" : p.NmProduto.ToLower()).Contains(normalizedNmProduto));
                }

                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                if (!string.IsNullOrEmpty(cdClassFiscal))
                {
                    var normalizedCdClassFiscal = UtlStrings.RemoveDiacritics(cdClassFiscal.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdClassFiscal == null) ? "" : p.CdClassFiscal.ToLower()) == normalizedCdClassFiscal);
                }

                if (!string.IsNullOrEmpty(cest))
                {
                    var normalizedCest = UtlStrings.RemoveDiacritics(cest.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.Cest == null) ? "" : p.Cest.ToLower()) == normalizedCest);
                }

                if (!string.IsNullOrEmpty(cdInterno))
                {
                    var normalizedCdInterno = UtlStrings.RemoveDiacritics(cdInterno.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdInterno == null) ? "" : p.CdInterno.ToLower()) == normalizedCdInterno);
                }

                if (!string.IsNullOrEmpty(cdBarra))
                {
                    var normalizedCdBarra = UtlStrings.RemoveDiacritics(cdBarra.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdBarra == null) ? "" : p.CdBarra.ToLower()) == normalizedCdBarra);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.CdProduto);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ProdutoEstoque>(pagedList);

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
