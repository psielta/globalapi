using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class OrcamentoRepository : GenericPagedRepository<OrcamentoCab, GlobalErpFiscalBaseContext, Guid, OrcamentoCabDto>
    {
        public OrcamentoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OrcamentoCab, GlobalErpFiscalBaseContext, Guid, OrcamentoCabDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<OrcamentoCab>> GetOrcamentoCabAsyncPorUnity(int unity)
        {
            return Task.FromResult(db.Set<OrcamentoCab>()
                .Where(e => e.Unity == unity).AsQueryable());
        }
    }
}
