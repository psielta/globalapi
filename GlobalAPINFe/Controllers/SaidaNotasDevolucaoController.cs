using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    public class SaidaNotasDevolucaoController : GenericPagedController<SaidaNotasDevolucao, int, SaidaNotasDevolucaoDto>
    {
        public SaidaNotasDevolucaoController(IQueryRepository<SaidaNotasDevolucao, int, SaidaNotasDevolucaoDto> repo, ILogger<GenericPagedController<SaidaNotasDevolucao, int, SaidaNotasDevolucaoDto>> logger) : base(repo, logger)
        {
        }
        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<SaidaNotasDevolucao>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<SaidaNotasDevolucao>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaidaNotasDevolucao), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaidaNotasDevolucao>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaidaNotasDevolucao), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<SaidaNotasDevolucao>> Create([FromBody] SaidaNotasDevolucaoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaidaNotasDevolucao), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaidaNotasDevolucao>> Update(int id, [FromBody] SaidaNotasDevolucaoDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<SaidaNotasDevolucao>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<SaidaNotasDevolucao>>> CreateBulk([FromBody] IEnumerable<SaidaNotasDevolucaoDto> dtos)
        {
            return await base.CreateBulk(dtos);
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

        [HttpGet("GetSaidaNotasDevolucaoPorSaida", Name = nameof(GetSaidaNotasDevolucaoPorSaida))]
        [ProducesResponseType(typeof(PagedResponse<SaidaNotasDevolucao>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<SaidaNotasDevolucao>>> GetSaidaNotasDevolucaoPorSaida(int nrSaida, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((SaidaNotasDevolucaoRepository)repo).GetSaidaNotasDevolucaoPorSaida(nrSaida);
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(c => c.Id);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<SaidaNotasDevolucao>(pagedList);

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

        [HttpGet("GetAllSaidaNotasDevolucaoPorSaida/{nrSaida}", Name = nameof(GetAllSaidaNotasDevolucaoPorSaida))]
        [ProducesResponseType(typeof(IEnumerable<SaidaNotasDevolucao>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SaidaNotasDevolucao>>> GetAllSaidaNotasDevolucaoPorSaida(int nrSaida)
        {
            try
            {
                var entitysFilterByEmpresa = await ((SaidaNotasDevolucaoRepository)repo).GetSaidaNotasDevolucaoPorSaida(nrSaida);

                if (entitysFilterByEmpresa == null)
                {
                    return NotFound("Entities not found.");
                }
                if (entitysFilterByEmpresa.Count() == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(entitysFilterByEmpresa);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
