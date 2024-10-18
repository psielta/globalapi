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
    public class ImptotalnfeController : GenericDtoController<Imptotalnfe, string, ImptotalnfeDto>
    {
        public ImptotalnfeController(IRepositoryDto<Imptotalnfe, string, ImptotalnfeDto> repo, ILogger<GenericDtoController<Imptotalnfe, string, ImptotalnfeDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Imptotalnfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Imptotalnfe>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Imptotalnfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Imptotalnfe>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Imptotalnfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Imptotalnfe>> Create([FromBody] ImptotalnfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Imptotalnfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Imptotalnfe>> Update(string id, [FromBody] ImptotalnfeDto dto)
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
