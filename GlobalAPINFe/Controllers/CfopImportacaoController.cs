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
using X.PagedList.Extensions;
using System.Collections.Generic;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CfopImportacaoController : GenericPagedController<CfopImportacao, int, CfopImportacaoDto>
    {
        public CfopImportacaoController(IQueryRepository<CfopImportacao, int, CfopImportacaoDto> repo, ILogger<GenericPagedController<CfopImportacao, int, CfopImportacaoDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<CfopImportacao>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<CfopImportacao>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CfopImportacao), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CfopImportacao>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CfopImportacao), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<CfopImportacao>> Create([FromBody] CfopImportacaoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CfopImportacao), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CfopImportacao>> Update(int id, [FromBody] CfopImportacaoDto dto)
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

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<CfopImportacao>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<CfopImportacao>>> CreateBulk([FromBody] IEnumerable<CfopImportacaoDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        // Método personalizado ajustado
        [HttpGet("GetCfopImportacaoPorEmpresa", Name = nameof(GetCfopImportacaoPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<CfopImportacao>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<CfopImportacao>>> GetCfopImportacaoPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? cdCfopS = null,
            [FromQuery] string? cdCfopE = null)
        {
            try
            {
                var query = ((CfopImportacaoPagedRepository)repo).GetCfopImportacaoPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(cdCfopS))
                {
                    var _cdCfopS = UtlStrings.RemoveDiacritics(cdCfopS.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdCfopS == null) ? "" : p.CdCfopS.ToLower()).Contains(_cdCfopS));
                }
                if (!string.IsNullOrEmpty(cdCfopE))
                {
                    var _cdCfopE = UtlStrings.RemoveDiacritics(cdCfopE.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdCfopE == null) ? "" : p.CdCfopE.ToLower()).Contains(_cdCfopE));
                }


                filteredQuery = filteredQuery.OrderByDescending(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<CfopImportacao>(pagedList);

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
