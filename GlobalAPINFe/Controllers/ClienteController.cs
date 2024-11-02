using AutoMapper;
using FastReport.Export.PdfSimple;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : GenericPagedController<Cliente, int, ClienteDto>
    {
        private readonly IMapper mapper;

        public ClienteController(IQueryRepository<Cliente, int, ClienteDto> repo, ILogger<GenericPagedController<Cliente, int, ClienteDto>> logger, IMapper mapper) : base(repo, logger)
        {
            this.mapper = mapper;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Cliente>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Cliente>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cliente), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cliente>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cliente), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Cliente>> Create([FromBody] ClienteDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cliente), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cliente>> Update(int id, [FromBody] ClienteDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Cliente>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Cliente>>> CreateBulk([FromBody] IEnumerable<ClienteDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        // Métodos personalizados ajustados

        [HttpGet("GetClientePorEmpresa", Name = nameof(GetClientePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Cliente>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Cliente>>> GetClientePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmCliente = null,
            [FromQuery] int? id = null,
            [FromQuery] string? cpfCnpj = null,
            [FromQuery] string? ie = null)
        {
            try
            {
                var query = ((ClientePagedRepositoyDto)repo).GetClientePorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmCliente))
                {
                    var normalizedNmCliente = UtlStrings.RemoveDiacritics(nmCliente.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmBairro == null) ? "" : p.NmCliente.ToLower()).Contains(normalizedNmCliente));
                }

                if (id.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.Id == id.Value);
                }

                if (!string.IsNullOrEmpty(cpfCnpj))
                {
                    var normalizeCpfCnpj = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cpfCnpj.ToLower().Trim()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.NrDoc == null) ? "" : p.NrDoc.ToLower().Trim())) == normalizeCpfCnpj);
                }

                if (!string.IsNullOrEmpty(ie))
                {
                    var normalizedIe = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(ie.ToLower().Trim()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.InscricaoEstadual == null) ? "" : p.InscricaoEstadual.ToLower().Trim())) == normalizedIe);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Cliente>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }

                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpGet("CreateReport", Name = nameof(CreateReport))]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateReport()
        {
            var projectRootPath = Environment.CurrentDirectory;
            var reportFilePath = System.IO.Path.Combine(projectRootPath, "reports", "ReportMvc.frx");
            if (!System.IO.Directory.Exists(System.IO.Path.Combine(projectRootPath, "reports")))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(projectRootPath, "reports"));
            }
            var freport = new FastReport.Report();
            var query = await this.repo.RetrieveAllAsync();

            var lista = query.ToList();

            List<ClienteDto> listaDto = new List<ClienteDto>();
            foreach (var item in lista)
            {
                listaDto.Add(this.mapper.Map<ClienteDto>(item));
            }
            var lll = listaDto.Slice(0, 10);

            freport.Dictionary.RegisterBusinessObject(lll, "clientList", 10, true);
            freport.Report.Save(reportFilePath);

            return Ok($" Relatorio gerado : {reportFilePath}");
        }

        [HttpGet("ClienteReport", Name = nameof(ClienteReport))]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ClienteReport(
            int idEmpresa,
            [FromQuery] string? nmCliente = null,
            [FromQuery] int? id = null,
            [FromQuery] string? cpfCnpj = null,
            [FromQuery] string? ie = null)
        {
            var reportFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "reports", "ReportMvc.frx");
            using var report = new FastReport.Report();
            report.Load(reportFilePath);

            var query = ((ClientePagedRepositoyDto)repo).GetClientePorEmpresa(idEmpresa).Result.AsQueryable();

            if (query == null)
            {
                return NotFound("Entities not found.");
            }

            var filteredQuery = query.AsEnumerable();

            if (!string.IsNullOrEmpty(nmCliente))
            {
                var normalizedNmCliente = UtlStrings.RemoveDiacritics(nmCliente.ToLower());
                filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmBairro == null) ? "" : p.NmCliente.ToLower()).Contains(normalizedNmCliente));
            }

            if (id.HasValue)
            {
                filteredQuery = filteredQuery.Where(p => p.Id == id.Value);
            }

            if (!string.IsNullOrEmpty(cpfCnpj))
            {
                var normalizeCpfCnpj = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cpfCnpj.ToLower().Trim()));
                filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.NrDoc == null) ? "" : p.NrDoc.ToLower().Trim())) == normalizeCpfCnpj);
            }

            if (!string.IsNullOrEmpty(ie))
            {
                var normalizedIe = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(ie.ToLower().Trim()));
                filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.InscricaoEstadual == null) ? "" : p.InscricaoEstadual.ToLower().Trim())) == normalizedIe);
            }

            filteredQuery = filteredQuery.OrderBy(p => p.Id);

            var listaDto = filteredQuery.Select(mapper.Map<ClienteDto>).ToList();
            report.Dictionary.RegisterBusinessObject(listaDto, "clientList", 10, true);

            report.Prepare();

            using var pdfExport = new PDFSimpleExport();
            using var ms = new MemoryStream();
            pdfExport.Export(report, ms);
            ms.Flush();
            ms.Position = 0;

            return File(ms.ToArray(), "application/pdf", "ClienteReport.pdf");
        }

        [HttpGet("GetClienteByName/{idEmpresa}/{nome}")]
        [ProducesResponseType(typeof(IEnumerable<Cliente>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClienteByName(int idEmpresa, string nome)
        {
            var clientes = await (repo as ClientePagedRepositoyDto).GetClientePorEmpresa(idEmpresa);

            var clientesList = clientes.ToList();
            if (clientesList == null)
            {
                return NotFound("Cliente não encontrada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = clientesList.Where(c => UtlStrings.RemoveDiacritics(c.NmCliente.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmCliente)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("clientes não encontrada.");
            }
            return Ok(filter);
        }
    }
}
