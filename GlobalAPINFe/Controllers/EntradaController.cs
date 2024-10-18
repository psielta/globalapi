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

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>
    {
        public EntradaController(IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repo, ILogger<GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>> logger) : base(repo, logger)
        {
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

        // Método personalizado ajustado
        [HttpGet("GetEntradaPorEmpresa", Name = nameof(GetEntradaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Entrada>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Entrada>>> GetEntradaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nrNotaFiscal = null,
            [FromQuery] int? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoEntrada = null,
            [FromQuery] string? dataInicio = null,
            [FromQuery] string? dataFim = null)
        {
            try
            {
                var query = ((EntradaPagedRepository)repo).GetEntradaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmProduto = UtlStrings.RemoveDiacritics(nmForn.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmProduto));
                }

                if (nrNotaFiscal.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNf != null && p.NrNf.Equals(nrNotaFiscal.ToString()));
                }

                if (serie.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie.ToString()));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdChaveNfe == null) ? "" : p.CdChaveNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoEntrada))
                {
                    var normalizedTipoEntrada = UtlStrings.RemoveDiacritics(tipoEntrada.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.TpEntrada == null) ? "" : p.TpEntrada.ToLower()) == normalizedTipoEntrada);
                }

                if (!string.IsNullOrEmpty(dataInicio))
                {
                    filteredQuery = filteredQuery.Where(p => p.Data.ToDateTime(TimeOnly.MinValue) >= DateTime.Parse(dataInicio));
                }

                if (!string.IsNullOrEmpty(dataFim))
                {
                    filteredQuery = filteredQuery.Where(p => p.Data.ToDateTime(TimeOnly.MaxValue) <= DateTime.Parse(dataFim));
                }

                filteredQuery = filteredQuery.OrderBy(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Entrada>(pagedList);

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
