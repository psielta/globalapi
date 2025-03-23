using System.Globalization;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFe.Classes.Informacoes.Transporte;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroCaixaController : GenericPagedController<LivroCaixa, long, LivroCaixaDto>
    {
        public LivroCaixaController(IQueryRepository<LivroCaixa, long, LivroCaixaDto> repo, ILogger<GenericPagedController<LivroCaixa, long, LivroCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<LivroCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LivroCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<LivroCaixa>> GetEntity(long id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(LivroCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<LivroCaixa>> Create([FromBody] LivroCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<LivroCaixa>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<LivroCaixa>>> CreateBulk([FromBody] IEnumerable<LivroCaixaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LivroCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<LivroCaixa>> Update(long id, [FromBody] LivroCaixaDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }

        public enum TipoPeriodoLC
        {
            TPC_Geral = 0,
            TPC_Periodo = 1,
            TPC_Ate_Data = 2,
        }

        [HttpGet("GetLivroCaixaPorEmpresa", Name = nameof(GetLivroCaixaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<LivroCaixa>>> GetLivroCaixaPorEmpresa(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? NrCp = null,
            [FromQuery] int? NrCr = null,
            [FromQuery] int? NrConta = null,
            [FromQuery] string? CdHistorico = null,
            [FromQuery] string? CdPlano = null,
            [FromQuery] int tipoPeriodoLC = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null
        )
        {
            try
            {
                var query = ((LivroCaixaRepository)repo).GetLivroCaixaAsyncPorUnity(unity).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                if (!string.IsNullOrEmpty(CdHistorico))
                {
                    query = query.Where(p => p.CdHistorico.ToLower().Contains(CdHistorico.ToLower()));
                }
                if (!string.IsNullOrEmpty(CdPlano))
                {
                    query = query.Where(p => p.CdPlano.ToLower().Contains(CdPlano.ToLower()));
                }
                if (NrCp.HasValue)
                {
                    query = query.Where(p => p.NrCp == NrCp.Value);
                }
                if (NrCr.HasValue)
                {
                    query = query.Where(p => p.NrCr == NrCr.Value);
                }
                if (NrConta.HasValue)
                {
                    query = query.Where(p => p.NrConta == NrConta.Value);
                }

                TipoPeriodoLC enumPeriodoLC = (TipoPeriodoLC)tipoPeriodoLC;

                switch (enumPeriodoLC)
                {
                    case TipoPeriodoLC.TPC_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            var dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            var dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            query = query.Where(p => DateOnly.FromDateTime(p.DtLanc) >= dtInicial && DateOnly.FromDateTime(p.DtLanc) <= dtFinal);
                        }
                        break;
                    case TipoPeriodoLC.TPC_Ate_Data:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            var dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            query = query.Where(p => DateOnly.FromDateTime(p.DtLanc) <= dtFinal);
                        }
                        break;
                    case TipoPeriodoLC.TPC_Geral:
                    default:
                        // Nenhum filtro de data
                        break;
                }

                var pagedList = await query.OrderByDescending(x => x.NrLanc).ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<LivroCaixa>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpGet("GetLivroCaixaPorEmpresa_ALL", Name = nameof(GetLivroCaixaPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<LivroCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<LivroCaixa>>> GetLivroCaixaPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((LivroCaixaRepository)repo).GetLivroCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.AsNoTracking().ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(list); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
