using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsTabelaPrecoController : GenericDtoController<OsTabelaPreco, long, OsTabelaPrecoDto>
    {
        public OsTabelaPrecoController(IRepositoryDto<OsTabelaPreco, long, OsTabelaPrecoDto> repo, ILogger<GenericDtoController<OsTabelaPreco, long, OsTabelaPrecoDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OsTabelaPreco), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OsTabelaPreco>> GetEntity(long id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OsTabelaPreco), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OsTabelaPreco>> Create([FromBody] OsTabelaPrecoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OsTabelaPreco), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OsTabelaPreco>> Update(long id, [FromBody] OsTabelaPrecoDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }
    }
}
