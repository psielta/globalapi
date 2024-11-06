using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CsosnController : GenericDtoController<Csosn, string, CsosnDto>
    {
        public CsosnController(IRepositoryDto<Csosn, string, CsosnDto> repo, ILogger<GenericDtoController<Csosn, string, CsosnDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Csosn>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Csosn>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Csosn), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Csosn>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Csosn), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Csosn>> Create([FromBody] CsosnDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Csosn), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Csosn>> Update(string id, [FromBody] CsosnDto dto)
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
