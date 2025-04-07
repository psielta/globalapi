using GlobalErpData.Data;
using GlobalErpData.Models;
using GlobalLib.Utils;
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
        public async Task<ActionResult<DashboardEstoqueTotalEntradas>> GetDashboardEstoqueTotalEntradas(int id, int? month = null, int? year = null, int? idEmpresa = 1)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalEntradas(id, month, year,idEmpresa).FirstOrDefaultAsync();

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
        public async Task<ActionResult<DashboardEstoqueTotalSaidas>> GetDashboardEstoqueTotalSaidas(int id, int? month = null, int? year = null, int? idEmpresa = 1)
        {
            try
            {
                var resultado = await _context.GetDashboardEstoqueTotalSaidas(id, month, year,idEmpresa).FirstOrDefaultAsync();

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
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalSaidasPorMes>>> GetDashboardEstoqueTotalSaidasPorMes(int id, int? idEmpresa = 1)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalSaidasPorMes(id,idEmpresa).ToListAsync();

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
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalEntradasPorMes>>> GetDashboardEstoqueTotalEntradasPorMes(int id, int? idEmpresa = 1)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalEntradasPorMes(id, idEmpresa).ToListAsync();

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
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalEntradasPorDia>>> GetDashboardEstoqueTotalEntradasPorDia(int id, int? idEmpresa = 1)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalEntradasPorDia(id, idEmpresa).ToListAsync();

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
        public async Task<ActionResult<IEnumerable<DashboardEstoqueTotalSaidasPorDia>>> GetDashboardEstoqueTotalSaidasPorDia(int id, int? idEmpresa = 1)
        {
            try
            {
                var resultados = await _context.GetDashboardEstoqueTotalSaidasPorDia(id, idEmpresa).ToListAsync();

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

        [HttpGet("GetTotalPorGrupo/{id}")]
        [ProducesResponseType(typeof(IEnumerable<TotalPorGrupo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<TotalPorGrupo>>> GetTotalPorGrupo(int id, int? idEmpresa = 1)
        {
            try
            {
                var resultados = await _context.GetTotalPorGrupo(id, idEmpresa).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para o ID {Id}", id);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter total por grupo para o ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("NfceGetTotalDia/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<TotalDiaResult>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<TotalDiaResult>>> NfceGetTotalDia(int unity, int? empresa = -1, DateOnly? data = null)
        {
            try
            {

                var resultados = await _context.GetTotalDia(unity, empresa ?? -1, data).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para a unidade {Unity}", unity);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total por dia para a unidade {Unity}", unity);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("NfceGetTotalPeriodo/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<TotalPeriodoResult>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<TotalPeriodoResult>>> NfceGetTotalPeriodo(
            int unity,
            DateOnly dataInicial,
            DateOnly dataFinal,
            int? empresa = -1)
        {
            try
            {
                var resultados = await _context.GetTotalPeriodo(unity, dataInicial, dataFinal, empresa ?? -1).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum resultado encontrado para a unidade {Unity} no período de {DataInicial} a {DataFinal}",
                        unity, dataInicial, dataFinal);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter o total por período para a unidade {Unity}", unity);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("NfceGetFormasPagamento/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<FormaPagamentoResult>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FormaPagamentoResult>>> NfceGetFormasPagamento(
            int unity,
            DateOnly data,
            int? empresa = -1)
        {
            try
            {

                var resultados = await _context.GetFormasPagamento(unity, data, empresa ?? -1).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhuma forma de pagamento encontrada para a unidade {Unity} na data {Data}",
                        unity, data);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter formas de pagamento para a unidade {Unity}", unity);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }

        [HttpGet("NfceGetProduto5MaisVendidos/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<Produto5MaisVendidosResult>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Produto5MaisVendidosResult>>> NfceGetProduto5MaisVendidos(
            int unity,
            DateOnly data,
            int? empresa = -1)
        {
            try
            {
                var resultados = await _context.GetProduto5MaisVendidos(unity, data, empresa ?? -1).ToListAsync();

                if (resultados == null || !resultados.Any())
                {
                    _log.LogWarning("Nenhum produto encontrado para a unidade {Unity} na data {Data}",
                        unity, data);
                    return NotFound();
                }

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao obter os 5 produtos mais vendidos para a unidade {Unity}", unity);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a solicitação.");
            }
        }
    }
}
