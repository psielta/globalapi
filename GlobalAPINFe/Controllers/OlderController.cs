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

        [HttpGet("GetOldersCanceledByMonth/{idEmpresa}", Name = nameof(GetOldersCanceledByMonth))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetOldersCanceledByMonth(int idEmpresa)
        {
            try
            {
                OldersCanceledDto oldersCanceledDto = new OldersCanceledDto();

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersCanceled = allOlders.Where(o => o.Status == StatusOlder.Cancelado).ToList();

                var currentMonth = DateTime.Now.Month;
                var previousMonth = DateTime.Now.AddMonths(-1).Month;

                var canceledOrdersCurrentMonth = allOldersCanceled
                    .Where(o => o.CreatedAt.Month == currentMonth)
                    .ToList();

                var canceledOrdersPreviousMonth = allOldersCanceled
                    .Where(o => o.CreatedAt.Month == previousMonth)
                    .ToList();

                oldersCanceledDto.QuantityOfCancellations = canceledOrdersCurrentMonth.Count;

                if (canceledOrdersPreviousMonth.Count > 0)
                {
                    oldersCanceledDto.PercentageComparedToPreviousMonth = ((double)(canceledOrdersCurrentMonth.Count - canceledOrdersPreviousMonth.Count) / canceledOrdersPreviousMonth.Count) * 100;
                }
                else
                {
                    oldersCanceledDto.PercentageComparedToPreviousMonth = 100;
                }

                return Ok(oldersCanceledDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao atualizar o status do pedido.");
                return StatusCode(500, "Ocorreu um erro ao atualizar o status do pedido. Por favor, tente novamente mais tarde.");
            }
        }


        [HttpGet("GetOldersApprovedByMonth/{idEmpresa}", Name = nameof(GetOldersApprovedByMonth))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetOldersApprovedByMonth(
            int idEmpresa)
        {
            try
            {
                OldersQuantityDto OldersApprovedByMonthDto = new OldersQuantityDto();
                OldersApprovedByMonthDto.PercentageComparedToPrevious = 0;
                OldersApprovedByMonthDto.QuantityOfCategory = 0;

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersApproved = allOlders.Where(o => o.Status == StatusOlder.Aprovado).ToList();
                var currentMonth = DateTime.Now.Month;
                var previousMonth = DateTime.Now.AddMonths(-1).Month;

                var approvedOldersCurrentMonth = allOldersApproved
                    .Where(o => o.CreatedAt.Month == currentMonth)
                    .ToList();

                var approvedOldersPreviousMonth = allOldersApproved
                    .Where(o => o.CreatedAt.Month == previousMonth)
                    .ToList();

                OldersApprovedByMonthDto.QuantityOfCategory = approvedOldersCurrentMonth.Count;

                if (approvedOldersPreviousMonth.Count > 0)
                {
                    OldersApprovedByMonthDto.PercentageComparedToPrevious = ((double)(approvedOldersCurrentMonth.Count - approvedOldersPreviousMonth.Count) / approvedOldersPreviousMonth.Count) * 100;
                }
                else
                {
                    OldersApprovedByMonthDto.PercentageComparedToPrevious = 100;
                }


                return Ok(OldersApprovedByMonthDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao obter dados.");
                return StatusCode(500, "Ocorreu um erro ao obter dados.");
            }
        }

        [HttpGet("GetOldersApprovedByDay/{idEmpresa}", Name = nameof(GetOldersApprovedByDay))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetOldersApprovedByDay(
            int idEmpresa)
        {
            try
            {
                OldersQuantityDto OldersByDay = new OldersQuantityDto();
                OldersByDay.PercentageComparedToPrevious = 0;
                OldersByDay.QuantityOfCategory = 0;

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersApproved = allOlders.Where(o => o.Status == StatusOlder.Aprovado).ToList();
                var approvedOldersToday = allOldersApproved
                    .Where(o => o.CreatedAt.Date == DateTime.Today)
                    .ToList();
                var approvedOldersFromPreviousDay = allOldersApproved
                    .Where(o => o.CreatedAt.Date == DateTime.Today.AddDays(-1))
                    .ToList();
                OldersByDay.QuantityOfCategory = approvedOldersToday.Count;

                if (approvedOldersFromPreviousDay.Count > 0)
                {
                    OldersByDay.PercentageComparedToPrevious = ((double)(approvedOldersToday.Count - approvedOldersFromPreviousDay.Count) / approvedOldersFromPreviousDay.Count) * 100;
                }
                else
                {
                    OldersByDay.PercentageComparedToPrevious = 100;
                }


                return Ok(OldersByDay);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao obter dados.");
                return StatusCode(500, "Ocorreu um erro ao obter dados.");
            }
        }

        [HttpGet("GetOldersMoneyByMonth/{idEmpresa}", Name = nameof(GetOldersMoneyByMonth))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetOldersMoneyByMonth(
            int idEmpresa)
        {
            try
            {
                OldersMoneyByMonthDto OldersApprovedByMonthDto = new OldersMoneyByMonthDto();
                OldersApprovedByMonthDto.PercentageComparedToPrevious = 0;
                OldersApprovedByMonthDto.totalMonth = 0;

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersApproved = allOlders.Where(o => o.Status == StatusOlder.Aprovado).ToList();
                var currentMonth = DateTime.Now.Month;
                var previousMonth = DateTime.Now.AddMonths(-1).Month;

                var approvedOldersCurrentMonth = allOldersApproved
                    .Where(o => o.CreatedAt.Month == currentMonth)
                    .ToList();

                var approvedOldersPreviousMonth = allOldersApproved
                    .Where(o => o.CreatedAt.Month == previousMonth)
                    .ToList();

                var totalCurrentMonth = approvedOldersCurrentMonth.Sum(o => o.Total);
                var totalPreviousMonth = approvedOldersPreviousMonth.Sum(o => o.Total);

                OldersApprovedByMonthDto.totalMonth = (double)totalCurrentMonth;

                if (totalPreviousMonth > 0)
                {
                    OldersApprovedByMonthDto.PercentageComparedToPrevious = ((double)((totalCurrentMonth - totalPreviousMonth) / totalPreviousMonth) * 100);
                }
                else
                {
                    OldersApprovedByMonthDto.PercentageComparedToPrevious = 100;
                }

                return Ok(OldersApprovedByMonthDto);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao obter dados.");
                return StatusCode(500, "Ocorreu um erro ao obter dados.");
            }
        }

        [HttpGet("GetOldersMoneyByLast7Days/{idEmpresa}", Name = nameof(GetOldersMoneyByLast7Days))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetOldersMoneyByLast7Days(int idEmpresa)
        {
            try
            {
                OldersMoneyByDayLast7DaysDto result = new OldersMoneyByDayLast7DaysDto
                {
                    Items = new List<MoneyByDayLast7DaysDto>()
                };

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersApproved = allOlders.Where(o => o.Status == StatusOlder.Aprovado).ToList();

                for (int i = 6; i >= 0; i--)
                {
                    var date = DateTime.Today.AddDays(-i);

                    var totalDay = allOldersApproved
                        .Where(o => o.CreatedAt.Date == date)
                        .Sum(o => o.Total);

                    result.Items.Add(new MoneyByDayLast7DaysDto
                    {
                        DateTime = date,
                        totalDay = (double) totalDay
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao obter dados.");
                return StatusCode(500, "Ocorreu um erro ao obter dados.");
            }
        }

        [HttpGet("GetTop5Products/{idEmpresa}", Name = nameof(GetTop5Products))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetTop5Products(int idEmpresa)
        {
            try
            {
                Top05ProductsDto result = new Top05ProductsDto
                {
                    Items = new List<Top05ProductsItemDto>()
                };

                var allOlders = ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);
                var allOldersApproved = allOlders.Where(o => o.Status == StatusOlder.Aprovado).ToList();

                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var approvedOldersCurrentMonth = allOldersApproved
                    .Where(o => o.CreatedAt.Month == currentMonth && o.CreatedAt.Year == currentYear)
                    .ToList();

                var orderItems = approvedOldersCurrentMonth
                    .SelectMany(o => o.OlderItems)
                    .ToList();

                var groupedItems = orderItems
                    .GroupBy(i => i.Name)
                    .Select(g => new Top05ProductsItemDto
                    {
                        Name = g.Key,
                        Quantity = g.Sum(i => i.Quantity),
                        Total = (double)g.Sum(i => i.Subtotal)
                    })
                    .OrderByDescending(i => i.Quantity)
                    .Take(5)
                    .ToList();

                result.Items = groupedItems;

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao obter os produtos mais vendidos.");
                return StatusCode(500, "Ocorreu um erro ao obter os produtos mais vendidos.");
            }
        }
    }
}
