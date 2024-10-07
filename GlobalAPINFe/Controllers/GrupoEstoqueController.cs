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
    public class GrupoEstoqueController : GenericPagedController<GrupoEstoque, int, GrupoEstoqueDto>
    {
        public GrupoEstoqueController(IQueryRepository<GrupoEstoque, int, GrupoEstoqueDto> repo, ILogger<GenericPagedController<GrupoEstoque, int, GrupoEstoqueDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("GetAllGrupoEstoquePorEmpresa/{idEmpresa}", Name = nameof(GetAllGrupoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllGrupoEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((GrupoEstoquePagedRepository)repo).GetGrupoEstoqueAsyncPorEmpresa(idEmpresa);
                
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

        [HttpGet("GetGrupoEstoquePorEmpresa", Name = nameof(GetGrupoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGrupoEstoquePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((GrupoEstoquePagedRepository)repo).GetGrupoEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<GrupoEstoque>(pagedList);

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

        [HttpGet("GetGrupoEstoqueByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGrupoEstoqueByName(string nome, int idEmpresa)
        {
            var grupos = await (repo as GrupoEstoquePagedRepository).RetrieveAllAsync();
            if (grupos == null)
            {
                return NotFound("Grupo não encontrado.");
            }
            var gruposFilteredByEmpresa = grupos.AsEnumerable().Where(u => u.CdEmpresa == idEmpresa);

            if (gruposFilteredByEmpresa == null)
            {
                return NotFound("Grupos não encontrados para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = gruposFilteredByEmpresa.AsEnumerable().Where(c => UtlStrings.RemoveDiacritics(c.NmGrupo.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmGrupo)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Grupo não encontrados.");
            }
            return Ok(filter);
        }
    }
}
