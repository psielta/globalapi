using GlobalErpData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Linq.Expressions;
using GlobalErpData.Models;
using System.Reflection;

namespace GlobalAPINFe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConexaoController : Controller
    {
        private readonly GlobalErpFiscalBaseContext _context;
        public ConexaoController(GlobalErpFiscalBaseContext context)
        {
            _context = context;
        }
        [HttpGet("RunSQL")]
        public async Task<IActionResult> RunSQL([FromQuery] string sql)
        {
            throw new NotImplementedException();
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(sql);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Consulta")]
        public async Task<IActionResult> Consulta([FromQuery] string sql)
        {
            throw new NotImplementedException();
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        var resultList = new List<Dictionary<string, object>>();

                        while (await result.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                row[result.GetName(i)] = result.GetValue(i);
                            }

                            resultList.Add(row);
                        }

                        var json = JsonConvert.SerializeObject(resultList);
                        return Ok(json);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ConsultaV2")]
        public async Task<IActionResult> ConsultaV2([FromQuery] string sql, [FromQuery] string entityType = null)
        {
            throw new NotImplementedException();
            try
            {
                if (!string.IsNullOrEmpty(entityType))
                {
                    var type = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.Name == entityType && t.Namespace == "GlobalErpData.Models");

                    if (type != null)
                    {
                        var method = typeof(ConexaoController).GetMethod(nameof(ExecuteSqlForEntity), BindingFlags.NonPublic | BindingFlags.Instance);
                        var genericMethod = method.MakeGenericMethod(type);
                        var task = (Task<IActionResult>)genericMethod.Invoke(this, new object[] { sql });
                        return await task;
                    }
                }
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        var resultList = new List<Dictionary<string, object>>();

                        while (await result.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();

                            for (int i = 0; i < result.FieldCount; i++)
                            {
                                row[result.GetName(i)] = result.GetValue(i);
                            }

                            resultList.Add(row);
                        }

                        // Serializa os resultados para JSON
                        var json = JsonConvert.SerializeObject(resultList);
                        return Ok(json);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<IActionResult> ExecuteSqlForEntity<TEntity>(string sql) where TEntity : class
        {
            throw new NotImplementedException();
            var results = await _context.Set<TEntity>().FromSqlRaw(sql).ToListAsync();
            var json = JsonConvert.SerializeObject(results);
            return Ok(json);
        }

    }
}
