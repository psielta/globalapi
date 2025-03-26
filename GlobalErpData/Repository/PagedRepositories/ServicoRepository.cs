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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ServicoRepository : GenericPagedRepository<Servico, GlobalErpFiscalBaseContext, long, ServicoDto>
    {
        public ServicoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Servico, GlobalErpFiscalBaseContext, long, ServicoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<Servico>> GetServicosPorUnity(int unity)
        {
            return await db.Set<Servico>()
                .Where(e => e.Unity == unity)
                .ToListAsync();
        }
    }
}
