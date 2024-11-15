using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReimprimirController : ControllerBase
    {
        private readonly ILogger<GerarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public ReimprimirController(IConfiguration config,
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

        [HttpGet("{id}")]
        [Produces("application/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Saida? saida = await db.Saidas.FindAsync(id);
            if (saida == null || saida.Pdf == null)
            {
                return NotFound();
            }

            string contentType = "application/pdf";
            string fileName = $"saida_{id}.pdf";
            Response.Headers[HeaderNames.ContentDisposition] = new ContentDispositionHeaderValue("inline")
            {
                FileName = fileName
            }.ToString();

            return File(saida.Pdf, contentType);
        }
    }
}
