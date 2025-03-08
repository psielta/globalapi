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
    public class UnityController : GenericDtoController<Unity, int, UnityDto>
    {
        public UnityController(IRepositoryDto<Unity, int, UnityDto> repo, ILogger<GenericDtoController<Unity, int, UnityDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Unity>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Unity>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Unity), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Unity>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Unity), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Unity>> Create([FromBody] UnityDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Unity), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Unity>> Update(int id, [FromBody] UnityDto dto)
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
