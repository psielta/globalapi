using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using static GlobalAPINFe.Controllers.ContasAPagarController;
using System.Globalization;
using X.PagedList.Extensions;
using GlobalLib.Dto;
using GlobalErpData.Services;

namespace GlobalAPINFe.Controllers
{
    public class ContasAReceberController : GenericPagedController<ContasAReceber, int, ContasAReceberDto>
    {
        private readonly BaixaCRService _baixaCRService;
        public ContasAReceberController(IQueryRepository<ContasAReceber, int, ContasAReceberDto> repo, ILogger<GenericPagedController<ContasAReceber, int, ContasAReceberDto>> logger,
            BaixaCRService baixaCRService) : base(repo, logger)
        {
            _baixaCRService = baixaCRService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ContasAReceber>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ContasAReceber>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContasAReceber), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContasAReceber>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContasAReceber), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ContasAReceber>> Create([FromBody] ContasAReceberDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContasAReceber), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContasAReceber>> Update(int id, [FromBody] ContasAReceberDto dto)
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
        [ProducesResponseType(typeof(IEnumerable<ContasAReceber>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ContasAReceber>>> CreateBulk([FromBody] IEnumerable<ContasAReceberDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPost("BaixarCR", Name = nameof(BaixarCR))]
        [ProducesResponseType(typeof(List<ContasAReceber>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<ContasAReceber>>> BaixarCR([FromBody] ListCRDto dto)
        {
            try
            {
                var CrAtt = await _baixaCRService.Core(dto);
                return Ok(CrAtt);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity.");
                return StatusCode(500, new ErrorMessage(500,
                    "An error occurred while updating entity. Please try again later."
                    ));
            }
        }

        [HttpPost("Revert/{nrConta}", Name = nameof(RevertCR))]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> RevertCR(int nrConta)
        {
            try
            {
                await _baixaCRService.Revert(nrConta);
                return Ok(new Success());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity.");
                return StatusCode(500, new InternalServerError(
                    "An error occurred while updating entity. Please try again later."
                    ));
            }
        }
        public enum TipoPeriodoCAR
        {
            TPC_Geral = 0,
            TPC_Periodo = 1,
            TPC_Ate_Data = 2,
        }

        public enum TipoDataCar
        {
            TDC_Lancamento = 0,
            TDC_Vencimento = 1,
            TDC_Pagamento = 2,
        }

        [HttpGet("GetContasAReceberPorEmpresa", Name = nameof(GetContasAReceberPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<ContasAReceber>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ContasAReceber>>> GetContasAReceberPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoCAR = 0,
            [FromQuery] int tipoDataCar = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string recebeu = "N",
            [FromQuery] string? nmCliente = null,
            [FromQuery] int? nrSaida = null,
            [FromQuery] string? nrDuplicata = null,
            [FromQuery] string? cdHistoricoCaixa = null,
            [FromQuery] string? cdPlanoCaixa = null
        )
        {
            try
            {
                // Enumerações de períodos e tipos de data
                var ENUM_tipoDataCar = (TipoDataCar)tipoDataCar;
                var ENUM_tipoPeriodoCAR = (TipoPeriodoCAR)tipoPeriodoCAR;

                var query = await ((ContasAReceberRepository)repo).GetContasAReceberAsyncPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound(new ErrorMessage(404, "Entities not found"));
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmCliente))
                {
                    var normalizedNmCliente = UtlStrings.RemoveDiacritics(nmCliente.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdClienteNavigation.NmCliente == null) ? "" : p.CdClienteNavigation.NmCliente.ToLower()).Contains(normalizedNmCliente));
                }

                if (nrSaida.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrSaida != null && p.NrSaida.Equals(nrSaida.Value));
                }

                if (!string.IsNullOrEmpty(nrDuplicata))
                {
                    var normalizedNrDuplicata = UtlStrings.RemoveDiacritics(nrDuplicata.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NrDuplicata == null) ? "" : p.NrDuplicata.ToLower()) == normalizedNrDuplicata);
                }

                if (!string.IsNullOrEmpty(recebeu))
                {
                    filteredQuery = filteredQuery.Where(p => p.Recebeu == recebeu);
                }

                // Filtro por período
                if (ENUM_tipoPeriodoCAR == TipoPeriodoCAR.TPC_Periodo && !string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                {
                    DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    switch (ENUM_tipoDataCar)
                    {
                        case TipoDataCar.TDC_Lancamento:
                            filteredQuery = filteredQuery.Where(p => p.DataLanc >= dtInicial && p.DataLanc <= dtFinal);
                            break;
                        case TipoDataCar.TDC_Vencimento:
                            filteredQuery = filteredQuery.Where(p => p.DtVencimento >= dtInicial && p.DtVencimento <= dtFinal);
                            break;
                        case TipoDataCar.TDC_Pagamento:
                            filteredQuery = filteredQuery.Where(p => p.DtPagamento >= dtInicial && p.DtPagamento <= dtFinal);
                            break;
                    }
                }
                else if (ENUM_tipoPeriodoCAR == TipoPeriodoCAR.TPC_Ate_Data && !string.IsNullOrEmpty(periodoFinal))
                {
                    DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    switch (ENUM_tipoDataCar)
                    {
                        case TipoDataCar.TDC_Lancamento:
                            filteredQuery = filteredQuery.Where(p => p.DataLanc <= dtFinal);
                            break;
                        case TipoDataCar.TDC_Vencimento:
                            filteredQuery = filteredQuery.Where(p => p.DtVencimento <= dtFinal);
                            break;
                        case TipoDataCar.TDC_Pagamento:
                            filteredQuery = filteredQuery.Where(p => p.DtPagamento <= dtFinal);
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(cdHistoricoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdHistoricoCaixa == cdHistoricoCaixa);
                }

                if (!string.IsNullOrEmpty(cdPlanoCaixa))
                {
                    filteredQuery = filteredQuery.Where(p => p.CdPlanoCaixa == cdPlanoCaixa);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.NrConta);
                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ContasAReceber>(pagedList);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving Contas a Receber for Empresa.");
                return StatusCode(500, new ErrorMessage(500, "An error occurred while retrieving entities. Please try again later."));
            }
        }

    }
}
