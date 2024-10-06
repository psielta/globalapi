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
    public class SectionController : GenericDtoController<Section, int, SectionDto>
    {
        public SectionController(IRepositoryDto<Section, int, SectionDto> repo, ILogger<GenericDtoController<Section, int, SectionDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/SectionsPorEmpresa/{id}", Name = nameof(GetSectionsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSectionsPorEmpresa(int id)
        {
            IEnumerable<Section>? entities = await ((SectionRepository)repo).GetSectionsByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

    }
}
