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
    public class RetiradaNfeRepository : GenericPagedRepository<RetiradaNfe, GlobalErpFiscalBaseContext, int, RetiradaNfeDto>
    {
        public RetiradaNfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<RetiradaNfe, GlobalErpFiscalBaseContext, int, RetiradaNfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<RetiradaNfe>> GetRetiradaNfeAsyncPorCliente(int idCliente)
        {
            return Task.FromResult(db.Set<RetiradaNfe>().Where(e => e.IdCliente == idCliente).AsQueryable());
        }
    }
}
