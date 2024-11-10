using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailController : GenericDtoController<ProductDetail, int, ProductDetailDto>
    {
        public ProductDetailController(IRepositoryDto<ProductDetail, int, ProductDetailDto> repo, ILogger<GenericDtoController<ProductDetail, int, ProductDetailDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<ProductDetail>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDetail), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProductDetail>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDetail), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProductDetail>> Create([FromBody] ProductDetailDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDetail), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProductDetail>> Update(int id, [FromBody] ProductDetailDto dto)
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

        [HttpGet("/api/ProductDetailsPorEmpresa/{id}", Name = nameof(GetProductDetailsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<ProductDetail>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDetail>>> GetProductDetailsPorEmpresa(int id)
        {
            IEnumerable<ProductDetail>? entities = await ((ProductDetailRepository)repo).GetProductDetailsAsyncPerEmpresa(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
