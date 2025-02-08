using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CteController : GenericPagedController<Cte, int, CteDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly ILogger<CteController> _log;
        public CteController(IQueryRepository<Cte, int, CteDto> repo, ILogger<GenericPagedController<Cte, int, CteDto>> logger, GlobalErpFiscalBaseContext _context, ILogger<CteController> _log) : base(repo, logger)
        {
            this._context = _context;
            this._log = _log;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Cte>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Cte>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cte), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cte>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cte), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Cte>> Create([FromBody] CteDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Cte>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Cte>>> CreateBulk([FromBody] IEnumerable<CteDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cte), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cte>> Update(int id, [FromBody] CteDto dto)
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
