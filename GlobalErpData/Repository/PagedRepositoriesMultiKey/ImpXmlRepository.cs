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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class ImpXmlRepository : GenericPagedRepositoryMultiKey<Impxml, GlobalErpFiscalBaseContext, int, string, ImpxmlDto>
    {
        public ImpXmlRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Impxml, GlobalErpFiscalBaseContext, int, string, ImpxmlDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<Impxml>> GetImpxmlByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<Impxml>()
                .Where(e => e.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
