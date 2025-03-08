using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
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
using System.Globalization;
using GlobalErpData.Services;
using GlobalLib.Dto;
using AutoMapper;
using GlobalErpData.Data;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradaController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>
    {
        private readonly EntradaCalculationService _calculationService; 
        private readonly IMapper mapper;
        private readonly GlobalErpFiscalBaseContext context;
        public EntradaController(
            IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repo,
            ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Entrada, int, int, EntradaDto>> logger,
            EntradaCalculationService calculationService, IMapper mapper, GlobalErpFiscalBaseContext _context
        ) : base(repo, logger)
        {
            _calculationService = calculationService;
            this.mapper = mapper;
            context = _context;
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

        [HttpGet("GetEntradaPorEmpresa", Name = nameof(GetEntradaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Entrada>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Entrada>>> GetEntradaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoEntrada = 0,
            [FromQuery] int tipoDataEntrada = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? nr = null,
            [FromQuery] int? nrNotaFiscal = null,
            [FromQuery] int? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoEntrada = null
)
        {
            try
            {
                TipoDataEntrada ENUM_tipoDataEntrada = TipoDataEntrada.TDE_DataEntrada;
                TipoPeriodoEntrada ENUM_tipoPeriodoEntrada = TipoPeriodoEntrada.TPE_Geral;

                if (tipoDataEntrada < 0 || tipoDataEntrada > 2 || tipoPeriodoEntrada < 0 || tipoPeriodoEntrada > 2)
                {
                    return BadRequest(new ErrorMessage(500, "Invalid parameters"));
                }

                ENUM_tipoDataEntrada = (TipoDataEntrada)tipoDataEntrada;
                ENUM_tipoPeriodoEntrada = (TipoPeriodoEntrada)tipoPeriodoEntrada;

                var query = await ((EntradaPagedRepository)repo).GetEntradaAsyncPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound(new ErrorMessage(404, "Entities not found"));
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmForn = UtlStrings.RemoveDiacritics(nmForn.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmForn));
                }

                if (nr.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.Nr == nr.Value);
                }

                if (nrNotaFiscal.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNf != null && p.NrNf.Equals(nrNotaFiscal.Value.ToString()));
                }

                if (serie.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie.Value.ToString()));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdChaveNfe == null) ? "" : p.CdChaveNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoEntrada))
                {
                    var normalizedTipoEntrada = UtlStrings.RemoveDiacritics(tipoEntrada.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.TpEntrada ?? "").ToUpper() == normalizedTipoEntrada);
                }

                switch ((int)ENUM_tipoPeriodoEntrada)
                {
                    case (int)TipoPeriodoEntrada.TPE_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataEntrada)
                            {
                                case (int)TipoDataEntrada.TDE_DataEntrada:
                                    filteredQuery = filteredQuery.Where(p => p.Data >= dtInicial && p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataEmissao:
                                    filteredQuery = filteredQuery.Where(p => p.DtEmissao >= dtInicial && p.DtEmissao <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida >= dtInicial && p.DtSaida <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case (int)TipoPeriodoEntrada.TPE_AteData:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataEntrada)
                            {
                                case (int)TipoDataEntrada.TDE_DataEntrada:
                                    filteredQuery = filteredQuery.Where(p => p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataEmissao:
                                    filteredQuery = filteredQuery.Where(p => p.DtEmissao <= dtFinal);
                                    break;
                                case (int)TipoDataEntrada.TDE_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                foreach (var entrada in filteredQuery)
                {
                    _calculationService.CalculateTotals(entrada);
                }
                filteredQuery = filteredQuery.OrderByDescending(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Entrada>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound(new ErrorMessage(404, "Entities not found."));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, new ErrorMessage(500,
                    "An error occurred while retrieving entities. Please try again later."
                    ));
            }
        }

        public enum TipoDataEntrada
        {
            TDE_DataEntrada = 0,
            TDE_DataEmissao = 1,
            TDE_DataSaida = 2
        }

        public enum TipoPeriodoEntrada
        {
            TPE_Geral = 0,
            TPE_Periodo = 1,
            TPE_AteData = 2
        }

        [HttpGet("CreateReportEntrada", Name = nameof(CreateReportEntrada))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateReportEntrada()
        {
            var projectRootPath = Environment.CurrentDirectory;
            var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcEntrada.frx");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(projectRootPath, "reports")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(projectRootPath, "reports"));
            }
            var freport = new FastReport.Report();
            //Entrada
            //Produtos Entrada
            //Empresa
            Entrada? saida = await context.Entradas.
                Include(p => p.Fornecedor).
                Include(p => p.ProdutoEntrada)
                .Include(p => p.CdEmpresaNavigation)
                .OrderByDescending(p => p.Nr)
                .LastOrDefaultAsync();

            if (saida == null)
            {
                return BadRequest();
            }
            var EntradaMapped = mapper.Map<EntradaGetDto>(saida);
            var EmpresaMapper = mapper.Map<EmpresaGetDto>(saida.CdEmpresaNavigation);
            var ProdutosDaEntrada = mapper.Map<List<ProdutoEntradaGetDto>>(saida.ProdutoEntrada);

            // Registra os objetos no dicionário do relatório
            freport.Dictionary.RegisterBusinessObject(new List<EmpresaGetDto>() { EmpresaMapper }, "Empresa", 1, true);
            freport.Dictionary.RegisterBusinessObject(new List<EntradaGetDto>() { EntradaMapped }, "Entrada", 1, true);
            freport.Dictionary.RegisterBusinessObject(ProdutosDaEntrada, "ProdutoEntrada", 1, true);

            // Habilita os datasources
            freport.GetDataSource("Empresa").Enabled = true;
            freport.GetDataSource("Entrada").Enabled = true;
            freport.GetDataSource("ProdutoEntrada").Enabled = true;

            freport.Report.Save(reportFilePath);

            return Ok($" Relatorio gerado : {reportFilePath}");
        }

        [HttpGet("GetEntradaReport", Name = nameof(GetEntradaReport))]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEntradaReport([FromQuery] int nrLanc)
        {
            try
            {
                var projectRootPath = Environment.CurrentDirectory;
                var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcEntrada.frx");

                if (!System.IO.File.Exists(reportFilePath))
                {
                    return NotFound("Template de relatório não encontrado no servidor.");
                }

                var entrada = await context.Entradas.Include(p => p.Fornecedor)
                    .Include(p => p.ProdutoEntrada).ThenInclude(prods => prods.ProdutoEstoque)
                    .Include(p => p.CdEmpresaNavigation).ThenInclude(cidade => cidade.CdCidadeNavigation)
                    .FirstOrDefaultAsync(s => s.Nr == nrLanc);
                _calculationService.CalculateTotals(entrada);

                if (entrada == null)
                {
                    return NotFound($"Nenhuma saída encontrada para o NrLanc = {nrLanc}.");
                }

                var EntradaMapped = mapper.Map<EntradaGetDto>(entrada);
                var EmpresaMapped = mapper.Map<EmpresaGetDto>(entrada.CdEmpresaNavigation);
                var ProdutosDaEntrada = mapper.Map<List<ProdutoEntradaGetDto>>(entrada.ProdutoEntrada);

                using var freport = new FastReport.Report();
                freport.Load(reportFilePath);

                freport.RegisterData(new List<EmpresaGetDto>() { EmpresaMapped }, "Empresa");
                freport.RegisterData(new List<EntradaGetDto>() { EntradaMapped }, "Entrada");
                freport.RegisterData(ProdutosDaEntrada, "ProdutoEntrada");

                freport.GetDataSource("Empresa").Enabled = true;
                freport.GetDataSource("Entrada").Enabled = true;
                freport.GetDataSource("ProdutoEntrada").Enabled = true;

                freport.Prepare();

                using var ms = new MemoryStream();
                var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                freport.Export(pdfExport, ms);
                ms.Position = 0;

                return File(ms.ToArray(), "application/pdf", $"Entrada_{nrLanc}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao gerar relatório da saída {NrLanc}", nrLanc);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao gerar o relatório.");
            }
        }
    }
}
