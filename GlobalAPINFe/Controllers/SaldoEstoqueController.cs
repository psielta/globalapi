using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoEstoqueController : GenericPagedController<SaldoEstoque, int, SaldoEstoqueDto>
    {
        public SaldoEstoqueController(IQueryRepository<SaldoEstoque, int, SaldoEstoqueDto> repo, ILogger<GenericPagedController<SaldoEstoque, int, SaldoEstoqueDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<SaldoEstoque>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<SaldoEstoque>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaldoEstoque), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaldoEstoque>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaldoEstoque), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<SaldoEstoque>> Create([FromBody] SaldoEstoqueDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<SaldoEstoque>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<SaldoEstoque>>> CreateBulk([FromBody] IEnumerable<SaldoEstoqueDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaldoEstoque), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<SaldoEstoque>> Update(int id, [FromBody] SaldoEstoqueDto dto)
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

        // Método personalizado ajustado

        [HttpGet("GetSaldoEstoquePorEmpresa", Name = nameof(GetSaldoEstoquePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<SaldoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<SaldoEstoque>>> GetSaldoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cdProduto = null,
            [FromQuery] int? cdPlano = null)
        {
            try
            {
                var query = ((SaldoEstoquePagedRepository)repo).GetSaldoEstoquePorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                if (cdPlano.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdPlano == cdPlano.Value);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<SaldoEstoque>(pagedList);

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
