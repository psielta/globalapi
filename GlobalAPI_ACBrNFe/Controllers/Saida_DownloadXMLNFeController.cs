using AutoMapper;
using GlobalAPI_ACBrNFe.Lib.ACBr.NFe;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace GlobalAPI_ACBrNFe.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Saida_DownloadXMLNFeController : ControllerBase
    {
        private readonly ILogger<Saida_GerarValidarNFeController> _logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;
        protected GlobalErpFiscalBaseContext db;
        private readonly IConfiguration _config;
        private readonly NFeGlobalService nFeGlobalService;
        public Saida_DownloadXMLNFeController(IConfiguration config,
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

        [HttpGet("{id}")]
        [Produces("application/xml")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Saida? saida = await db.Saidas.FindAsync(id);
            if (saida == null || saida.XmNf == null || saida.XmNf.Length == 0)
            {
                return NotFound();
            }

            string contentType = "application/xml";
            string fileName = $"saida_{id}.xml";
            Response.Headers[HeaderNames.ContentDisposition] = new ContentDispositionHeaderValue("inline")
            {
                FileName = fileName
            }.ToString();

            return File(Encoding.UTF8.GetBytes(saida.XmNf ?? ""), contentType);
        }
    }
}
