using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : GenericDtoController<Category, int, CategoryDto>
    {
        public CategoryController(IRepositoryDto<Category, int, CategoryDto> repo, ILogger<GenericDtoController<Category, int, CategoryDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/CategorysPorEmpresa/{id}", Name = nameof(GetCategorysPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get category by ID for NSwag", Description = "Retrieves a category by its ID for NSwag.")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategorysPorEmpresa(int id)
        {
            IEnumerable<Category>? entities = await ((CategoryRep)repo).GetCategorysByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

        // Override the GetEntities method to provide Swagger documentation
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves all categories.")]
        public override async Task<ActionResult<IEnumerable<Category>>> GetEntities()
        {
            return await base.GetEntities();
        }

        // Override the GetEntity method to provide Swagger documentation
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Get a category by ID", Description = "Retrieves a category by its ID.")]
        public override async Task<ActionResult<Category>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        // Override the Create method to provide Swagger documentation
        [HttpPost]
        [ProducesResponseType(typeof(Category), 201)]
        [ProducesResponseType(400)]
        [SwaggerOperation(Summary = "Create a new category", Description = "Creates a new category.")]
        public override async Task<ActionResult<Category>> Create([FromBody] CategoryDto dto)
        {
            return await base.Create(dto);
        }

        // Override the Update method to provide Swagger documentation
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Update a category", Description = "Updates a category.")]
        public override async Task<ActionResult<Category>> Update(int id, [FromBody] CategoryDto dto)
        {
            return await base.Update(id, dto);
        }

        // Override the Delete method to provide Swagger documentation
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Delete a category", Description = "Deletes a category.")]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
