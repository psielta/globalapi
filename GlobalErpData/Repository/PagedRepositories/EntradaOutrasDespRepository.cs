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
    public class EntradaOutrasDespRepository : GenericPagedRepository<EntradaOutrasDesp, GlobalErpFiscalBaseContext, int, EntradaOutrasDespDto>
    {
        public EntradaOutrasDespRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<EntradaOutrasDesp, GlobalErpFiscalBaseContext, int, EntradaOutrasDespDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<EntradaOutrasDesp>> GetEntradaOutrasDespAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<EntradaOutrasDesp>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
