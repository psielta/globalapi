using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoEstoqueController : GenericPagedControllerNoCache<SaldoEstoque, int, SaldoEstoqueDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;

        public SaldoEstoqueController(IQueryRepositoryNoCache<SaldoEstoque, int, SaldoEstoqueDto> repo, ILogger<GenericPagedControllerNoCache<SaldoEstoque, int, SaldoEstoqueDto>> logger
                        , GlobalErpFiscalBaseContext context) : base(repo, logger)
        {
            this._context = context;
        }

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
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cdEmpresa = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] int? cdPlano = null,
            [FromQuery] string? nmProduto = null)
        {
            try
            {
                IQueryable<SaldoEstoque>? query;
                string SQL = $@"SELECT * FROM saldo_estoque se
                        WHERE se.unity = {unity}
                        ";
                if (!string.IsNullOrEmpty(nmProduto))
                {
                    SQL += $@" AND se.cd_produto IN (SELECT pe.cd_produto FROM produto_estoque pe
                        where upper(trim(pe.nm_produto)) LIKE '%{nmProduto.Trim().ToUpper()}%'
                        and pe.unity = {unity}) ";
                }

                query = _context.SaldoEstoques.FromSqlRaw(SQL).Include(c => c.ProdutoEstoque)
                    .Include(c => c.CdPlanoNavigation);

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

                filteredQuery = filteredQuery
                    .OrderByDescending(p => p.Id);

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
