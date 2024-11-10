using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using System.Collections.Generic;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoCaixaController : GenericPagedController<HistoricoCaixa, int, HistoricoCaixaDto>
    {
        public HistoricoCaixaController(IQueryRepository<HistoricoCaixa, int, HistoricoCaixaDto> repo, ILogger<GenericPagedController<HistoricoCaixa, int, HistoricoCaixaDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<HistoricoCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<HistoricoCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HistoricoCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<HistoricoCaixa>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HistoricoCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<HistoricoCaixa>> Create([FromBody] HistoricoCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<HistoricoCaixa>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<HistoricoCaixa>>> CreateBulk([FromBody] IEnumerable<HistoricoCaixaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HistoricoCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<HistoricoCaixa>> Update(int id, [FromBody] HistoricoCaixaDto dto)
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

        // Métodos personalizados ajustados

        [HttpGet("GetHistoricoCaixaPorEmpresa", Name = nameof(GetHistoricoCaixaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<HistoricoCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<HistoricoCaixa>>> GetHistoricoCaixaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((HistoricoCaixaRepository)repo).GetHistoricoCaixaPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(x => x.Id);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<HistoricoCaixa>(pagedList);

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

        [HttpGet("GetHistoricoCaixaPorEmpresa_ALL", Name = nameof(GetHistoricoCaixaPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<HistoricoCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<HistoricoCaixa>>> GetHistoricoCaixaPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((HistoricoCaixaRepository)repo).GetHistoricoCaixaPorEmpresa(idEmpresa).Result.AsQueryable();
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
