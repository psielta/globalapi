using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class ItemDetailRepository : GenericRepositoryDto<ItemDetail, GlobalErpFiscalBaseContext, int, ItemDetailDto>
    {
        public ItemDetailRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ItemDetail, GlobalErpFiscalBaseContext, int, ItemDetailDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public async Task<IEnumerable<ItemDetail>> GetItemDetailsAsyncPerEmpresa(int IdEmpresa)
        {
            return await db.Set<ItemDetail>()
                .Where(e => e.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
