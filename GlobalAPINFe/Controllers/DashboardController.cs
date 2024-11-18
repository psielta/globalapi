using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly ILogger<DashboardController> _log;

        public DashboardController(GlobalErpFiscalBaseContext context, ILogger<DashboardController> log)
        {
            _context = context;
            _log = log;
        }

        [HttpGet("GetDashboardEstoqueTotalEntradas/{id}")]
        [ProducesResponseType(typeof(DashboardEstoqueTotalEntradas), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DashboardEstoqueTotalEntradas>> GetDashboardEstoqueTotalEntradas(int id)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalEntradas(id).FirstOrDefaultAsync();

                if (resultado == null)
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de entradas para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("GetDashboardEstoqueTotalSaidas/{id}")]
        [ProducesResponseType(typeof(DashboardEstoqueTotalSaidas), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DashboardEstoqueTotalSaidas>> GetDashboardEstoqueTotalSaidas(int id)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalSaidas(id).FirstOrDefaultAsync();

                if (resultado == null)
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de saídas para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }
    }
}
