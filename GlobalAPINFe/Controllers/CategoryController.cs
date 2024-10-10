using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CategoryController : GenericDtoController<Category, int, CategoryDto>
    {
        public CategoryController(IRepositoryDto<Category, int, CategoryDto> repo, ILogger<GenericDtoController<Category, int, CategoryDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/CategorysPorEmpresa/{id}", Name = nameof(GetCategorysPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategorysPorEmpresa(int id)
        {
            IEnumerable<Category>? entities = await ((CategoryRep)repo).GetCategorysByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
