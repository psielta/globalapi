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
    public class PerfilLojaRepository : GenericRepositoryDto<PerfilLoja, GlobalErpFiscalBaseContext, int, PerfilLojaDto>
    {
        public PerfilLojaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PerfilLoja, GlobalErpFiscalBaseContext, int, PerfilLojaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<PerfilLoja>> GetPerfilLojasByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<PerfilLoja>()
                .Where(s => s.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
