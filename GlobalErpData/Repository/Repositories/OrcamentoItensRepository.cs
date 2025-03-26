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
    public class OrcamentoItensRepository : GenericRepositoryDto<OrcamentoIten, GlobalErpFiscalBaseContext, Guid, OrcamentoItensDto>
    {
        public OrcamentoItensRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OrcamentoIten, GlobalErpFiscalBaseContext, Guid, OrcamentoItensDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<OrcamentoIten>> GetOrcamentoItenPorCab(Guid idCab)
        {
            return await db.Set<OrcamentoIten>()
                .Where(e => e.IdCab == idCab)
                .ToListAsync();
        }
    }
}
