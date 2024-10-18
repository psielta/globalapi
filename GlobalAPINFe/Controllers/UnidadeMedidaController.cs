using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadeMedidaController : GenericPagedController<UnidadeMedida, int, UnidadeMedidaDto>
    {
        public UnidadeMedidaController(IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto> repo, ILogger<GenericPagedController<UnidadeMedida, int, UnidadeMedidaDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<UnidadeMedida>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<UnidadeMedida>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UnidadeMedida), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UnidadeMedida>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UnidadeMedida), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<UnidadeMedida>> Create([FromBody] UnidadeMedidaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UnidadeMedida), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<UnidadeMedida>> Update(int id, [FromBody] UnidadeMedidaDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeMedida>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<UnidadeMedida>>> CreateBulk([FromBody] IEnumerable<UnidadeMedidaDto> dtos)
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

        [HttpGet("GetUnidadeMedidaPorEmpresa", Name = nameof(GetUnidadeMedidaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<UnidadeMedida>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<UnidadeMedida>>> GetUnidadeMedidaPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((UnidadeMedidaPagedRepository)repo).GetUnidadeMedidaPorEmpresa(idEmpresa);
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<UnidadeMedida>(pagedList);

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

        [HttpGet("GetAllUnidadeMedidaPorEmpresa/{idEmpresa}", Name = nameof(GetAllUnidadeMedidaPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<UnidadeMedida>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UnidadeMedida>>> GetAllUnidadeMedidaPorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((UnidadeMedidaPagedRepository)repo).GetUnidadeMedidaPorEmpresa(idEmpresa);

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

        [HttpGet("GetUnidadeByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeMedida>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UnidadeMedida>>> GetUnidadeByName(string nome, int idEmpresa)
        {
            var unidades = await repo.RetrieveAllAsync();
            if (unidades == null)
            {
                return NotFound("Unidade não encontrada.");
            }
            var unidadesFilteredByEmpresa = unidades.Where(u => u.IdEmpresa == idEmpresa);

            if (unidadesFilteredByEmpresa == null)
            {
                return NotFound("Unidade não encontrada para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = unidadesFilteredByEmpresa.AsEnumerable()
                .Where(c => UtlStrings.RemoveDiacritics(c.CdUnidade.ToLower()).StartsWith(stringNormalizada))
                .OrderBy(c => c.CdUnidade.Length)
                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Unidade não encontrada.");
            }

            return Ok(filter);
        }
    }
}
