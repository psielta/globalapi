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
    public class Saida_GerarNFeController : ControllerBase
    {
        private readonly ILogger<Saida_GerarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public Saida_GerarNFeController(IConfiguration config,
            ILogger<Saida_GerarNFeController> logger,
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
        [ProducesResponseType(typeof(ResponseEnviarDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseEnviarDto>> PostWithId(/*[FromServices] ACBrNFe nfe,*/ int nrLanc, [FromBody] SessionHubDto sessionHubDto)
        {
            ResponseEnviarDto response = new ResponseEnviarDto();
            ResponseGerarDto responseGerar;

            NotaFiscal notaFiscal = null;
            try
            {
                #region Validações
                await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Iniciando validações...");
                Saida? saida = await db.Saidas
                    .Include(f => f.Fretes)
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
                Empresa? empresa = await db
                    .Empresas
                    .Include(c => c.CdCidadeNavigation)
                    .Where(e => e.CdEmpresa == saida.Empresa).FirstOrDefaultAsync();
                if (empresa == null || empresa.CdCidadeNavigation == null)
                {
                    throw new Exception("Empresa não encontrada");
                }
                DateOnly dataAtual = DateUtils.DateTimeToDateOnly(DateTime.Now);
                Certificado? cer = await db.Certificados
                    .Where(c => c.IdEmpresa == saida.Empresa
                    && c.ValidadeCert >= dataAtual)
                    .FirstOrDefaultAsync();
                if (cer == null)
                {
                    throw new Exception("Certificado não encontrado");
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
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion
                #region Gerar NFe
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                    responseGerar = await nFeGlobalService.GerarNFeAsync(notaFiscal, saida, empresa, cer);
                    if (responseGerar.envioRetornoResposta == null)
                    {
                        throw new Exception("Erro ao gerar NFe");
                    }
                    else if (responseGerar.envioRetornoResposta.Envio.CStat != 100)
                    {
                        response.success = false;
                        response.message = responseGerar.envioRetornoResposta.Resposta;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Gerar NFe (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion

                if (response.success)
                {
                    #region Atualizar Saida
                    try
                    {
                        await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Atualizando dados Saida.");
                        saida.CdSituacao = "02";
                        saida.NrAutorizacaoNfe = responseGerar.envioRetornoResposta.Envio.NRec;
                        saida.XmNf = responseGerar.xml;

                        if (System.IO.File.Exists(responseGerar.pathPdf))
                        {
                            saida.Pdf = System.IO.File.ReadAllBytes(responseGerar.pathPdf);
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
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Erro ao Alterar Situação (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                        _logger.LogError(errorMessage);
                        return BadRequest(new BadRequest(errorMessage));
                    }
                    #endregion
                }


                await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Gerado com sucesso.");

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
