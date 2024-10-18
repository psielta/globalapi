using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>
    {
        public EntradaController(IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repo, ILogger<GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetEntradaPorEmpresa", Name = nameof(GetEntradaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntradaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nrNotaFiscal = null,
            [FromQuery] int? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoEntrada = null,
            [FromQuery] string? dataInicio = null,
            [FromQuery] string? dataFim = null
            
            )
        {
            try
            {
                var query = ((EntradaPagedRepository)repo).GetEntradaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmProduto = UtlStrings.RemoveDiacritics(nmForn.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmProduto));
                }

                if (nrNotaFiscal.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNf != null && p.NrNf.Equals(nrNotaFiscal.ToString()));
                }

                if (serie.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie.ToString()));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdChaveNfe == null) ? "" : p.CdChaveNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoEntrada))
                {
                    var normalizedTipoEntrada = UtlStrings.RemoveDiacritics(tipoEntrada.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.TpEntrada == null) ? "" : p.TpEntrada.ToLower()) == normalizedTipoEntrada);
                }

                if (!string.IsNullOrEmpty(dataInicio))
                {
                    filteredQuery = filteredQuery.Where(p => p.Data.ToDateTime(TimeOnly.MinValue) >= DateTime.Parse(dataInicio));
                }

                if (!string.IsNullOrEmpty(dataFim))
                {
                    if (DateTime.TryParse(dataFim, out DateTime dtFim))
                    {
                        filteredQuery = filteredQuery.Where(p => p.Data <= dtFim);
                    }
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Entrada>(pagedList);

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
