using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioPermissaoController : GenericDtoController<UsuarioPermissao, int, UsuarioPermissaoDto>
    {
        private readonly IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario;
        private readonly IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao;

        public UsuarioPermissaoController(
            IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repo,
            ILogger<GenericDtoController<UsuarioPermissao, int, UsuarioPermissaoDto>> logger,
            IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao,
            IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario
        ) : base(repo, logger)
        {
            this.repositoryPermissao = repositoryPermissao;
            this.repositoryUsuario = repositoryUsuario;
        }

        // Overriding inherited methods and adding [ProducesResponseType] attributes

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioPermissao>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<UsuarioPermissao>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioPermissao), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioPermissao>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioPermissao), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<UsuarioPermissao>> Create([FromBody] UsuarioPermissaoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UsuarioPermissao), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UsuarioPermissao>> Update(int id, [FromBody] UsuarioPermissaoDto dto)
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

        // Adjusted custom method

        [HttpGet("/api/UsuarioPermissaoPorNome/{User}/{Modulo}", Name = nameof(GetPermissoesByUser))]
        [ProducesResponseType(typeof(IEnumerable<UsuarioPermissaoGetDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<UsuarioPermissaoGetDto>>> GetPermissoesByUser(string User, string Modulo)
        {
            try
            {
                var allUsuario = await repositoryUsuario.RetrieveAllAsync();
                var _User = allUsuario.FirstOrDefault(u => u.NmUsuario.Equals(User));
                if (_User == null)
                {
                    return NotFound("Usuário não encontrado.");
                }
                var allUsuarioPermissao = await repo.RetrieveAllAsync();
                var allPermissao = await repositoryPermissao.RetrieveAllAsync();
                var filterPermissaoModulo = allPermissao.Where(p => p.Modulo.Equals(Modulo));
                if (filterPermissaoModulo.Count() == 0)
                {
                    return NotFound("Módulo não encontrado.");
                }

                var filterUsuarioPermissao = allUsuarioPermissao.Where(p => p.IdUsuario.Equals(User));

                IEnumerable<UsuarioPermissaoGetDto> filterDto =
                    filterPermissaoModulo.Select(p =>
                    {
                        var _UsuarioPermissao = filterUsuarioPermissao.FirstOrDefault(up => up.IdPermissao.Equals(p.Id));
                        if (_UsuarioPermissao == null)
                        {
                            return new UsuarioPermissaoGetDto
                            {
                                IdPermissao = p.Id,
                                Descricao = (p.Descricao == null) ? "" : p.Descricao,
                                Possui = false,
                                IdUsuarioPermissao = 0
                            };
                        }
                        return new UsuarioPermissaoGetDto
                        {
                            IdPermissao = p.Id,
                            Descricao = (p.Descricao == null) ? "" : p.Descricao,
                            Possui = filterUsuarioPermissao.Any(up => up.IdPermissao.Equals(p.Id)),
                            IdUsuarioPermissao = _UsuarioPermissao.Id
                        };
                    });

                return Ok(filterDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao recuperar as permissões.");
                return StatusCode(500, "Ocorreu um erro ao recuperar as permissões. Por favor, tente novamente mais tarde.");

            }

        }
    }
}
