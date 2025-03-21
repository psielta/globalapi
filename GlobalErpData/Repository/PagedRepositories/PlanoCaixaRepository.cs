using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalLib.Repository;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class PlanoCaixaRepository : GenericPagedRepository<PlanoDeCaixa, GlobalErpFiscalBaseContext, int, PlanoCaixaDto>
    {
        public PlanoCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PlanoDeCaixa, GlobalErpFiscalBaseContext, int, PlanoCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<PlanoDeCaixa>> GetPlanoDeCaixaAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<PlanoDeCaixa>().Where(e => e.Unity == IdEmpresa).AsQueryable());
        }
    }
}
