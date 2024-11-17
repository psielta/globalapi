using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Org.BouncyCastle.Math.EC.ECCurve;
using GlobalErpData.Dto;
using ACBrLib.NFe;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Utils;
using System.Net.Http.Headers;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Saida_ConsultarSituacaoNFeController : ControllerBase
    {
        private readonly ILogger<Saida_ConsultarSituacaoNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public Saida_ConsultarSituacaoNFeController(IConfiguration config,
            ILogger<Saida_ConsultarSituacaoNFeController> logger,
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
        public async Task<ActionResult<ConsultaNFeDto>> Get(int nrLanc, [FromBody] PostConsultaDto sessionHubDto)
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
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Iniciando validações...");
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
                    if (saida.ProdutoSaida == null || saida.ProdutoSaida.Count == 0)
                    {
                        return NotFound(new NotFound("Nenhum produto encontrado."));
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
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Iniciando consulta da NFe...");
                    ResponseConsultaAcbr responseConsulta = await nFeGlobalService.ConsultaNFe(saida, empresa, cer, sessionHubDto.sessionId);
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
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Gerado com sucesso.");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Alterar Situação (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    return BadRequest(new ConsultaNFeDto(errorMessage));
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro emissão NFe: {ex.Message}");
                return BadRequest();
            }

            return Ok(new ConsultaNFeDto(vret));
        }
    }

}
