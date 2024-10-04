using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class UsuarioController : GenericDtoController<Usuario, string, UsuarioDto>
    {
        public UsuarioController(IRepositoryDto<Usuario, string, UsuarioDto> repo, ILogger<GenericDtoController<Usuario, string, UsuarioDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/UsuariosPorEmpresa/{id}", Name = nameof(GetUsuariosPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUsuariosPorEmpresa(int id)
        {
            IEnumerable<Usuario>? entities = await ((UsuarioRepositoryDto) repo).GetUsuariosAsyncPerEmpresa(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
