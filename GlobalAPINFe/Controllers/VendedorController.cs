using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendedorController : GenericPagedControllerMultiKey<Vendedor, int, int, VendedorDto>
    {
        public VendedorController(IQueryRepositoryMultiKey<Vendedor, int, int, VendedorDto> repo, ILogger<GenericPagedControllerMultiKey<Vendedor, int, int, VendedorDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Vendedor>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Vendedor>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Vendedor), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Vendedor>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Vendedor), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Vendedor>> Create([FromBody] VendedorDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Vendedor), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Vendedor>> Update(int idEmpresa, int idCadastro, [FromBody] VendedorDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, int idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }
    }
}
