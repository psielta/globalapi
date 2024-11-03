using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class SaidaController : GenericPagedController<Saida, int, SaidaDto>
    {
        public SaidaController(IQueryRepository<Saida, int, SaidaDto> repo, ILogger<GenericPagedController<Saida, int, SaidaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Saida>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Saida>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Saida), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Saida>> Create([FromBody] SaidaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> Update(int id, [FromBody] SaidaDto dto)
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

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Saida>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Saida>>> CreateBulk([FromBody] IEnumerable<SaidaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }
    }
}
