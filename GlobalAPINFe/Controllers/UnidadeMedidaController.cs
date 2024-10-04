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
    public class UnidadeMedidaController : GenericPagedController<UnidadeMedida, int, UnidadeMedidaDto>
    {
        public UnidadeMedidaController(IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto> repo, ILogger<GenericPagedController<UnidadeMedida, int, UnidadeMedidaDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("GetUnidadeMedidaPorEmpresa", Name = nameof(GetUnidadeMedidaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUnidadeMedidaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((UnidadeMedidaPagedRepository)repo).GetUnidadeMedidaPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<UnidadeMedida>(pagedList);

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

        [HttpGet("GetAllUnidadeMedidaPorEmpresa/{idEmpresa}", Name = nameof(GetAllUnidadeMedidaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllUnidadeMedidaPorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((UnidadeMedidaPagedRepository)repo).GetUnidadeMedidaPorEmpresa(idEmpresa);

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

        [HttpGet("GetUnidadeByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUnidadeByName(string nome, int idEmpresa)
        {
            var unidades = await repo.RetrieveAllAsync();
            if (unidades == null)
            {
                return NotFound("Unidade não encontrada.");
            }
            var unidadesFilteredByEmpresa = unidades.Where(u => u.IdEmpresa == idEmpresa);

            if (unidadesFilteredByEmpresa == null)
            {
                return NotFound("Unidade não encontrada para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = unidadesFilteredByEmpresa.AsEnumerable()
                .Where(c => UtlStrings.RemoveDiacritics(c.CdUnidade.ToLower()).StartsWith(stringNormalizada))
                .OrderBy(c => c.CdUnidade.Length)
                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Unidade não encontrada.");
            }

            return Ok(filter);
        }
    }
}
