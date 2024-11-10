using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracoesEmpresaController : GenericPagedControllerMultiKey<ConfiguracoesEmpresa, int, string, ConfiguracoesEmpresaDto>
    {
        public ConfiguracoesEmpresaController(IQueryRepositoryMultiKey<ConfiguracoesEmpresa, int, string, ConfiguracoesEmpresaDto> repo, ILogger<GenericPagedControllerMultiKey<ConfiguracoesEmpresa, int, string, ConfiguracoesEmpresaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ConfiguracoesEmpresa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ConfiguracoesEmpresa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ConfiguracoesEmpresa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ConfiguracoesEmpresa>> GetEntity(int idEmpresa, string idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ConfiguracoesEmpresa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ConfiguracoesEmpresa>> Create([FromBody] ConfiguracoesEmpresaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ConfiguracoesEmpresa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ConfiguracoesEmpresa>> Update(int idEmpresa, string idCadastro, [FromBody] ConfiguracoesEmpresaDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, string idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }

        [HttpGet("GetConfiguracoesEmpresaPorEmpresa", Name = nameof(GetConfiguracoesEmpresaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ConfiguracoesEmpresa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ConfiguracoesEmpresa>>> GetConfiguracoesEmpresaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? chave = null)
        {
            try
            {
                var query = await ((ConfiguracoesEmpresaRepository)repo).GetConfiguracoesEmpresaPorEmpresa(idEmpresa);

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
                var response = new PagedResponse<ConfiguracoesEmpresa>(pagedList);

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
