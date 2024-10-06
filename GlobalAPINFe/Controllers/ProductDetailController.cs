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
    public class ProductDetailController : GenericDtoController<ProductDetail, int, ProductDetailDto>
    {
        public ProductDetailController(IRepositoryDto<ProductDetail, int, ProductDetailDto> repo, ILogger<GenericDtoController<ProductDetail, int, ProductDetailDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/ProductDetailsPorEmpresa/{id}", Name = nameof(GetProductDetailsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductDetailsPorEmpresa(int id)
        {
            IEnumerable<ProductDetail>? entities = await ((ProductDetailRepository)repo).GetProductDetailsAsyncPerEmpresa(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
