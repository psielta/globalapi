using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalLib.Dto;
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
    public class ImpcabnfeController : GenericDtoController<Impcabnfe, string, ImpcabnfeDto>
    {
        public ImpcabnfeController(IRepositoryDto<Impcabnfe, string, ImpcabnfeDto> repo, ILogger<GenericDtoController<Impcabnfe, string, ImpcabnfeDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Impcabnfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Impcabnfe>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Impcabnfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impcabnfe>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Impcabnfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Impcabnfe>> Create([FromBody] ImpcabnfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Impcabnfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impcabnfe>> Update(string id, [FromBody] ImpcabnfeDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }
    }
}
