using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.Repositories;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioEmpresaController : GenericDtoController<UsuarioEmpresa, int, UsuarioEmpresaDto>
    {
        private readonly GlobalErpFiscalBaseContext context;
        public UsuarioEmpresaController(IRepositoryDto<UsuarioEmpresa, int, UsuarioEmpresaDto> repo, ILogger<GenericDtoController<UsuarioEmpresa, int, UsuarioEmpresaDto>> logger, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            context = _context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioEmpresa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<UsuarioEmpresa>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioEmpresa>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioEmpresa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<UsuarioEmpresa>> Create([FromBody] UsuarioEmpresaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UsuarioEmpresa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioEmpresa>> Update(int id, [FromBody] UsuarioEmpresaDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpGet("GetUsuarioEmpresaByUsername/{Username}")]
        [ProducesResponseType(typeof(List<UsuarioEmpresa>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public  async Task<ActionResult<List<UsuarioEmpresa>>> GetUsuarioEmpresaByUsername([FromRoute] string Username)
        {
            try
            {
                List<UsuarioEmpresa> all = await ((UsuarioEmpresaRepository)repo).GetUsuarioEmpresaByUsername(Username);

                return Ok(all);


            }
            catch (Exception ex)
            {
                logger.LogError("Erro ao obter usuario_empresa por usuario", ex);
                return NotFound(new { errorMessage = "Erro ao obter UsuarioEmpresa por nmUsuario." });
            }
        }

        [HttpDelete("DeleteUsuarioEmpresaById/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteUsuarioEmpresaById([FromRoute] int Id)
        {
            try
            {
                UsuarioEmpresa? usuarioEmpresa = await context.UsuarioEmpresas.Where(u => u.Id == Id).FirstOrDefaultAsync();
                if (usuarioEmpresa == null)
                {
                    throw new Exception("UsuarioEmpresa não encontrado");
                }
                context.UsuarioEmpresas.Remove(usuarioEmpresa);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("Erro ao deletar usuario_empresa por id", ex);
                return NotFound(new { errorMessage = "Erro ao deletar UsuarioEmpresa por id." });
            }

        }
    }
}
