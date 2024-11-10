using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GlobalLib.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionItemController : GenericDtoController<SectionItem, int, SectionItemDto>
    {
        public SectionItemController(IRepositoryDto<SectionItem, int, SectionItemDto> repo, ILogger<GenericDtoController<SectionItem, int, SectionItemDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SectionItem>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<SectionItem>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SectionItem), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SectionItem>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SectionItem), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<SectionItem>> Create([FromBody] SectionItemDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SectionItem), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SectionItem>> Update(int id, [FromBody] SectionItemDto dto)
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

        // Método personalizado ajustado

        [HttpGet("/api/SectionItemsPorEmpresa/{id}", Name = nameof(GetSectionItemsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<SectionItem>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SectionItem>>> GetSectionItemsPorEmpresa(int id)
        {
            IEnumerable<SectionItem>? entities = await ((SectionItemRepository)repo).GetSectionItemsByEmpresaAsync(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
