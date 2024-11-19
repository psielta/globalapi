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
        public async Task<ActionResult<DashboardEstoqueTotalEntradas>> GetDashboardEstoqueTotalEntradas(int id, int? month = null, int? year = null)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalEntradas(id, month, year).FirstOrDefaultAsync();

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
        public async Task<ActionResult<DashboardEstoqueTotalSaidas>> GetDashboardEstoqueTotalSaidas(int id, int? month = null, int? year = null)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalSaidas(id, month, year).FirstOrDefaultAsync();

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

        [HttpGet("GetDashboardEstoqueTotalSaidasPorMes/{id}")]
        [ProducesResponseType(typeof(IEnumerable<DashboardEstoqueTotalSaidasPorMes>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalSaidasPorMes>>> GetDashboardEstoqueTotalSaidasPorMes(int id)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalSaidasPorMes(id).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de saídas por mês para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("GetDashboardEstoqueTotalEntradasPorMes/{id}")]
        [ProducesResponseType(typeof(IEnumerable<DashboardEstoqueTotalEntradasPorMes>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalEntradasPorMes>>> GetDashboardEstoqueTotalEntradasPorMes(int id)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalEntradasPorMes(id).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de entradas por mês para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("GetDashboardEstoqueTotalEntradasPorDia/{id}")]
        [ProducesResponseType(typeof(IEnumerable<DashboardEstoqueTotalEntradasPorDia>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalEntradasPorDia>>> GetDashboardEstoqueTotalEntradasPorDia(int id)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalEntradasPorDia(id).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de entradas por dia para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("GetDashboardEstoqueTotalSaidasPorDia/{id}")]
        [ProducesResponseType(typeof(IEnumerable<DashboardEstoqueTotalSaidasPorDia>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalSaidasPorDia>>> GetDashboardEstoqueTotalSaidasPorDia(int id)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalSaidasPorDia(id).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total de saídas por dia para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }
    }
}
