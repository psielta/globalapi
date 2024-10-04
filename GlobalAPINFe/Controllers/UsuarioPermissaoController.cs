using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class UsuarioPermissaoController : GenericDtoController<UsuarioPermissao, int, UsuarioPermissaoDto>
    {
        private readonly IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario;
        private readonly IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao;
        public UsuarioPermissaoController(IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repo, ILogger<GenericDtoController<UsuarioPermissao, int, UsuarioPermissaoDto>> logger,
            IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao,
            IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario
            ) : base(repo, logger)
        {
            this.repositoryPermissao = repositoryPermissao;
            this.repositoryUsuario = repositoryUsuario;
        }
        [HttpGet("/api/UsuarioPermissaoPorNome/{User}/{Modulo}", Name = nameof(GetPermissoesByUser))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissoesByUser(string User, string Modulo)
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
