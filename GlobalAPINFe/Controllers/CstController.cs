using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CstController : GenericDtoController<Cst, string, CstDto>
    {
        public CstController(IRepositoryDto<Cst, string, CstDto> repo, ILogger<GenericDtoController<Cst, string, CstDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cst>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Cst>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cst), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cst>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cst), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Cst>> Create([FromBody] CstDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cst), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cst>> Update(string id, [FromBody] CstDto dto)
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
