using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using GlobalLib.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class SectionRepository : GenericRepositoryDto<Section, GlobalErpFiscalBaseContext, int, SectionDto>
    {
        public SectionRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Section, GlobalErpFiscalBaseContext, int, SectionDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public async Task<IEnumerable<Section>> GetSectionsByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<Section>()
                .Where(s => s.Unity == IdEmpresa)
                .ToListAsync();
        }
    }
}
