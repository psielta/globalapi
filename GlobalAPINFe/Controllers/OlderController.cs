using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlderController : GenericPagedController<Older, Guid, OlderDto>
    {
        private readonly IDbContextFactory<GlobalErpFiscalBaseContext> dbContextFactory;
        private readonly IMapper _mapper;
        public OlderController(IQueryRepository<Older, Guid, OlderDto> repo, 
            ILogger<GenericPagedController<Older, Guid, OlderDto>> logger, IMapper mapper,
            IDbContextFactory<GlobalErpFiscalBaseContext> context) : base(repo, logger)
        {
            this._mapper = mapper;
            dbContextFactory = context;
        }

        [HttpGet("GetOlderPorEmpresa", Name = nameof(GetOlderPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOlderPorEmpresa(
            int idEmpresa,
            [FromQuery] Guid? id = null,
            [FromQuery] string? customerName = null,
            [FromQuery] StatusOlder? status = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                await using var _context = dbContextFactory.CreateDbContext();
                string sqlQuery = @"
                    SELECT o.* FROM older o
                    WHERE o.id_empresa = @idEmpresa
                    {0}";

                var parametros = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("idEmpresa", idEmpresa)
                };

                string filterCustomerName = "";

                if (!string.IsNullOrEmpty(customerName))
                {
                    var termos = customerName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(t => $"%{t}%")
                                          .ToList();

                    for (int i = 0; i < termos.Count; i++)
                    {
                        string paramName = $"@termo{i}";
                        filterCustomerName += $" AND unaccent(LOWER(o.customer_name)) LIKE CONCAT('%',unaccent(LOWER({paramName})),'%')";
                        parametros.Add(new NpgsqlParameter(paramName, termos[i]));
                    }
                }

                sqlQuery = string.Format(sqlQuery, filterCustomerName);

                var query = _context.Olders.FromSqlRaw(sqlQuery, parametros.ToArray())
                    .Where(p => p.IdEmpresa == idEmpresa);

                //var query = await ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                if (id.HasValue)
                {
                    query = query.Where(o => o.Id == id.Value);
                }

                /*if (!string.IsNullOrEmpty(customerName))
                {
                    query = query.Where(o => o.CustomerName.Contains(customerName));
                }*/

                if (status.HasValue)
                {
                    query = query.Where(o => o.Status == status.Value);
                }

                query = query.Include(o => o.OlderItems);

                var mappedQuery = query
                    .OrderByDescending(p => p.Id)
                    .ProjectTo<GetOldersDto>(_mapper.ConfigurationProvider);

                var pagedList = await mappedQuery.ToPagedListAsync(pageNumber, pageSize);

                if (pagedList == null || !pagedList.Any())
                {
                    return NotFound("Entities not found.");
                }

                var response = new PagedResponse<GetOldersDto>(pagedList);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpPatch("UpdateOlderStatus/{idEmpresa}/{id}", Name = nameof(UpdateOlderStatus))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOlderStatus(
            int idEmpresa,
            Guid id,
            [FromBody] StatusUpdateDto statusUpdateDto)
        {
            try
            {
                await using var _context = dbContextFactory.CreateDbContext();
                var order = await _context.Olders.FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return NotFound("Pedido não encontrado.");
                }

                order.Status = statusUpdateDto.Status;

                await _context.SaveChangesAsync();

                return Ok("Status do pedido atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao atualizar o status do pedido.");
                return StatusCode(500, "Ocorreu um erro ao atualizar o status do pedido. Por favor, tente novamente mais tarde.");
            }
        }


    }
}
