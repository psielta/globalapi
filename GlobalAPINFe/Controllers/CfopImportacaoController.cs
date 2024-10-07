using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CfopImportacaoController : GenericPagedController<CfopImportacao, int, CfopImportacaoDto>
    {
        public CfopImportacaoController(IQueryRepository<CfopImportacao, int, CfopImportacaoDto> repo, ILogger<GenericPagedController<CfopImportacao, int, CfopImportacaoDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet("GetCfopImportacaoPorEmpresa", Name = nameof(GetCfopImportacaoPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCfopImportacaoPorEmpresa(
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


                filteredQuery = filteredQuery.OrderBy(p => p.Id);

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
