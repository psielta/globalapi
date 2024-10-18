using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : GenericDtoController<Section, int, SectionDto>
    {
        public SectionController(IRepositoryDto<Section, int, SectionDto> repo, ILogger<GenericDtoController<Section, int, SectionDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Section>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Section>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Section), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Section>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Section), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Section>> Create([FromBody] SectionDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Section), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Section>> Update(int id, [FromBody] SectionDto dto)
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

        [HttpGet("/api/SectionsPorEmpresa/{id}", Name = nameof(GetSectionsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<Section>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Section>>> GetSectionsPorEmpresa(int id)
        {
            IEnumerable<Section>? entities = await ((SectionRepository)repo).GetSectionsByEmpresaAsync(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
