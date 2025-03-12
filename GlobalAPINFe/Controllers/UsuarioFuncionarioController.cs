using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioFuncionarioController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<UsuarioFuncionario, int, string, UsuarioFuncionarioDto>
    {
        public UsuarioFuncionarioController(IQueryRepositoryMultiKey<UsuarioFuncionario, int, string, UsuarioFuncionarioDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<UsuarioFuncionario, int, string, UsuarioFuncionarioDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UsuarioFuncionario>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<UsuarioFuncionario>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(UsuarioFuncionario), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioFuncionario>> GetEntity(int idEmpresa, string idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioFuncionario), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<UsuarioFuncionario>> Create([FromBody] UsuarioFuncionarioDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(UsuarioFuncionario), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioFuncionario>> Update(int idEmpresa, string idCadastro, [FromBody] UsuarioFuncionarioDto dto)
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

        [HttpGet("GetUsuarioFuncionarioByUsuario/{nmUsuario}", Name = nameof(GetUsuarioFuncionarioByUsuario))]
        [ProducesResponseType(typeof(UsuarioFuncionario), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuarioFuncionario>> GetUsuarioFuncionarioByUsuario(
            string nmUsuario)
        {
            try
            {
                UsuarioFuncionario? usuarioFuncionario = await ((UsuarioFuncionarioRepository)repo).GetUsuarioFuncionarioByUsuario(nmUsuario);

                if (usuarioFuncionario == null)
                {
                    return NotFound($"UsuarioFuncionario with NmUsuario {nmUsuario} not found.");
                }

                return Ok(usuarioFuncionario);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (UsuarioFuncionario).");
                return StatusCode(500, "An error occurred while retrieving entities (UsuarioFuncionario). Please try again later.");
            }
        }
    }
}
