using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Dto;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlderItemController : GenericDtoController<OlderItem, Guid, OlderItemDto>
    {
        public OlderItemController(IRepositoryDto<OlderItem, Guid, OlderItemDto> repo, ILogger<GenericDtoController<OlderItem, Guid, OlderItemDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OlderItem>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<OlderItem>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OlderItem), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OlderItem>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OlderItem), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OlderItem>> Create([FromBody] OlderItemDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OlderItem), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OlderItem>> Update(Guid id, [FromBody] OlderItemDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }

        // Método personalizado ajustado
        [HttpGet("/api/OlderItemsPorPedido/{id}", Name = nameof(GetOlderItemsPorPedido))]
        [ProducesResponseType(typeof(IEnumerable<OlderItem>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OlderItem>>> GetOlderItemsPorPedido(Guid id)
        {
            IEnumerable<OlderItem>? entities = await ((OlderItemRepository)repo).GetOlderItemsByOlderAsync(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
