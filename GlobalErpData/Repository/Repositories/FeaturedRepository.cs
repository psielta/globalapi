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
    public class FeaturedRepository : GenericRepositoryDto<Featured, GlobalErpFiscalBaseContext, int, FeaturedDto>
    {
        public FeaturedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Featured, GlobalErpFiscalBaseContext, int, FeaturedDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<Featured>> GetFeaturedByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<Featured>()
                .Where(e => e.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
