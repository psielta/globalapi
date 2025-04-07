using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities.Encoders;
using X.PagedList.EF;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NfceSaidaController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<NfceSaida, int, int, NfceSaidaDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        public NfceSaidaController(IQueryRepositoryMultiKey<NfceSaida, int, int, NfceSaidaDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<NfceSaida, int, int, NfceSaidaDto>> logger, GlobalErpFiscalBaseContext context) : base(repo, logger)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<NfceSaida>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<NfceSaida>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("/GetNfceSaidaByUnity/{unity}")]
        [ProducesResponseType(typeof(PagedResponse<NfceSaida>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PagedResponse<NfceSaida>>> GetNfceSaidaByUnity(
            [FromRoute] int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? cliente = null,
            [FromQuery] int? empresa = null,
            [FromQuery] string? cdSituacao = null,
            [FromQuery] string? dataInicioIsoString = null,
            [FromQuery] string? dataFimIsoString = null
        )
        {
            try
            {
                string SQL = $@"
                    SELECT * from nfce_saidas where unity = {unity} 
                    {((cliente.HasValue) ? ($" and cliente = {cliente} ") : (""))}
                    {((empresa.HasValue) ? ($" and empresa = {empresa} ") : (""))}
                    {((string.IsNullOrEmpty(cdSituacao)) ? ("") : ($" and cd_situacao = '{cdSituacao}' "))} 
                    {((string.IsNullOrEmpty(dataInicioIsoString)) ? ("") : ($" and data >= '{dataInicioIsoString}' "))} 
                    {((string.IsNullOrEmpty(dataFimIsoString)) ? ("") : ($" and data <= '{dataFimIsoString}' "))} 
                ";

                var query = this._context.NfceSaidas.FromSqlRaw(SQL)
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
                    .AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<NfceSaida>(pagedList);

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

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceSaida), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceSaida>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }
        
        [HttpGet("/NfceSaidaBySequence/{sequence}")]
        [ProducesResponseType(typeof(NfceSaidaDtoBySequence), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NfceSaidaDtoBySequence>> NfceSaidaBySequence(int sequence)
        {
            try
            {
                NfceSaida? entity = await _context.NfceSaidas
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
                    .AsNoTracking().Where(x => x.Sequence == sequence).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NotFound($"Entity with ID {sequence} not found."); // 404 Resource not found
                }

                NfceSaidaDtoBySequence nfceSaidaDtoBySequence = new NfceSaidaDtoBySequence();

                nfceSaidaDtoBySequence.itens = entity.NfceProdutoSaida;
                nfceSaidaDtoBySequence.cabecalho = entity;
                nfceSaidaDtoBySequence.formaPagamento = entity.NfceFormaPgts;
                nfceSaidaDtoBySequence.empresa = entity.EmpresaNavigation;
                nfceSaidaDtoBySequence.cliente = entity.ClienteNavigation;

                return Ok(nfceSaidaDtoBySequence); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the entity with ID {sequence}.", sequence);
                return StatusCode(500, $"An error occurred while retrieving the entity with ID {sequence}. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(NfceSaida), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<NfceSaida>> Create([FromBody] NfceSaidaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(NfceSaida), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<NfceSaida>> Update(int idEmpresa, int idCadastro, [FromBody] NfceSaidaDto dto)
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
    }
}
