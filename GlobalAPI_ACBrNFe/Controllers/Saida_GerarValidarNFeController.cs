using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using AutoMapper;
using GlobalAPI_ACBrNFe.Lib;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalErpData.Services;
using GlobalLib.Database;
using GlobalLib.Repository;
using GlobalLib.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Saida_GerarValidarNFeController : ControllerBase
    {
        private readonly ILogger<Saida_GerarValidarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public Saida_GerarValidarNFeController(IConfiguration config,
            ILogger<Saida_GerarValidarNFeController> logger,
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
        [ProducesResponseType(typeof(ResponseGerarDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseGerarDto>> PostWithId(/*[FromServices] ACBrNFe nfe,*/
            int nrLanc, [FromBody] PostGerarDto sessionHubDto
            )
        {
            ResponseGerarDto response = new ResponseGerarDto();
            NotaFiscal notaFiscal = null;
            Saida? saida = null;
            Certificado? cer = null;
            Empresa? empresa = null;
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
                    if ((saida.CdSituacao ?? "").Equals("02") || (saida.CdSituacao ?? "").Equals("11") || (saida.CdSituacao ?? "").Equals("70"))
                    {
                        return BadRequest(new BadRequest("Nota já emitida."));
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
                    response.Message = errorMessage;
                    response.StatusCode = 400;
                    response.success = false;
                    return BadRequest(response);
                }
                #endregion
                #region Montar NFe
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                    notaFiscal = await nFeGlobalService.MontarNFeAsync(saida, empresa, cer);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao montar Saida (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    response.Message = errorMessage;
                    response.success = false;
                    response.StatusCode = 400;
                    return BadRequest(response);
                }
                #endregion
                #region Validar
                string reponseValidacao = "";
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Validando NFe.");
                    reponseValidacao = await nFeGlobalService.ValidarNFeAsync(notaFiscal, saida, empresa, cer);
                    if (!reponseValidacao.Equals("Success"))
                    {
                        throw new Exception(reponseValidacao);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"{ex.Message}";
                    _logger.LogError(ex, errorMessage);
                    response.Message = errorMessage;
                    response.success = false;
                    response.StatusCode = 400;
                    return BadRequest(response);
                }
                #endregion
                if (sessionHubDto.Gerar && reponseValidacao.Equals("Success"))
                {
                    #region Gerar NFe
                    try
                    {
                        await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                        response = await nFeGlobalService.GerarNFeAsync(notaFiscal, saida, empresa, cer);
                        if (response.envioRetornoResposta == null)
                        {
                            throw new Exception("Erro ao gerar NFe");
                        }
                        else if (response.envioRetornoResposta.Envio.CStat != 100)
                        {
                            throw new Exception($"Erro ao gerar NFe: {response.envioRetornoResposta.Envio.XMotivo}");
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Erro ao Gerar NFe (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                        _logger.LogError(ex, errorMessage);
                        response.success = false;
                        response.Message = errorMessage;
                        response.StatusCode = 400;
                        return BadRequest(response);
                    }


                    #endregion
                    #region Atualizar Saida
                    try
                    {
                        await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Atualizando dados Saida.");
                        saida.CdSituacao = "02";
                        saida.NrAutorizacaoNfe = response.envioRetornoResposta.Envio.NRec;
                        saida.XmNf = response.xml;

                        if (System.IO.File.Exists(response.pathPdf))
                        {
                            saida.Pdf = System.IO.File.ReadAllBytes(response.pathPdf);
                        }
                        else
                        {
                            throw new Exception($"PDF não registrado (SAIDA: {saida.NrLanc})");
                        }

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
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Erro ao Alterar Situação (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                        _logger.LogError(ex, errorMessage);
                        response.success = false;
                        response.Message = errorMessage;
                        response.StatusCode = 400;
                        return BadRequest(response);
                    }

                    #endregion
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro emissão NFe: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
