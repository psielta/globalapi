using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using GlobalLib.Dto;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosFornController : GenericPagedController<ProdutosForn, int, ProdutosFornDto>
    {
        public ProdutosFornController(IQueryRepository<ProdutosForn, int, ProdutosFornDto> repo, ILogger<GenericPagedController<ProdutosForn, int, ProdutosFornDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProdutosForn>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ProdutosForn>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutosForn), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutosForn>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutosForn), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProdutosForn>> Create([FromBody] ProdutosFornDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutosForn), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutosForn>> Update(int id, [FromBody] ProdutosFornDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ProdutosForn>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ProdutosForn>>> CreateBulk([FromBody] IEnumerable<ProdutosFornDto> dtos)
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

        // Método personalizado ajustado

        [HttpGet("GetProdutosFornPorEmpresa", Name = nameof(GetProdutosFornPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ProdutosForn>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ProdutosForn>>> GetProdutosFornPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cdForn = null,
            [FromQuery] int? cdProduto = null)
        {
            try
            {
                var query = ((ProdutosFornRepository)repo).GetProdutosFornAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();


                if (cdForn.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdForn == cdForn.Value);
                }

                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ProdutosForn>(pagedList);

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
