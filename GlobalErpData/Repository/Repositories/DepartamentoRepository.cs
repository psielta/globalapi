using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class DepartamentoRepository : GenericRepositoryDto<Departamento, GlobalErpFiscalBaseContext, long, DepartamentoDto>
    {
        public DepartamentoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Departamento, GlobalErpFiscalBaseContext, long, DepartamentoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<Departamento>> GetDepartamentosPorUnity(int unity)
        {
            return await db.Set<Departamento>()
                .Where(e => e.Unity == unity)
                .ToListAsync();
        }
    }
}
