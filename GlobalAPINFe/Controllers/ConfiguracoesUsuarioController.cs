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
    public class ConfiguracoesUsuarioController : GenericPagedControllerMultiKey<ConfiguracoesUsuario, int, string, ConfiguracoesUsuarioDto>
    {
        public ConfiguracoesUsuarioController(IQueryRepositoryMultiKey<ConfiguracoesUsuario, int, string, ConfiguracoesUsuarioDto> repo, ILogger<GenericPagedControllerMultiKey<ConfiguracoesUsuario, int, string, ConfiguracoesUsuarioDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetConfiguracoesUsuarioPorEmpresa", Name = nameof(GetConfiguracoesUsuarioPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetConfiguracoesUsuarioPorEmpresa(
            int idUsuario,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? chave = null)
        {
            try
            {
                var query = await ((ConfiguracoesUsuarioRepository)repo).GetConfiguracoesUsuarioPorUsuario(idUsuario);

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsQueryable().AsEnumerable();

                if (!string.IsNullOrEmpty(chave))
                {
                    var normalizedChaveConf = UtlStrings.RemoveDiacritics(chave.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.Chave == null) ? "" : p.Chave.ToLower()).Contains(normalizedChaveConf));
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Chave);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ConfiguracoesUsuario>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found.");
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
