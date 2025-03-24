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

        [HttpPost("BeforePost/{editing}")]
        [ProducesResponseType(typeof(UserValidationDto), 200)]
        [ProducesResponseType(typeof(UserValidationDto), 400)]
        [ProducesResponseType(typeof(UserValidationDto), 404)]
        public async Task<ActionResult<UserValidationDto>> BeforePost(
            [FromRoute] int editing,
            [FromBody] UsuarioDto dto,
            [FromQuery] int? IdUsuario = null)
        {
            IEnumerable<Usuario>? allUsuario = await repo.RetrieveAllAsync();
            if (allUsuario == null)
            {
                return NotFound(new UserValidationDto() { Status = 404, Message = "All Users not found." });
            }
            if (editing == 1)
            {
                Usuario? oldUsuario = await this.repo.RetrieveAsyncAsNoTracking(dto.NmUsuario);
                if (oldUsuario == null)
                {
                    return NotFound(new UserValidationDto() { Status = 404, Message = "Old User not found." });
                }
                if (!oldUsuario.Email.Equals(dto.Email))
                {
                    // verifica se outro usuario possui dto.Email
                    Usuario? usuario = allUsuario.FirstOrDefault(p => p.Email.Equals(dto.Email));
                    if (usuario != null)
                    {
                        return BadRequest(new UserValidationDto() { Status = 400, Message = "Email ja utilizado." });
                    }
                }
                if (!oldUsuario.NmUsuario.Equals(dto.NmUsuario))
                {
                    // verifica se outro usuario possui dto.NmUsuario
                    Usuario? usuario = allUsuario.FirstOrDefault(p => p.NmUsuario.Equals(dto.NmUsuario));
                    if (usuario != null)
                    {
                        return BadRequest(new UserValidationDto() { Status = 400, Message = "Identificador de usuario ja utilizado." });
                    }
                }
                return Ok(new UserValidationDto() { Status = 200, Message = "OK" });
            }
            else // creating
            {
                //verifica se outro usuario possui dto.Email
                Usuario? usuario = allUsuario.FirstOrDefault(p => p.Email.Equals(dto.Email));
                if (usuario != null)
                {
                    return BadRequest(new UserValidationDto() { Status = 400, Message = "Email ja utilizado." });
                }
                //verifica se outro usuario possui dto.NmUsuario
                usuario = allUsuario.FirstOrDefault(p => p.NmUsuario.Equals(dto.NmUsuario));
                if (usuario != null)
                {
                    return BadRequest(new UserValidationDto() { Status = 400, Message = "Identificador de usuario ja utilizado." });
                }
                return Ok(new UserValidationDto() { Status = 200, Message = "OK" });
            }
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
          [FromRoute] int unity
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
