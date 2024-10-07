using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class OlderRepository : GenericPagedRepository<Older, GlobalErpFiscalBaseContext, Guid, OlderDto>
    {
        public OlderRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Older, GlobalErpFiscalBaseContext, Guid, OlderDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<Older>> GetOlderPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Older>().Where(e => e.IdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Older>().AsQueryable());
            }
        }
    }
}
