using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetailController : GenericDtoController<ItemDetail, int, ItemDetailDto>
    {
        public ItemDetailController(IRepositoryDto<ItemDetail, int, ItemDetailDto> repo, ILogger<GenericDtoController<ItemDetail, int, ItemDetailDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemDetail>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<ItemDetail>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ItemDetail), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ItemDetail>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemDetail), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ItemDetail>> Create([FromBody] ItemDetailDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ItemDetail), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ItemDetail>> Update(int id, [FromBody] ItemDetailDto dto)
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
        [HttpGet("/api/ItemDetailsPorEmpresa/{id}", Name = nameof(GetItemDetailsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<ItemDetail>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ItemDetail>>> GetItemDetailsPorEmpresa(int id)
        {
            IEnumerable<ItemDetail>? entities = await ((ItemDetailRepository)repo).GetItemDetailsAsyncPerEmpresa(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
