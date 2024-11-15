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
    public class EntregaNfeRepository : GenericPagedRepository<EntregaNfe, GlobalErpFiscalBaseContext, int, EntregaNfeDto>
    {
        public EntregaNfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<EntregaNfe, GlobalErpFiscalBaseContext, int, EntregaNfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<EntregaNfe>> GetEntregaNfeAsyncPorCliente(int idCliente)
        {
            return Task.FromResult(db.Set<EntregaNfe>().Where(e => e.IdCliente == idCliente).AsQueryable());
        }
    }
}
