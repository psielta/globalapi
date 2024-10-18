using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : GenericDtoController<Usuario, string, UsuarioDto>
    {
        public UsuarioController(IRepositoryDto<Usuario, string, UsuarioDto> repo, ILogger<GenericDtoController<Usuario, string, UsuarioDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Usuario>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Usuario>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Usuario>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Usuario>> Create([FromBody] UsuarioDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Usuario>> Update(string id, [FromBody] UsuarioDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }

        // Método personalizado ajustado

        [HttpGet("/api/UsuariosPorEmpresa/{id}", Name = nameof(GetUsuariosPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<Usuario>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPorEmpresa(int id)
        {
            IEnumerable<Usuario>? entities = await ((UsuarioRepositoryDto)repo).GetUsuariosAsyncPerEmpresa(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
