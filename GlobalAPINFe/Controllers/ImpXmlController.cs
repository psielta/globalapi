using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class ImpXmlController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Impxml, int, string, ImpxmlDto>
    {
        public ImpXmlController(IQueryRepositoryMultiKey<Impxml, int, string, ImpxmlDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Impxml, int, string, ImpxmlDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Impxml>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Impxml>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Impxml), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impxml>> GetEntity(int idEmpresa, string idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Impxml), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Impxml>> Create([FromBody] ImpxmlDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Impxml), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impxml>> Update(int idEmpresa, string idCadastro, [FromBody] ImpxmlDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, string idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }

        [HttpGet("/api/ImpxmlsPorEmpresa/{id}", Name = nameof(GetImpxmlsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<Impxml>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Impxml>>> GetImpxmlsPorEmpresa(int id)
        {
            IEnumerable<Impxml>? entities = await ((ImpXmlRepository)repo).GetImpxmlByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
