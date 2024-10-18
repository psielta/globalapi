using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpitensnfeController : GenericPagedControllerMultiKey<Impitensnfe, string, string, ImpitensnfeDto>
    {
        public ImpitensnfeController(IQueryRepositoryMultiKey<Impitensnfe, string, string, ImpitensnfeDto> repo, ILogger<GenericPagedControllerMultiKey<Impitensnfe, string, string, ImpitensnfeDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Impitensnfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Impitensnfe>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Impitensnfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impitensnfe>> GetEntity(string idEmpresa, string idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Impitensnfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Impitensnfe>> Create([FromBody] ImpitensnfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Impitensnfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impitensnfe>> Update(string idEmpresa, string idCadastro, [FromBody] ImpitensnfeDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(string idEmpresa, string idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }
    }
}
