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
    public class OlderItemController : GenericDtoController<OlderItem, Guid, OlderItemDto>
    {
        public OlderItemController(IRepositoryDto<OlderItem, Guid, OlderItemDto> repo, ILogger<GenericDtoController<OlderItem, Guid, OlderItemDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/OlderItemsPorPedido/{id}", Name = nameof(GetOlderItemsPorPedido))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOlderItemsPorPedido(Guid id)
        {
            IEnumerable<OlderItem>? entities = await ((OlderItemRepository)repo).GetOlderItemsByOlderAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

    }
}
