using System.Text;
using GlobalAPI_ACBrNFe.Services;
using GlobalErpData.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GlobalAPI_ACBrNFe.Controllers
{
    /// <summary>
    /// Controlador responsável por gerar e retornar o arquivo Sintegra.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SintegraController : ControllerBase
    {
        private readonly SintegraService _sintegraService;
        private readonly ILogger<SintegraController> _logger;

        public SintegraController(
            SintegraService sintegraService,
            ILogger<SintegraController> logger)
        {
            _sintegraService = sintegraService;
            _logger = logger;
        }

        [HttpPost("{id}")]
        [Produces("text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetSintegra(
            [FromRoute] int id,
            [FromBody] SintegraDto sintegraDto)
        {
            try
            {
                var sintegraTxt = await _sintegraService.Core(sintegraDto);

                byte[] fileBytes = Encoding.UTF8.GetBytes(sintegraTxt);
                string fileName = $"Sintegra_{id}.txt";

                return File(fileBytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar Sintegra (ID: {Id})", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
