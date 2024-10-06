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
    public class ItemDetailController : GenericDtoController<ItemDetail, int, ItemDetailDto>
    {
        public ItemDetailController(IRepositoryDto<ItemDetail, int, ItemDetailDto> repo, ILogger<GenericDtoController<ItemDetail, int, ItemDetailDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("/api/ItemDetailsPorEmpresa/{id}", Name = nameof(GetItemDetailsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetItemDetailsPorEmpresa(int id)
        {
            IEnumerable<ItemDetail>? entities = await ((ItemDetailRepository)repo).GetItemDetailsAsyncPerEmpresa(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
