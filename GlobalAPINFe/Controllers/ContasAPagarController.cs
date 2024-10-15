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
    public class ContasAPagarController : GenericPagedController<ContasAPagar, int, ContasAPagarDto>
    {
        public ContasAPagarController(IQueryRepository<ContasAPagar, int, ContasAPagarDto> repo, ILogger<GenericPagedController<ContasAPagar, int, ContasAPagarDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetContasAPagarPorEmpresa", Name = nameof(GetContasAPagarPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetContasAPagarPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nrEntrada = null,
            [FromQuery] string? nrDuplicata = null,
            [FromQuery] string? dtVencimentoInicio = null,
            [FromQuery] string? dtVencimentoFim = null,
            [FromQuery] string? cdHistoricoCaixa = null,
            [FromQuery] string? cdPlanoCaixa = null
        )
        {
            try
            {
                var query = await ((ContasAPagarRepository)repo).GetContasAPagarAsyncPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmForn = UtlStrings.RemoveDiacritics(nmForn.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmForn));
                }

                if (nrEntrada.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrEntrada != null && p.NrEntrada.Equals(nrEntrada.Value));
                }

                if (!string.IsNullOrEmpty(nrDuplicata))
                {
                    var normalizedNrDuplicata = UtlStrings.RemoveDiacritics(nrDuplicata.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NrDuplicata == null) ? "" : p.NrDuplicata.ToLower()) == normalizedNrDuplicata);
                }

                if (!string.IsNullOrEmpty(dtVencimentoInicio))
                {
                    filteredQuery = filteredQuery.Where(p => p.DtVencimento >= DateOnly.Parse(dtVencimentoInicio));
                }

                if (!string.IsNullOrEmpty(dtVencimentoFim))
                {
                    filteredQuery = filteredQuery.Where(p => p.DtVencimento <= DateOnly.Parse(dtVencimentoFim));
                }

                if (!string.IsNullOrEmpty(cdHistoricoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdHistoricoCaixa == cdHistoricoCaixa);
                }

                if (!string.IsNullOrEmpty(cdPlanoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdPlanoCaixa == cdPlanoCaixa);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ContasAPagar>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
