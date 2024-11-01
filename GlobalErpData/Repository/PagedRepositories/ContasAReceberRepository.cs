using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContasAReceberRepository : GenericPagedRepository<ContasAReceber, GlobalErpFiscalBaseContext, int, ContasAReceberDto>
    {
        public ContasAReceberRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContasAReceber, GlobalErpFiscalBaseContext, int, ContasAReceberDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContasAReceber>> GetContasAReceberAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ContasAReceber>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
