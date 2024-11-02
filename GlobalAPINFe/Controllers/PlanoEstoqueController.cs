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
    public class PlanoEstoqueController : GenericPagedController<PlanoEstoque, int, PlanoEstoqueDto>
    {
        public PlanoEstoqueController(IQueryRepository<PlanoEstoque, int, PlanoEstoqueDto> repo, ILogger<GenericPagedController<PlanoEstoque, int, PlanoEstoqueDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PlanoEstoque>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<PlanoEstoque>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlanoEstoque), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoEstoque>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlanoEstoque), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PlanoEstoque>> Create([FromBody] PlanoEstoqueDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<PlanoEstoque>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<PlanoEstoque>>> CreateBulk([FromBody] IEnumerable<PlanoEstoqueDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PlanoEstoque), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoEstoque>> Update(int id, [FromBody] PlanoEstoqueDto dto)
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

        [HttpGet("GetPlanoEstoquePorEmpresa", Name = nameof(GetPlanoEstoquePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<PlanoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<PlanoEstoque>>> GetPlanoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((PlanoEstoquePagedRepository)repo).GetPlanoEstoquePorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();



                filteredQuery = filteredQuery.OrderByDescending(p => p.CdPlano);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<PlanoEstoque>(pagedList);

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
