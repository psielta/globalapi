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
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenciaEstoqueController : GenericPagedController<ReferenciaEstoque, int, ReferenciaEstoqueDto>
    {
        public ReferenciaEstoqueController(IQueryRepository<ReferenciaEstoque, int, ReferenciaEstoqueDto> repo, ILogger<GenericPagedController<ReferenciaEstoque, int, ReferenciaEstoqueDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ReferenciaEstoque>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ReferenciaEstoque>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReferenciaEstoque), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ReferenciaEstoque>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReferenciaEstoque), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ReferenciaEstoque>> Create([FromBody] ReferenciaEstoqueDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReferenciaEstoque), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ReferenciaEstoque>> Update(int id, [FromBody] ReferenciaEstoqueDto dto)
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

        [HttpGet("GetReferenciaEstoquePorEmpresa", Name = nameof(GetReferenciaEstoquePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ReferenciaEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ReferenciaEstoque>>> GetReferenciaEstoquePorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ReferenciaEstoquePagedRepository)repo).GetReferenciaEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ReferenciaEstoque>(pagedList);

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

        [HttpGet("GetAllReferenciaEstoquePorEmpresa/{idEmpresa}", Name = nameof(GetAllReferenciaEstoquePorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<ReferenciaEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReferenciaEstoque>>> GetAllReferenciaEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((ReferenciaEstoquePagedRepository)repo).GetReferenciaEstoqueAsyncPorEmpresa(idEmpresa);

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

        [HttpGet("GetReferenciaEstoqueByName/{nome}/{idEmpresa}")]
        [ProducesResponseType(typeof(IEnumerable<ReferenciaEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReferenciaEstoque>>> GetReferenciaEstoqueByName(string nome, int idEmpresa)
        {
            var referencias = await (repo as ReferenciaEstoquePagedRepository).RetrieveAllAsync();
            if (referencias == null)
            {
                return NotFound("Referência não encontrada.");
            }
            var referenciasFilteredByEmpresa = referencias.AsEnumerable().Where(u => u.CdEmpresa == idEmpresa);

            if (referenciasFilteredByEmpresa == null)
            {
                return NotFound("Referências não encontradas para empresa especificada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = referenciasFilteredByEmpresa.AsEnumerable().Where(c => UtlStrings.RemoveDiacritics(c.NmRef.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmRef)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Cidade não encontrada.");
            }
            return Ok(filter);
        }
    }
}
