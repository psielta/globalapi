using ACBrLib.Core.DFe;
using ACBrLib.Core.NFe;
using ACBrLib.NFe;
using AutoMapper;
using GlobalAPI_ACBrNFe.Lib;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
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
using System.Security.Claims;
using System.Text;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GerarNFeController : ControllerBase
    {
        private readonly ILogger<GerarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public GerarNFeController(IConfiguration config,
            ILogger<GerarNFeController> logger,
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostWithId(/*[FromServices] ACBrNFe nfe,*/ int nrLanc, [FromBody] SessionHubDto sessionHubDto)
        {
            NotaFiscal notaFiscal = null;
            try
            {
                #region Validações
                await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Iniciando validações...");
                Saida? saida = await db.Saidas
                    .Include(f => f.Fretes)
                    .Include(p => p.CdGrupoEstoqueNavigation)
                    .Include(p => p.ProdutoSaida)
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
                    await nFeGlobalService.GerarNFeAsync(notaFiscal, saida, empresa, cer);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Gerar NFe (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion
                #region Armazenar XML
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Armazenar XML (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion
                #region Armazenar PDF
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Armazenar PDF (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion
                #region Alterar Situação
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Montando NFe.");
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Alterar Situação (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion 
                #region Atualizar Saida
                try
                {
                    await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Atualizando dados Saida.");
                    //realizar post saida
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Erro ao Alterar Situação (nrLanc={nrLanc}, empresa={saida.Empresa}): {ex.Message}";
                    _logger.LogError(errorMessage);
                    return BadRequest(new BadRequest(errorMessage));
                }
                #endregion


                await _hubContext.Clients.Group(sessionHubDto.sessionId).SendAsync("ReceiveProgress", "Gerado com sucesso.");

                return Ok();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro emissão NFe: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
