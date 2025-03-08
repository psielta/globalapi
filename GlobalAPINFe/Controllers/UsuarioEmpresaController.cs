using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioEmpresaController : GenericDtoController<UsuarioEmpresa, int, UsuarioEmpresaDto>
    {
        public UsuarioEmpresaController(IRepositoryDto<UsuarioEmpresa, int, UsuarioEmpresaDto> repo, ILogger<GenericDtoController<UsuarioEmpresa, int, UsuarioEmpresaDto>> logger) : base(repo, logger)
        {
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
    }
}
