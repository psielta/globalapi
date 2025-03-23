using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class IcmRepository : GenericPagedRepository<Icm, GlobalErpFiscalBaseContext, int, IcmDto>
    {
        public IcmRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Icm, GlobalErpFiscalBaseContext, int, IcmDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<Icm>> GetIcmAsyncPorEmpresa(int unity)
        {
            return Task.FromResult(db.Set<Icm>().Where(e => e.Unity == unity).AsQueryable());
        }
    }
}
