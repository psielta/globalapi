using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using GlobalErpData.Dto;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Utils;
using System.Net.Http.Headers;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Saida_InutilizarNFeController : ControllerBase
    {
        private readonly ILogger<Saida_InutilizarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public Saida_InutilizarNFeController(IConfiguration config,
            ILogger<Saida_InutilizarNFeController> logger,
            IMapper mapper,
            IHubContext<ImportProgressHub> hubContext,
            GlobalErpFiscalBaseContext db,
            NFeGlobalService nFeGlobalService
            )

        {
            _config = config;
            _logger = logger;
            this.mapper = mapper;
            _hubContext = hubContext;
            this.db = db;
            this.nFeGlobalService = nFeGlobalService;
        }

        [HttpPost("{nrLanc}")]
        [ProducesResponseType(typeof(ConsultaNFeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConsultaNFeDto>> Post(int nrLanc, [FromBody] PostCancelamentoDto sessionHubDto)
        {
            Saida? saida = null;
            Certificado? cer = null;
            Empresa? empresa = null;
            string vret = string.Empty;
            try
            {
                #region Validações Iniciais
                try
                {
                    if (sessionHubDto == null || string.IsNullOrEmpty(sessionHubDto.sessionId))
                    {
                        return BadRequest(new BadRequest("Sessão inválida."));
                    }
                    else if (sessionHubDto.justificativa == null || sessionHubDto.justificativa.Length == 0)
                    {
                        return BadRequest(new BadRequest("Justificativa não informada."));
                    }
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Buscando dados NFe...");
                    saida = await db.Saidas
                        .Include(f => f.Fretes).ThenInclude(f => f.Transportadora).ThenInclude(f => f.CdCidadeNavigation)
                        .Include(s => s.SaidasVolumes)
                        .Include(p => p.CdGrupoEstoqueNavigation)
                        .Include(p => p.ProdutoSaida).ThenInclude(p => p.ProdutoEstoque)
                        .Include(e => e.ClienteNavigation).ThenInclude(cliente => cliente.CdCidadeNavigation)
                        .Include(l => l.SaidaNotasDevolucaos)
                        .Where(s => s.NrLanc == nrLanc).FirstOrDefaultAsync();


                    if (saida == null)
                    {
                        return NotFound(new NotFound());
                    }
                    if (saida.CdGrupoEstoqueNavigation == null)
                    {
                        return NotFound(new NotFound("Grupo de estoque não encontrado."));
                    }
                    if (saida.ClienteNavigation == null)
                    {
                        return NotFound(new NotFound("Cliente não encontrado."));
                    }
                    if (saida.TpSaida.Equals("A"))
                    {
                        return BadRequest(new BadRequest("Nota de acerto não pode ser emitida."));
                    }
                    if (!(saida.CdSituacao ?? "").Equals("01"))
                    {
                        return BadRequest(new BadRequest("Nota não pode ser cancelada pois foi emitida."));
                    }
                    empresa = await db
                        .Empresas
                        .Include(c => c.CdCidadeNavigation)
                        .Where(e => e.CdEmpresa == saida.Empresa).FirstOrDefaultAsync();
                    if (empresa == null || empresa.CdCidadeNavigation == null)
                    {
                        throw new Exception("Empresa não encontrada");
                    }
                    DateOnly dataAtual = DateUtils.DateTimeToDateOnly(DateTime.Now);
                    cer = await db.Certificados
                        .Where(c => c.IdEmpresa == saida.Empresa
                        && c.ValidadeCert >= dataAtual)
                        .FirstOrDefaultAsync();
                    if (cer == null)
                    {
                        throw new Exception("Certificado não encontrado");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao buscar dados (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    return BadRequest(new ConsultaNFeDto(errorMessage));
                }
                #endregion

                #region Core
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Iniciando inutilização da NFe...");
                    ResponseConsultaAcbr responseConsulta = await nFeGlobalService.InutilizacaoNFe(saida, empresa, cer, sessionHubDto);
                    vret = responseConsulta.Message;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao emitir NFe (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    return BadRequest(new ConsultaNFeDto(errorMessage));
                }
                #endregion

                #region Atualizar Saida
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Atualizando dados Saida.");
                    HttpClient client = new HttpClient();
                    string url = Constants.URL_API_NFE + "/api/Saida/" + saida.NrLanc;
                    SaidaDto saidaDto = mapper.Map<SaidaDto>(saida);
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseEndpoint = await client.PutAsJsonAsync(url, saidaDto);
                    if (!responseEndpoint.IsSuccessStatusCode)
                    {
                        throw new Exception($"Erro ao atualizar Saida ({saida.NrLanc}) HTTP {responseEndpoint.StatusCode}: {responseEndpoint.Content}.");
                    }
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Finalizado com sucesso");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Atualizar saida (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    return BadRequest(new ConsultaNFeDto(errorMessage));
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inutilização NFe: {ex.Message}");
                return BadRequest();
            }

            return Ok(new ConsultaNFeDto(vret));
        }

        [HttpGet("Imprimir/{id}")]
        [Produces("application/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Imprimir(int id)
        {
            Saida? saida = await db.Saidas.FindAsync(id);
            if (saida == null || saida.Pdf == null)
            {
                return NotFound(new NotFound("Nota não encontrada"));
            }
            if (saida.CdSituacao != "70")
            {
                return BadRequest(new BadRequest("Nota não inutilizada"));
            }
            if (saida.PdfInu == null)
            {
                return NotFound(new NotFound("PDF não encontrado."));
            }

            string contentType = "application/pdf";
            string fileName = $"cancelamento_{id}.pdf";
            Response.Headers[HeaderNames.ContentDisposition] = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("inline")
            {
                FileName = fileName
            }.ToString();

            return File(saida.PdfInu, contentType);
        }
    }
}
