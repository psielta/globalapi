using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrigemCstController : GenericDtoController<OrigemCst, string, OrigemCstDto>
    {
        public OrigemCstController(IRepositoryDto<OrigemCst, string, OrigemCstDto> repo, ILogger<GenericDtoController<OrigemCst, string, OrigemCstDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrigemCst>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<OrigemCst>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrigemCst), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrigemCst>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrigemCst), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OrigemCst>> Create([FromBody] OrigemCstDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrigemCst), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrigemCst>> Update(string id, [FromBody] OrigemCstDto dto)
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
