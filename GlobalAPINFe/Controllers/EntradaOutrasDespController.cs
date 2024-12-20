using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaOutrasDespController : GenericPagedController<EntradaOutrasDesp, int, EntradaOutrasDespDto>
    {
        public EntradaOutrasDespController(IQueryRepository<EntradaOutrasDesp, int, EntradaOutrasDespDto> repo, ILogger<GenericPagedController<EntradaOutrasDesp, int, EntradaOutrasDespDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<EntradaOutrasDesp>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<EntradaOutrasDesp>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EntradaOutrasDesp), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<EntradaOutrasDesp>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EntradaOutrasDesp), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<EntradaOutrasDesp>> Create([FromBody] EntradaOutrasDespDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<EntradaOutrasDesp>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<EntradaOutrasDesp>>> CreateBulk([FromBody] IEnumerable<EntradaOutrasDespDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EntradaOutrasDesp), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<EntradaOutrasDesp>> Update(int id, [FromBody] EntradaOutrasDespDto dto)
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

        [HttpGet("GetEntradaOutrasDespPorEmpresa", Name = nameof(GetEntradaOutrasDespPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<EntradaOutrasDesp>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<EntradaOutrasDesp>>> GetEntradaOutrasDespPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((EntradaOutrasDespRepository)repo).GetEntradaOutrasDespAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<EntradaOutrasDesp>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

    }
}
