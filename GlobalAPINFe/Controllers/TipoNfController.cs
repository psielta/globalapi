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
    public class TipoNfController : GenericDtoController<TipoNf, string, TipoNfDto>
    {
        public TipoNfController(IRepositoryDto<TipoNf, string, TipoNfDto> repo, ILogger<GenericDtoController<TipoNf, string, TipoNfDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoNf>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<TipoNf>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TipoNf), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<TipoNf>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TipoNf), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<TipoNf>> Create([FromBody] TipoNfDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TipoNf), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<TipoNf>> Update(string id, [FromBody] TipoNfDto dto)
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
