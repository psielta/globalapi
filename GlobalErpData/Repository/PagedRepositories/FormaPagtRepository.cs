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
    public class FormaPagtRepository : GenericPagedRepository<FormaPagt, GlobalErpFiscalBaseContext, int, FormaPagtDto>
    {
        public FormaPagtRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<FormaPagt, GlobalErpFiscalBaseContext, int, FormaPagtDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<FormaPagt>> GetFormaPagtAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<FormaPagt>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
