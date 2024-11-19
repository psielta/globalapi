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
    public class PagtosParciaisCpRepository : GenericPagedRepository<PagtosParciaisCp, GlobalErpFiscalBaseContext, int, PagtosParciaisCpDto>
    {
        public PagtosParciaisCpRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PagtosParciaisCp, GlobalErpFiscalBaseContext, int, PagtosParciaisCpDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<PagtosParciaisCp>> GetPagtosParciaisCpAsyncPorCP(int id)
        {
            return Task.FromResult(db.Set<PagtosParciaisCp>().Where(e => e.IdContasPagar == id).AsQueryable());
        }
    }
}
