using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using GlobalLib.Repository;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class ConfiguracoesUsuarioRepository : GenericPagedRepositoryMultiKey<ConfiguracoesUsuario, GlobalErpFiscalBaseContext, int, string, ConfiguracoesUsuarioDto>
    {
        public ConfiguracoesUsuarioRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<ConfiguracoesUsuario, GlobalErpFiscalBaseContext, int, string, ConfiguracoesUsuarioDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ConfiguracoesUsuario>> GetConfiguracoesUsuarioPorUsuario(int idUsuario)
        {
            try
            {
                return Task.FromResult(db.Set<ConfiguracoesUsuario>().Where(e => e.IdUsuario == idUsuario).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ConfiguracoesUsuario>().AsQueryable());
            }
        }
    }
}
