using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using X.PagedList.Extensions;
using GlobalLib.Dto;
using GlobalErpData.Services;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasAPagarController : GenericPagedController<ContasAPagar, int, ContasAPagarDto>
    {
        private readonly BaixaCPService _baixaCPService;
        public ContasAPagarController(IQueryRepository<ContasAPagar, int, ContasAPagarDto> repo,
            ILogger<GenericPagedController<ContasAPagar, int, ContasAPagarDto>> logger,
            BaixaCPService baixaCPService) : base(repo, logger)
        {
            _baixaCPService = baixaCPService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ContasAPagar>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ContasAPagar>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContasAPagar), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContasAPagar>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContasAPagar), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ContasAPagar>> Create([FromBody] ContasAPagarDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContasAPagar), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContasAPagar>> Update(int id, [FromBody] ContasAPagarDto dto)
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
        [ProducesResponseType(typeof(IEnumerable<ContasAPagar>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ContasAPagar>>> CreateBulk([FromBody] IEnumerable<ContasAPagarDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }



        public enum TipoPeriodoCAP
        {
            TPC_Geral = 0,
            TPC_Periodo = 1,
            TPC_Ate_Data = 2,
        }

        public enum TipoDataCap
        {
            TDC_Lancamento = 0,
            TDC_Vencimento = 1,
            TDC_Pagamento = 2,
        }

        [HttpPost("BaixarCP", Name = nameof(BaixarCP))]
        [ProducesResponseType(typeof(List<ContasAPagar>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<ContasAPagar>>> BaixarCP([FromBody] ListCRDto dto)
        {
            try
            {
                var CpAtt = await _baixaCPService.Core(dto);
                return Ok(CpAtt);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity.");
                return StatusCode(500, new ErrorMessage(500,
                    "An error occurred while updating entity. Please try again later."
                    ));
            }
        }


        [HttpGet("GetContasAPagarPorEmpresa", Name = nameof(GetContasAPagarPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ContasAPagar>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ContasAPagar>>> GetContasAPagarPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoCAP = 0,
            [FromQuery] int tipoDataCap = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string pagou = "N",
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nrEntrada = null,
            [FromQuery] string? nrDuplicata = null,
            [FromQuery] string? cdHistoricoCaixa = null,
            [FromQuery] string? cdPlanoCaixa = null
        )
        {
            try
            {
                TipoDataCap ENUM_tipoDataCap = TipoDataCap.TDC_Lancamento;
                TipoPeriodoCAP ENUM_tipoPeriodoCAP = TipoPeriodoCAP.TPC_Geral;
                try
                {
                    if (tipoDataCap < 0 || tipoDataCap > 2 || tipoPeriodoCAP < 0 || tipoPeriodoCAP > 2)
                    {
                        return BadRequest(new ErrorMessage(500, "Invalid parameters"));
                    }
                    ENUM_tipoDataCap = (TipoDataCap)tipoDataCap;
                    ENUM_tipoPeriodoCAP = (TipoPeriodoCAP)tipoPeriodoCAP;
                }
                catch
                {
                    return BadRequest(new ErrorMessage(500, "Invalid parameters"));
                }
                var query = await ((ContasAPagarRepository)repo).GetContasAPagarAsyncPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound(new ErrorMessage(404, "Entities not found"));
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmForn = UtlStrings.RemoveDiacritics(nmForn.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmForn));
                }

                if (nrEntrada.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrEntrada != null && p.NrEntrada.Equals(nrEntrada.Value));
                }

                if (!string.IsNullOrEmpty(nrDuplicata))
                {
                    var normalizedNrDuplicata = UtlStrings.RemoveDiacritics(nrDuplicata.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NrDuplicata == null) ? "" : p.NrDuplicata.ToLower()) == normalizedNrDuplicata);
                }

                if (!string.IsNullOrEmpty(pagou))
                {
                    filteredQuery = filteredQuery.Where(p => p.Pagou == pagou);
                }

                switch ((int)ENUM_tipoPeriodoCAP)
                {
                    case (int)TipoPeriodoCAP.TPC_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataCap)
                            {
                                case (int)TipoDataCap.TDC_Lancamento:
                                    filteredQuery = filteredQuery.Where(p => p.DtLancamento >= dtInicial && p.DtLancamento <= dtFinal);
                                    break;
                                case (int)TipoDataCap.TDC_Vencimento:
                                    filteredQuery = filteredQuery.Where(p => p.DtVencimento >= dtInicial && p.DtVencimento <= dtFinal);
                                    break;
                                case (int)TipoDataCap.TDC_Pagamento:
                                    filteredQuery = filteredQuery.Where(p => p.DtPagou >= dtInicial && p.DtPagou <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case (int)TipoPeriodoCAP.TPC_Ate_Data:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataCap)
                            {
                                case (int)TipoDataCap.TDC_Lancamento:
                                    filteredQuery = filteredQuery.Where(p => p.DtLancamento <= dtFinal);
                                    break;
                                case (int)TipoDataCap.TDC_Vencimento:
                                    filteredQuery = filteredQuery.Where(p => p.DtVencimento <= dtFinal);
                                    break;
                                case (int)TipoDataCap.TDC_Pagamento:
                                    filteredQuery = filteredQuery.Where(p => p.DtPagou <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(cdHistoricoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdHistoricoCaixa == cdHistoricoCaixa);
                }

                if (!string.IsNullOrEmpty(cdPlanoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdPlanoCaixa == cdPlanoCaixa);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ContasAPagar>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound(new ErrorMessage(404, "Entities not found."));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, new ErrorMessage(500,
                    "An error occurred while retrieving entities. Please try again later."
                    ));
            }
        }
    }
}
