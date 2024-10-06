using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilLojaController : GenericDtoController<PerfilLoja, int, PerfilLojaDto>
    {
        public PerfilLojaController(IRepositoryDto<PerfilLoja, int, PerfilLojaDto> repo, ILogger<GenericDtoController<PerfilLoja, int, PerfilLojaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/PerfilLojasPorEmpresa/{id}", Name = nameof(GetPerfilLojasPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPerfilLojasPorEmpresa(int id)
        {
            IEnumerable<PerfilLoja>? entities = await ((PerfilLojaRepository)repo).GetPerfilLojasByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

    }
}
