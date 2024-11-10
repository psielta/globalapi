using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpdupnfeController : GenericDtoController<Impdupnfe, string, ImpdupnfeDto>
    {
        public ImpdupnfeController(IRepositoryDto<Impdupnfe, string, ImpdupnfeDto> repo, ILogger<GenericDtoController<Impdupnfe, string, ImpdupnfeDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Impdupnfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Impdupnfe>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Impdupnfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impdupnfe>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Impdupnfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Impdupnfe>> Create([FromBody] ImpdupnfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Impdupnfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Impdupnfe>> Update(string id, [FromBody] ImpdupnfeDto dto)
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
