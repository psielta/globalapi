using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalLib.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NcmController : GenericDtoController<Ncm, int, NcmDto>
    {
        public NcmController(IRepositoryDto<Ncm, int, NcmDto> repo, ILogger<GenericDtoController<Ncm, int, NcmDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ncm>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Ncm>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Ncm), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Ncm>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Ncm), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Ncm>> Create([FromBody] NcmDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Ncm), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Ncm>> Update(int id, [FromBody] NcmDto dto)
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
