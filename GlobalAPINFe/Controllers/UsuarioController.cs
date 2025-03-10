using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GlobalLib.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetUsuarioByUnity/{unity}", Name = nameof(GetUsuarioByUnity))]
        [ProducesResponseType(typeof(List<Usuario>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Usuario>>> GetUsuarioByUnity(
          [FromRoute]  int unity
            )
        {
            try
            {
                var query = ((UsuarioRepositoryDto)repo).GetUsuariosAsyncPerUnity(unity);

                var filteredQuery = query.OrderBy(p => p.Id);

                var List = await filteredQuery.ToListAsync();

                return Ok(List);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (Usuarios).");
                return StatusCode(500, "An error occurred while retrieving entities (Usuarios). Please try again later.");
            }
        }
    }
}
