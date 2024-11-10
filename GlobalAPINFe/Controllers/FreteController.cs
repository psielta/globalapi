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
    public class FreteController : GenericPagedController<Frete, int, FreteDto>
    {
        public FreteController(IQueryRepository<Frete, int, FreteDto> repo, ILogger<GenericPagedController<Frete, int, FreteDto>> logger) : base(repo, logger)
        {
        }
        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Frete>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Frete>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Frete), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Frete>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Frete), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Frete>> Create([FromBody] FreteDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Frete), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Frete>> Update(int id, [FromBody] FreteDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Frete>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Frete>>> CreateBulk([FromBody] IEnumerable<FreteDto> dtos)
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

        [HttpGet("GetFretePorSaida", Name = nameof(GetFretePorSaida))]
        [ProducesResponseType(typeof(PagedResponse<Frete>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Frete>>> GetFretePorSaida(int nrSaida, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((FreteRepository)repo).GetFretePorSaida(nrSaida);
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(c => c.NrLanc);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<Frete>(pagedList);

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

        [HttpGet("GetAllFretePorSaida/{nrSaida}", Name = nameof(GetAllFretePorSaida))]
        [ProducesResponseType(typeof(IEnumerable<Frete>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Frete>>> GetAllFretePorSaida(int nrSaida)
        {
            try
            {
                var entitysFilterByEmpresa = await ((FreteRepository)repo).GetFretePorSaida(nrSaida);

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
