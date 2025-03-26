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
    public class OsTabelaPrecoRepository : GenericRepositoryDto<OsTabelaPreco, GlobalErpFiscalBaseContext, long, OsTabelaPrecoDto>
    {
        public OsTabelaPrecoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OsTabelaPreco, GlobalErpFiscalBaseContext, long, OsTabelaPrecoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<OsTabelaPreco>> GetOsTabelaPrecoPorUnity(int unity)
        {
            return await db.Set<OsTabelaPreco>()
                .Where(e => e.Unity == unity)
                .ToListAsync();
        }
    }
}
