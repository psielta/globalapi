using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfceAberturaCaixaController : GenericPagedControllerMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        public NfceAberturaCaixaController(IQueryRepositoryMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto> repo, ILogger<GenericPagedControllerMultiKey<NfceAberturaCaixa, int, int, NfceAberturaCaixaDto>> logger, GlobalErpFiscalBaseContext context) : base(repo, logger)
        {
            _context = context;
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NfceAberturaCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NfceAberturaCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceAberturaCaixa>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NfceAberturaCaixa>> Create([FromBody] NfceAberturaCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceAberturaCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceAberturaCaixa>> Update(int idEmpresa, int idCadastro, [FromBody] NfceAberturaCaixaDto dto)
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

        [HttpGet("/GetNfceAberturaCaixaByUnity/{unity}")]
        [ProducesResponseType(typeof(PagedResponse<NfceAberturaCaixa>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PagedResponse<NfceAberturaCaixa>>> GetNfceAberturaCaixaByUnity(
            [FromRoute] int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? empresa = null,
            [FromQuery] string? dataInicioIsoString = null,
            [FromQuery] string? dataFimIsoString = null
        )
        {
            try
            {
                string SQL = $@"
                    SELECT * from nfce_abertura_caixa where unity = {unity} 
                    {((empresa.HasValue) ? ($" and cd_empresa = {empresa} ") : (""))}
                    {((string.IsNullOrEmpty(dataInicioIsoString)) ? ("") : ($" and data_lanc >= '{dataInicioIsoString}' "))} 
                    {((string.IsNullOrEmpty(dataFimIsoString)) ? ("") : ($" and data_lanc <= '{dataFimIsoString}' "))} 
                ";

                var query = this._context.NfceAberturaCaixas.FromSqlRaw(SQL)
                    .Include(e => e.CdEmpresaNavigation)
                    .OrderByDescending(e => e.DataLanc)
                    .AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<NfceAberturaCaixa>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError("Erro ao obter NFCeSaida", ex);
                return StatusCode(500, $"Erro ao obter NFCeSaida: {ex.Message}");
            }
        }

        [HttpGet("/NfceAberturaCaixaBySequence/{sequence}")]
        [ProducesResponseType(typeof(NfceAberturaCaixaDtoBySequence), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NfceAberturaCaixaDtoBySequence>> NfceAberturaCaixaBySequence(int sequence)
        {
            try
            {
                NfceAberturaCaixa? entity = await _context.NfceAberturaCaixas
                    .Include(e => e.CdEmpresaNavigation)
                    .Include(e => e.SangriaCaixas)
                    .AsNoTracking().Where(x => x.Sequence == sequence).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NotFound($"Entity with ID {sequence} not found."); // 404 Resource not found
                }

                NfceAberturaCaixaDtoBySequence dto = new NfceAberturaCaixaDtoBySequence();

                dto.abertura = entity;
                dto.sangrias = entity.SangriaCaixas;

                return Ok(dto); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the entity with ID {sequence}.", sequence);
                return StatusCode(500, $"An error occurred while retrieving the entity with ID {sequence}. Please try again later.");
            }
        }
    }
}
