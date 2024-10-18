using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ConfiguracoesUsuario>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ConfiguracoesUsuario>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ConfiguracoesUsuario), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ConfiguracoesUsuario>> GetEntity(int idEmpresa, string idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ConfiguracoesUsuario), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ConfiguracoesUsuario>> Create([FromBody] ConfiguracoesUsuarioDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ConfiguracoesUsuario), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ConfiguracoesUsuario>> Update(int idEmpresa, string idCadastro, [FromBody] ConfiguracoesUsuarioDto dto)
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

        // Método personalizado ajustado
        [HttpGet("GetConfiguracoesUsuarioPorEmpresa", Name = nameof(GetConfiguracoesUsuarioPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ConfiguracoesUsuario>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ConfiguracoesUsuario>>> GetConfiguracoesUsuarioPorEmpresa(
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
