using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContasAPagarRepository : GenericPagedRepository<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>
    {
        public ContasAPagarRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContasAPagar>> GetContasAPagarAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<ContasAPagar>().Where(e => e.CdEmpresa == IdEmpresa)
                    .Include(e => e.Fornecedor)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ContasAPagar>().AsQueryable());
            }
        }
    }
}
