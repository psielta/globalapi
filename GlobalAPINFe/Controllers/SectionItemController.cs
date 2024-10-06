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
    public class SectionItemController : GenericDtoController<SectionItem, int, SectionItemDto>
    {
        public SectionItemController(IRepositoryDto<SectionItem, int, SectionItemDto> repo, ILogger<GenericDtoController<SectionItem, int, SectionItemDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/SectionItemsPorEmpresa/{id}", Name = nameof(GetSectionItemsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSectionItemsPorEmpresa(int id)
        {
            IEnumerable<SectionItem>? entities = await ((SectionItemRepository)repo).GetSectionItemsByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
