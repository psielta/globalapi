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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class ConfiguracoesEmpresaRepository : GenericPagedRepositoryMultiKey<ConfiguracoesEmpresa, GlobalErpFiscalBaseContext, int, string, ConfiguracoesEmpresaDto>
    {
        public ConfiguracoesEmpresaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<ConfiguracoesEmpresa, GlobalErpFiscalBaseContext, int, string, ConfiguracoesEmpresaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ConfiguracoesEmpresa>> GetConfiguracoesEmpresaPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<ConfiguracoesEmpresa>().Where(e => e.CdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ConfiguracoesEmpresa>().AsQueryable());
            }
        }
    }
}
