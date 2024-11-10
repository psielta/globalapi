using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class OlderItemRepository : GenericRepositoryDto<OlderItem, GlobalErpFiscalBaseContext, Guid, OlderItemDto>
    {
        public OlderItemRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OlderItem, GlobalErpFiscalBaseContext, Guid, OlderItemDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<OlderItem>> GetOlderItemsByOlderAsync(Guid OlderId)
        {
            return await db.Set<OlderItem>()
                .Where(s => s.OlderId == OlderId)
                .ToListAsync();
        }
    }
}
