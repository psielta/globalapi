﻿using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    public class SaidaController : GenericPagedController<Saida, int, SaidaDto>
    {
        public SaidaController(IQueryRepository<Saida, int, SaidaDto> repo, ILogger<GenericPagedController<Saida, int, SaidaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Saida>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Saida>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Saida), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Saida>> Create([FromBody] SaidaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> Update(int id, [FromBody] SaidaDto dto)
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
        [ProducesResponseType(typeof(IEnumerable<Saida>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Saida>>> CreateBulk([FromBody] IEnumerable<SaidaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpGet("GetSaidaPorEmpresa", Name = nameof(GetSaidaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Saida>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Saida>>> GetSaidaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoSaida = 0,
            [FromQuery] int tipoDataSaida = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string? nmCliente = null,
            [FromQuery] int? nrLanc = null,
            [FromQuery] string? nrNotaFiscal = null,
            [FromQuery] string? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoSaida = null,
            [FromQuery] string? cdSituacao = null

        )
        {
            try
            {
                TipoDataSaida ENUM_tipoDataSaida = TipoDataSaida.TDS_DataSaida;
                TipoPeriodoSaida ENUM_tipoPeriodoSaida = TipoPeriodoSaida.TPS_Geral;

                if (tipoDataSaida < 0 || tipoDataSaida > 2 || tipoPeriodoSaida < 0 || tipoPeriodoSaida > 2)
                {
                    return BadRequest(new ErrorMessage(500, "Parâmetros inválidos"));
                }

                ENUM_tipoDataSaida = (TipoDataSaida)tipoDataSaida;
                ENUM_tipoPeriodoSaida = (TipoPeriodoSaida)tipoPeriodoSaida;

                var query = await ((SaidaRepository)repo).GetSaidaAsyncPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound(new ErrorMessage(404, "Entidades não encontradas"));
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmCliente))
                {
                    var normalizedNmCliente = UtlStrings.RemoveDiacritics(nmCliente.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmCliente == null) ? "" : p.NmCliente.ToLower()).Contains(normalizedNmCliente));
                }

                if (nrLanc.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrLanc == nrLanc.Value);
                }

                if (!string.IsNullOrEmpty(nrNotaFiscal))
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNotaFiscal != null && p.NrNotaFiscal.Equals(nrNotaFiscal));
                }

                if (!string.IsNullOrEmpty(serie))
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.ChaveAcessoNfe == null) ? "" : p.ChaveAcessoNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoSaida))
                {
                    var normalizedTipoSaida = UtlStrings.RemoveDiacritics(tipoSaida.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.TpSaida ?? "").ToUpper() == normalizedTipoSaida);
                }
                if (!string.IsNullOrEmpty(cdSituacao))
                {
                    var normalizedCdSituacao = UtlStrings.RemoveDiacritics(cdSituacao.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.CdSituacao ?? "").ToUpper() == normalizedCdSituacao);
                }

                switch ((int)ENUM_tipoPeriodoSaida)
                {
                    case (int)TipoPeriodoSaida.TPS_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataSaida)
                            {
                                case (int)TipoDataSaida.TDS_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida >= dtInicial && p.DtSaida <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_Data:
                                    filteredQuery = filteredQuery.Where(p => p.Data >= dtInicial && p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_DataPedido:
                                    filteredQuery = filteredQuery.Where(p => p.DtPedido >= dtInicial && p.DtPedido <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case (int)TipoPeriodoSaida.TPS_AteData:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataSaida)
                            {
                                case (int)TipoDataSaida.TDS_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_Data:
                                    filteredQuery = filteredQuery.Where(p => p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_DataPedido:
                                    filteredQuery = filteredQuery.Where(p => p.DtPedido <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.NrLanc);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Saida>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound(new ErrorMessage(404, "Entidades não encontradas."));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar as entidades paginadas.");
                return StatusCode(500, new ErrorMessage(500,
                    "Ocorreu um erro ao recuperar as entidades. Por favor, tente novamente mais tarde."
                    ));
            }
        }

        public enum TipoDataSaida
        {
            TDS_DataSaida = 0,
            TDS_Data = 1,
            TDS_DataPedido = 2
        }

        public enum TipoPeriodoSaida
        {
            TPS_Geral = 0,
            TPS_Periodo = 1,
            TPS_AteData = 2
        }
    }
}