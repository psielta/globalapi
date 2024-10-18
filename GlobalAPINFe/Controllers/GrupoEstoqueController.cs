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
using System.Collections.Generic;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoEstoqueController : GenericPagedController<GrupoEstoque, int, GrupoEstoqueDto>
    {
        public GrupoEstoqueController(IQueryRepository<GrupoEstoque, int, GrupoEstoqueDto> repo, ILogger<GenericPagedController<GrupoEstoque, int, GrupoEstoqueDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<GrupoEstoque>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<GrupoEstoque>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GrupoEstoque), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<GrupoEstoque>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GrupoEstoque), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<GrupoEstoque>> Create([FromBody] GrupoEstoqueDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<GrupoEstoque>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<GrupoEstoque>>> CreateBulk([FromBody] IEnumerable<GrupoEstoqueDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GrupoEstoque), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<GrupoEstoque>> Update(int id, [FromBody] GrupoEstoqueDto dto)
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

        [HttpGet("GetAllGrupoEstoquePorEmpresa/{idEmpresa}", Name = nameof(GetAllGrupoEstoquePorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<GrupoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<GrupoEstoque>>> GetAllGrupoEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((GrupoEstoquePagedRepository)repo).GetGrupoEstoqueAsyncPorEmpresa(idEmpresa);

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

        [HttpGet("GetGrupoEstoquePorEmpresa", Name = nameof(GetGrupoEstoquePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<GrupoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<GrupoEstoque>>> GetGrupoEstoquePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((GrupoEstoquePagedRepository)repo).GetGrupoEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<GrupoEstoque>(pagedList);

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

        [HttpGet("GetGrupoEstoqueByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(typeof(IEnumerable<GrupoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<GrupoEstoque>>> GetGrupoEstoqueByName(string nome, int idEmpresa)
        {
            var grupos = await (repo as GrupoEstoquePagedRepository).RetrieveAllAsync();
            if (grupos == null)
            {
                return NotFound("Grupo não encontrado.");
            }
            var gruposFilteredByEmpresa = grupos.AsEnumerable().Where(u => u.CdEmpresa == idEmpresa);

            if (gruposFilteredByEmpresa == null)
            {
                return NotFound("Grupos não encontrados para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = gruposFilteredByEmpresa.AsEnumerable().Where(c => UtlStrings.RemoveDiacritics(c.NmGrupo.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmGrupo)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Grupo não encontrados.");
            }
            return Ok(filter);
        }
    }
}
