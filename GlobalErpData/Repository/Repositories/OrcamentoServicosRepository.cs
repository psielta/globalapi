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

namespace GlobalErpData.Repository.Repositories
{
    public class OrcamentoServicosRepository : GenericRepositoryDto<OrcamentoServico, GlobalErpFiscalBaseContext, Guid, OrcamentoServicosDto>
    {
        public OrcamentoServicosRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OrcamentoServico, GlobalErpFiscalBaseContext, Guid, OrcamentoServicosDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<OrcamentoServico>> GetOrcamentoServicoPorCab(Guid idCab)
        {
            return await db.Set<OrcamentoServico>()
                .Where(e => e.IdCab == idCab)
                .ToListAsync();
        }
    }
}
