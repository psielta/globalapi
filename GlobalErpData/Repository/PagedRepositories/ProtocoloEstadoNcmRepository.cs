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
    public class ProtocoloEstadoNcmRepository : GenericPagedRepository<ProtocoloEstadoNcm, GlobalErpFiscalBaseContext, int, ProtocoloEstadoNcmDto>
    {
        public ProtocoloEstadoNcmRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProtocoloEstadoNcm, GlobalErpFiscalBaseContext, int, ProtocoloEstadoNcmDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ProtocoloEstadoNcm>> GetProtocoloEstadoNcmAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ProtocoloEstadoNcm>().Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
        }
    }

}
