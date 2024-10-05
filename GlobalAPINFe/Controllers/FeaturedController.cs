using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class FeaturedController : GenericDtoController<Featured, int, FeaturedDto>
    {
        public FeaturedController(IRepositoryDto<Featured, int, FeaturedDto> repo, ILogger<GenericDtoController<Featured, int, FeaturedDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("/api/FeaturedsPorEmpresa/{id}", Name = nameof(GetFeaturedsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFeaturedsPorEmpresa(int id)
        {
            IEnumerable<Featured>? entities = await ((FeaturedRepository)repo).GetFeaturedByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

    }
}
