using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using System.Collections.Generic;
using System.Globalization;
using GlobalErpData.Services;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>
    {
        private readonly EntradaCalculationService _calculationService;
        public EntradaController(
            IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repo,
            ILogger<GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>> logger,
            EntradaCalculationService calculationService
        ) : base(repo, logger)
        {
            _calculationService = calculationService;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Entrada>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Entrada>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Entrada), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Entrada>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Entrada), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Entrada>> Create([FromBody] EntradaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Entrada), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Entrada>> Update(int idEmpresa, int idCadastro, [FromBody] EntradaDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, int idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }

        [HttpGet("GetEntradaPorEmpresa", Name = nameof(GetEntradaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Entrada>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Entrada>>> GetEntradaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoEntrada = 0,
            [FromQuery] int tipoDataEntrada = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nr = null,
            [FromQuery] int? nrNotaFiscal = null,
            [FromQuery] int? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoEntrada = null
)
        {
            try
            {
                TipoDataEntrada ENUM_tipoDataEntrada = TipoDataEntrada.TDE_DataEntrada;
                TipoPeriodoEntrada ENUM_tipoPeriodoEntrada = TipoPeriodoEntrada.TPE_Geral;

                if (tipoDataEntrada < 0 || tipoDataEntrada > 2 || tipoPeriodoEntrada < 0 || tipoPeriodoEntrada > 2)
                {
                    return BadRequest(new ErrorMessage(500, "Invalid parameters"));
                }

                ENUM_tipoDataEntrada = (TipoDataEntrada)tipoDataEntrada;
                ENUM_tipoPeriodoEntrada = (TipoPeriodoEntrada)tipoPeriodoEntrada;

                var query = await ((EntradaPagedRepository)repo).GetEntradaAsyncPorEmpresa(idEmpresa);

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

                if (nr.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.Nr == nr.Value);
                }

                if (nrNotaFiscal.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNf != null && p.NrNf.Equals(nrNotaFiscal.Value.ToString()));
                }

                if (serie.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie.Value.ToString()));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdChaveNfe == null) ? "" : p.CdChaveNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoEntrada))
                {
                    var normalizedTipoEntrada = UtlStrings.RemoveDiacritics(tipoEntrada.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.TpEntrada ?? "").ToUpper() == normalizedTipoEntrada);
                }

                switch ((int)ENUM_tipoPeriodoEntrada)
                {
                    case (int)TipoPeriodoEntrada.TPE_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataEntrada)
                            {
                                case (int)TipoDataEntrada.TDE_DataEntrada:
                                    filteredQuery = filteredQuery.Where(p => p.Data >= dtInicial && p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataEmissao:
                                    filteredQuery = filteredQuery.Where(p => p.DtEmissao >= dtInicial && p.DtEmissao <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida >= dtInicial && p.DtSaida <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case (int)TipoPeriodoEntrada.TPE_AteData:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataEntrada)
                            {
                                case (int)TipoDataEntrada.TDE_DataEntrada:
                                    filteredQuery = filteredQuery.Where(p => p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataEmissao:
                                    filteredQuery = filteredQuery.Where(p => p.DtEmissao <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                foreach (var entrada in filteredQuery)
                {
                    _calculationService.CalculateTotals(entrada);
                }
                filteredQuery = filteredQuery.OrderBy(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Entrada>(pagedList);

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

        public enum TipoDataEntrada
        {
            TDE_DataEntrada = 0,
            TDE_DataEmissao = 1,
            TDE_DataSaida = 2
        }

        public enum TipoPeriodoEntrada
        {
            TPE_Geral = 0,
            TPE_Periodo = 1,
            TPE_AteData = 2
        }


    }
}
