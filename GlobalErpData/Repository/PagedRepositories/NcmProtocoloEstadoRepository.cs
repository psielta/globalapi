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
    public class NcmProtocoloEstadoRepository : GenericPagedRepository<NcmProtocoloEstado, GlobalErpFiscalBaseContext, int, NcmProtocoloEstadoDto>
    {
        public NcmProtocoloEstadoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<NcmProtocoloEstado, GlobalErpFiscalBaseContext, int, NcmProtocoloEstadoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<NcmProtocoloEstado>> GetNcmProtocoloEstadoAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<NcmProtocoloEstado>().Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
