using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class IbptController : GenericDtoController<Ibpt, int, IbptDto>
    {
        public IbptController(IRepositoryDto<Ibpt, int, IbptDto> repo, ILogger<GenericDtoController<Ibpt, int, IbptDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ibpt>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Ibpt>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ibpt), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Ibpt>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Ibpt), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Ibpt>> Create([FromBody] IbptDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Ibpt), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Ibpt>> Update(int id, [FromBody] IbptDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
