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
    public class PagtosParciaisCrRepository : GenericPagedRepository<PagtosParciaisCr, GlobalErpFiscalBaseContext, int, PagtosParciaisCrDto>
    {
        public PagtosParciaisCrRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PagtosParciaisCr, GlobalErpFiscalBaseContext, int, PagtosParciaisCrDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<PagtosParciaisCr>> GetPagtosParciaisCrAsyncPorCR(int nrConta)
        {
            return Task.FromResult(db.Set<PagtosParciaisCr>().Where(e => e.NrConta == nrConta).AsQueryable());
        }
    }
}
