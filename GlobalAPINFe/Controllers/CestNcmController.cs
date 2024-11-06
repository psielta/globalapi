using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CestNcmController : GenericDtoController<CestNcm, int, CestNcmDto>
    {
        public CestNcmController(IRepositoryDto<CestNcm, int, CestNcmDto> repo, ILogger<GenericDtoController<CestNcm, int, CestNcmDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CestNcm>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<CestNcm>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CestNcm), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CestNcm>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CestNcm), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<CestNcm>> Create([FromBody] CestNcmDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CestNcm), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CestNcm>> Update(int id, [FromBody] CestNcmDto dto)
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
