using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroCaixaController : GenericPagedController<LivroCaixa, long, LivroCaixaDto>
    {
        public LivroCaixaController(IQueryRepository<LivroCaixa, long, LivroCaixaDto> repo, ILogger<GenericPagedController<LivroCaixa, long, LivroCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<LivroCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LivroCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<LivroCaixa>> GetEntity(long id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LivroCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<LivroCaixa>> Create([FromBody] LivroCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<LivroCaixa>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<LivroCaixa>>> CreateBulk([FromBody] IEnumerable<LivroCaixaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LivroCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<LivroCaixa>> Update(long id, [FromBody] LivroCaixaDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }

        // Métodos personalizados ajustados

        [HttpGet("GetLivroCaixaPorEmpresa", Name = nameof(GetLivroCaixaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<LivroCaixa>>> GetLivroCaixaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((LivroCaixaRepository)repo).GetLivroCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<LivroCaixa>(pagedList);

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

        [HttpGet("GetLivroCaixaPorEmpresa_ALL", Name = nameof(GetLivroCaixaPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<LivroCaixa>>> GetLivroCaixaPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((LivroCaixaRepository)repo).GetLivroCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.AsNoTracking().ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(list); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
