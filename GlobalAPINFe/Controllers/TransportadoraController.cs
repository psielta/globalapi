using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportadoraController : GenericPagedControllerMultiKey<Transportadora, int, int, TransportadoraDto>
    {
        public TransportadoraController(IQueryRepositoryMultiKey<Transportadora, int, int, TransportadoraDto> repo, ILogger<GenericPagedControllerMultiKey<Transportadora, int, int, TransportadoraDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetTransportadoraPorEmpresa", Name = nameof(GetTransportadoraPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTransportadoraPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmTransp = null,
            [FromQuery] int? cdTransp = null,
            [FromQuery] string? cnpj = null)
        {
            try
            {
                var query = ((TransportadoraPagedRepository)repo).GetTransportadoraPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmTransp))
                {
                    var normalizednmTransp = UtlStrings.RemoveDiacritics(nmTransp.ToLower().Trim());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmTransportadora == null) ? "" : p.NmTransportadora.ToLower()).Contains(normalizednmTransp));
                }

                if (cdTransp.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdTransportadora == cdTransp.Value);
                }

                if (!string.IsNullOrEmpty(cnpj))
                {
                    var normalizeCnpj = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cnpj.Trim().ToLower()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.CdCnpj == null) ? "" : p.CdCnpj.Trim().ToLower())) == normalizeCnpj);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.CdTransportadora);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Transportadora>(pagedList);

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

        [HttpGet("GetTransportadoraByName/{idEmpresa}/{nome}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTransportadoraByName(int idEmpresa, string nome)
        {
            var Transportadoras = await (repo as TransportadoraPagedRepository).GetTransportadoraPorEmpresa(idEmpresa);

            var TransportadorasList = Transportadoras.ToList();
            if (TransportadorasList == null)
            {
                return NotFound("Transportadora não encontrada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = TransportadorasList.Where(c => UtlStrings.RemoveDiacritics(c.NmTransportadora.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmTransportadora)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Transportadoras não encontrada.");
            }
            return Ok(filter);
        }
    }
}
