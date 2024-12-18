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
    public class DistribuicaoDfeController : GenericPagedController<DistribuicaoDfe, Guid, DistribuicaoDfeDto>
    {
        public DistribuicaoDfeController(IQueryRepository<DistribuicaoDfe, Guid, DistribuicaoDfeDto> repo, ILogger<GenericPagedController<DistribuicaoDfe, Guid, DistribuicaoDfeDto>> logger) : base(repo, logger)
        {
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<DistribuicaoDfe>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<DistribuicaoDfe>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DistribuicaoDfe), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<DistribuicaoDfe>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DistribuicaoDfe), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<DistribuicaoDfe>> Create([FromBody] DistribuicaoDfeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<DistribuicaoDfe>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<DistribuicaoDfe>>> CreateBulk([FromBody] IEnumerable<DistribuicaoDfeDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DistribuicaoDfe), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<DistribuicaoDfe>> Update(Guid id, [FromBody] DistribuicaoDfeDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
