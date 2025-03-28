﻿using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using GlobalLib.Dto;
using X.PagedList.Extensions;
using GlobalErpData.Services;
using AutoMapper;
using GlobalErpData.Data;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace GlobalAPINFe.Controllers
{
    public class SaidaController : GenericPagedController<Saida, int, SaidaDto>
    {
        private readonly SaidaCalculationService _calculationService;
        private readonly IMapper mapper;
        private readonly GlobalErpFiscalBaseContext context;
        public SaidaController(IQueryRepository<Saida, int, SaidaDto> repo,
            ILogger<GenericPagedController<Saida, int, SaidaDto>> logger,
            SaidaCalculationService calculationService, IMapper mapper, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            _calculationService = calculationService;
            this.mapper = mapper;
            context = _context;
        }

        [HttpPost("/GetXML/{unity}")]
        [Produces("application/zip")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetXML([FromRoute] int unity, [FromBody] GetXmlDto data)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("SELECT * FROM saidas s");
            sb.AppendLine($"WHERE s.unity = {unity} AND s.tp_saida <> 'A' ");

            if (data.Empresas != null && data.Empresas.Count > 0)
                sb.AppendLine($"AND s.empresa IN ({UtlStrings.CommaText<int>(data.Empresas)})");

            if (data.Periodo != null)
            {
                var dataInicioStr = UtlStrings.QuotedStr(UtlStrings.DateToStr(data.Periodo.Inicio, "yyyy-MM-dd"));
                var dataFimStr = UtlStrings.QuotedStr(UtlStrings.DateToStr(data.Periodo.Fim, "yyyy-MM-dd"));
                sb.AppendLine($"AND s.data BETWEEN {dataInicioStr} AND {dataFimStr}");
            }

            if (data.Situacoes != null && data.Situacoes.Count > 0)
                sb.AppendLine($"AND s.cd_situacao IN ({UtlStrings.CommaText(data.Situacoes, '\'')})");

            var sql = sb.ToString();

            var saidas = await context.Saidas
                .FromSqlRaw(sql)
                .Include(s => s.EmpresaNavigation)
                .ToListAsync();

            if (saidas == null || saidas.Count == 0)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Não encontrado",
                    Detail = "Nenhuma NF encontrada para os filtros informados."
                });
            }

            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var saida in saidas)
                {
                    if (string.IsNullOrEmpty(saida.XmNf))
                        continue;
                    var empresaNome = saida.EmpresaNavigation?.NmEmpresa ?? "EmpresaDesconhecida";
                    var ano = saida.Data?.Year ?? 0;
                    var mes = saida.Data?.Month ?? 0;
                    string folderPath = $"Empresa {saida.Empresa} - {empresaNome}/{ano:D4}/{mes:D2}/";

                    var chave = !string.IsNullOrWhiteSpace(saida.ChaveAcessoNfe) ?
                        saida.ChaveAcessoNfe :
                        $"Saida-{saida.NrLanc}";
                    var fileName = $"{chave}.xml";

                    var entry = zipArchive.CreateEntry($"{folderPath}{fileName}", CompressionLevel.Optimal);
                    using var entryStream = entry.Open();
                    using var writer = new StreamWriter(entryStream);
                    writer.Write(saida.XmNf ?? "");
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            // "application/zip" conforme anotamos acima
            // FileContentResult: retorne os bytes do ZIP
            return File(memoryStream.ToArray(), "application/zip", "NotasFiscais.zip");
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Saida>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Saida>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Saida), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Saida>> Create([FromBody] SaidaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Saida), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Saida>> Update(int id, [FromBody] SaidaDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Saida>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Saida>>> CreateBulk([FromBody] IEnumerable<SaidaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpGet("GetSaidaPorEmpresa", Name = nameof(GetSaidaPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Saida>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Saida>>> GetSaidaPorEmpresa(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int tipoPeriodoSaida = 0,
            [FromQuery] int tipoDataSaida = 0,
            [FromQuery] string? periodoInicial = null,
            [FromQuery] string? periodoFinal = null,
            [FromQuery] string? nmCliente = null,
            [FromQuery] int? nrLanc = null,
            [FromQuery] int? cdEmpresa = null,
            [FromQuery] string? nrNotaFiscal = null,
            [FromQuery] string? serie = null,
            [FromQuery] string? chaveAcesso = null,
            [FromQuery] string? tipoSaida = null,
            [FromQuery] string? cdSituacao = null

        )
        {
            try
            {
                TipoDataSaida ENUM_tipoDataSaida = TipoDataSaida.TDS_DataSaida;
                TipoPeriodoSaida ENUM_tipoPeriodoSaida = TipoPeriodoSaida.TPS_Geral;

                if (tipoDataSaida < 0 || tipoDataSaida > 2 || tipoPeriodoSaida < 0 || tipoPeriodoSaida > 2)
                {
                    return BadRequest(new ErrorMessage(500, "Parâmetros inválidos"));
                }

                ENUM_tipoDataSaida = (TipoDataSaida)tipoDataSaida;
                ENUM_tipoPeriodoSaida = (TipoPeriodoSaida)tipoPeriodoSaida;

                var query = await ((SaidaRepository)repo).GetSaidaAsyncPorUnity(unity);

                if (query == null)
                {
                    return NotFound(new ErrorMessage(404, "Entidades não encontradas"));
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmCliente))
                {
                    var normalizedNmCliente = UtlStrings.RemoveDiacritics(nmCliente.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmCliente == null) ? "" : p.NmCliente.ToLower()).Contains(normalizedNmCliente));
                }

                if (nrLanc.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrLanc == nrLanc.Value);
                }

                if (cdEmpresa.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.Empresa == cdEmpresa.Value);
                }

                if (!string.IsNullOrEmpty(nrNotaFiscal))
                {
                    filteredQuery = filteredQuery.Where(p => p.NrNotaFiscal != null && p.NrNotaFiscal.Equals(nrNotaFiscal));
                }

                if (!string.IsNullOrEmpty(serie))
                {
                    filteredQuery = filteredQuery.Where(p => p.SerieNf != null && p.SerieNf.Equals(serie));
                }

                if (!string.IsNullOrEmpty(chaveAcesso))
                {
                    var normalizedChaveAcesso = UtlStrings.RemoveDiacritics(chaveAcesso.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.ChaveAcessoNfe == null) ? "" : p.ChaveAcessoNfe.ToLower()) == normalizedChaveAcesso);
                }

                if (!string.IsNullOrEmpty(tipoSaida))
                {
                    var normalizedTipoSaida = UtlStrings.RemoveDiacritics(tipoSaida.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.TpSaida ?? "").ToUpper() == normalizedTipoSaida);
                }
                if (!string.IsNullOrEmpty(cdSituacao))
                {
                    var normalizedCdSituacao = UtlStrings.RemoveDiacritics(cdSituacao.ToUpper()).Trim();
                    filteredQuery = filteredQuery.Where(p => (p.CdSituacao ?? "").ToUpper() == normalizedCdSituacao);
                }

                switch ((int)ENUM_tipoPeriodoSaida)
                {
                    case (int)TipoPeriodoSaida.TPS_Periodo:
                        if (!string.IsNullOrEmpty(periodoInicial) && !string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtInicial = DateOnly.ParseExact(periodoInicial, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataSaida)
                            {
                                case (int)TipoDataSaida.TDS_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida >= dtInicial && p.DtSaida <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_Data:
                                    filteredQuery = filteredQuery.Where(p => p.Data >= dtInicial && p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_DataPedido:
                                    filteredQuery = filteredQuery.Where(p => p.DtPedido >= dtInicial && p.DtPedido <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case (int)TipoPeriodoSaida.TPS_AteData:
                        if (!string.IsNullOrEmpty(periodoFinal))
                        {
                            DateOnly dtFinal = DateOnly.ParseExact(periodoFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            switch ((int)ENUM_tipoDataSaida)
                            {
                                case (int)TipoDataSaida.TDS_DataSaida:
                                    filteredQuery = filteredQuery.Where(p => p.DtSaida <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_Data:
                                    filteredQuery = filteredQuery.Where(p => p.Data <= dtFinal);
                                    break;
                                case (int)TipoDataSaida.TDS_DataPedido:
                                    filteredQuery = filteredQuery.Where(p => p.DtPedido <= dtFinal);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                foreach (var saida in filteredQuery)
                {
                    _calculationService.CalculateTotals(saida);
                }
                filteredQuery = filteredQuery.OrderByDescending(p => p.NrLanc);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Saida>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound(new ErrorMessage(404, "Entidades não encontradas."));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar as entidades paginadas.");
                return StatusCode(500, new ErrorMessage(500,
                    "Ocorreu um erro ao recuperar as entidades. Por favor, tente novamente mais tarde."
                    ));
            }
        }

        public enum TipoDataSaida
        {
            TDS_DataSaida = 0,
            TDS_Data = 1,
            TDS_DataPedido = 2
        }

        public enum TipoPeriodoSaida
        {
            TPS_Geral = 0,
            TPS_Periodo = 1,
            TPS_AteData = 2
        }
        [HttpGet("CreateReportSaida", Name = nameof(CreateReportSaida))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateReportSaida()
        {
            var projectRootPath = Environment.CurrentDirectory;
            var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcSaida.frx");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(projectRootPath, "reports")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(projectRootPath, "reports"));
            }
            var freport = new FastReport.Report();
            //Saida
            //Produtos Saida
            //Empresa
            Saida? saida = await context.Saidas.
                Include(p => p.ClienteNavigation).
                Include(p => p.ProdutoSaida)
                .Include(p => p.EmpresaNavigation)
                .OrderByDescending(p => p.NrLanc)
                .LastOrDefaultAsync();

            if (saida == null)
            {
                return BadRequest();
            }
            var SaidaMapped = mapper.Map<SaidaGetDto>(saida);
            var EmpresaMapper = mapper.Map<EmpresaGetDto>(saida.EmpresaNavigation);
            var ProdutosDaSaida = mapper.Map<List<ProdutoSaidum>>(saida.ProdutoSaida);

            // Registra os objetos no dicionário do relatório
            freport.Dictionary.RegisterBusinessObject(new List<EmpresaGetDto>() { EmpresaMapper }, "Empresa", 1, true);
            freport.Dictionary.RegisterBusinessObject(new List<SaidaGetDto>() { SaidaMapped }, "Saida", 1, true);
            freport.Dictionary.RegisterBusinessObject(ProdutosDaSaida, "ProdutoSaida", 1, true);

            // Habilita os datasources
            freport.GetDataSource("Empresa").Enabled = true;
            freport.GetDataSource("Saida").Enabled = true;
            freport.GetDataSource("ProdutoSaida").Enabled = true;

            freport.Report.Save(reportFilePath);

            return Ok($" Relatorio gerado : {reportFilePath}");
        }

        [HttpGet("GetSaidaReport", Name = nameof(GetSaidaReport))]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSaidaReport([FromQuery] int nrLanc)
        {
            try
            {
                var projectRootPath = Environment.CurrentDirectory;
                var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcSaida.frx");

                if (!System.IO.File.Exists(reportFilePath))
                {
                    return NotFound("Template de relatório não encontrado no servidor.");
                }

                var saida = await context.Saidas.Include(p => p.ClienteNavigation)
                    .Include(p => p.ProdutoSaida).ThenInclude(prods => prods.ProdutoEstoque)
                    .Include(p => p.EmpresaNavigation).ThenInclude(cidade => cidade.CdCidadeNavigation)
                    .FirstOrDefaultAsync(s => s.NrLanc == nrLanc);
                _calculationService.CalculateTotals(saida);

                if (saida == null)
                {
                    return NotFound($"Nenhuma saída encontrada para o NrLanc = {nrLanc}.");
                }

                var SaidaMapped = mapper.Map<SaidaGetDto>(saida);
                var EmpresaMapped = mapper.Map<EmpresaGetDto>(saida.EmpresaNavigation);
                var ProdutosDaSaida = mapper.Map<List<ProdutoSaidum>>(saida.ProdutoSaida);

                using var freport = new FastReport.Report();
                freport.Load(reportFilePath);

                freport.RegisterData(new List<EmpresaGetDto>() { EmpresaMapped }, "Empresa");
                freport.RegisterData(new List<SaidaGetDto>() { SaidaMapped }, "Saida");
                freport.RegisterData(ProdutosDaSaida, "ProdutoSaida");

                freport.GetDataSource("Empresa").Enabled = true;
                freport.GetDataSource("Saida").Enabled = true;
                freport.GetDataSource("ProdutoSaida").Enabled = true;

                freport.Prepare();

                using var ms = new MemoryStream();
                var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                freport.Export(pdfExport, ms);
                ms.Position = 0;

                return File(ms.ToArray(), "application/pdf", $"Saida_{nrLanc}.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao gerar relatório da saída {NrLanc}", nrLanc);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao gerar o relatório.");
            }
        }

        [HttpGet("CreateSaidaReportGeral", Name = nameof(CreateSaidaReportGeral))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateSaidaReportGeral()
        {
            var projectRootPath = Environment.CurrentDirectory;
            var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcSaidaGeral.frx");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(projectRootPath, "reports")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(projectRootPath, "reports"));
            }
            var freport = new FastReport.Report();
            //Saida
            //Produtos Saida
            //Empresa
            Saida? saida = await context.Saidas.
                Include(p => p.ClienteNavigation).
                Include(p => p.ProdutoSaida)
                .Include(p => p.EmpresaNavigation)
                .Include(u => u.UnityNavigation)
                .OrderByDescending(p => p.NrLanc)
                .LastOrDefaultAsync();

            if (saida == null)
            {
                return BadRequest();
            }
            var SaidaMapped = mapper.Map<SaidaGetDto>(saida);
            var EmpresaMapper = mapper.Map<EmpresaGetDto>(saida.EmpresaNavigation);
            var unityMapper = mapper.Map<UnityGetDto>(saida.UnityNavigation);
            // Registra os objetos no dicionário do relatório
            freport.Dictionary.RegisterBusinessObject(new List<SaidaGetDto>() { SaidaMapped }, "Saida", 1, true);
            freport.Dictionary.RegisterBusinessObject(new List<UnityGetDto>() { unityMapper }, "Unity", 1, true);

            // Habilita os datasources
            freport.GetDataSource("Saida").Enabled = true;
            freport.GetDataSource("Unity").Enabled = true;

            freport.Report.Save(reportFilePath);

            return Ok($" Relatorio gerado : {reportFilePath}");
        }

        [HttpGet("GetSaidaGeralReport")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSaidaGeralReport([FromQuery] int unity, [FromQuery] int? Empresa = null,
            [FromQuery] int? cliente = null, [FromQuery] string? dataInicialISO = null, [FromQuery] string? dataFinalISO = null,
            [FromQuery] string? cdSituacao = null, [FromQuery] string? tpSaida = null)
        {
            try
            {
                var Sql = $@"
Select * from saidas s
where
s.unity = {unity}
{((Empresa.HasValue) ? ($" and s.empresa = {Empresa} ") : (""))}
{((cliente.HasValue) ? ($" and s.cliente = {cliente} ") : (""))}
{(((!string.IsNullOrEmpty(dataFinalISO)) && (!string.IsNullOrEmpty(dataFinalISO))) ? ($" and s.dt_saida between '{dataInicialISO}' and '{dataFinalISO}' ") : (""))}
{((!string.IsNullOrEmpty(cdSituacao)) ? ($" and s.cd_situacao = {UtlStrings.QuotedStr(cdSituacao)} ") : (""))}
{((!string.IsNullOrEmpty(tpSaida)) ? ($" and s.tp_saida = {UtlStrings.QuotedStr(tpSaida)} ") : (""))}
";
                var projectRootPath = Environment.CurrentDirectory;
                var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvcSaidaGeral.frx");

                if (!System.IO.File.Exists(reportFilePath))
                {
                    return NotFound("Template de relatório não encontrado no servidor.");
                }

                var saidas = await context.Saidas.FromSqlRaw(Sql).Include(p => p.ClienteNavigation)
                    .Include(p => p.ProdutoSaida).ThenInclude(prods => prods.ProdutoEstoque)
                    .Include(p => p.EmpresaNavigation).ThenInclude(cidade => cidade.CdCidadeNavigation)
                    .ToListAsync();
                if (saidas == null || saidas.Count <= 0)
                {
                    return NotFound($"Nenhuma saída encontrada");
                }
                List<SaidaGetDto> SaidaMapped = new List<SaidaGetDto>();
                foreach (var item in saidas)
                {
                    _calculationService.CalculateTotals(item);
                    SaidaMapped.Add(mapper.Map<SaidaGetDto>(item));
                }

                Unity? unityobj = await context.Unities.FindAsync(unity);
                if (unityobj == null)
                    return NotFound($"NotFound Unity");
                UnityGetDto UnityMapped = mapper.Map<UnityGetDto>(unityobj);

                using var freport = new FastReport.Report();
                freport.Load(reportFilePath);

                freport.RegisterData(new List<UnityGetDto>() { UnityMapped }, "Unity");
                freport.RegisterData(SaidaMapped, "Saida");

                freport.GetDataSource("Unity").Enabled = true;
                freport.GetDataSource("Saida").Enabled = true;

                freport.Prepare();

                using var ms = new MemoryStream();
                var pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                freport.Export(pdfExport, ms);
                ms.Position = 0;

                return File(ms.ToArray(), "application/pdf", $"Saida_Geral.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao gerar relatório da saída geral");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao gerar o relatório.");
            }
        }

    }
}
