using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.Repositories
{
    public class SectionItemRepository : GenericRepositoryDto<SectionItem, GlobalErpFiscalBaseContext, int, SectionItemDto>
    {
        public SectionItemRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<SectionItem, GlobalErpFiscalBaseContext, int, SectionItemDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<SectionItem>> GetSectionItemsByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<SectionItem>()
                .Where(s => s.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
