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
    public class PlanoSimultaneoRepository : GenericRepositoryDto<PlanoSimultaneo, GlobalErpFiscalBaseContext, int, PlanoSimultaneoDto>
    {
        public PlanoSimultaneoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PlanoSimultaneo, GlobalErpFiscalBaseContext, int, PlanoSimultaneoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public IQueryable<PlanoSimultaneo> GetPlanoSimultaneosByUnity(int unity)
        {
            return this.db.PlanoSimultaneos
                .Include(p => p.CdPlanoPrincNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                .Include(p => p.CdPlanoReplicaNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                .Where(p => p.Unity == unity).AsQueryable();
        }

        public override async Task<PlanoSimultaneo?> RetrieveAsyncAsNoTracking(int id)
        {
            try
            {
                var tempEntity = Activator.CreateInstance<PlanoSimultaneo>();
                string keyName = tempEntity.GetKeyName();

                // NO TRACKING: Retorna a entidade sem cache estático
                var entity = await db.Set<PlanoSimultaneo>().AsNoTracking()
                    .Include(p => p.CdPlanoPrincNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .Include(p => p.CdPlanoReplicaNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .SingleOrDefaultAsync(e => EF.Property<int>(e, keyName).Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public override async Task<PlanoSimultaneo?> RetrieveAsync(int id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<PlanoSimultaneo>();
                string keyName = tempEntity.GetKeyName();

                // Filtra com EF.Property<int>(...) para encontrar o registro pela PK
                var entity = await db.Set<PlanoSimultaneo>()
                    .Include(p => p.CdPlanoPrincNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .Include(p => p.CdPlanoReplicaNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .SingleOrDefaultAsync(e => EF.Property<int>(e, keyName).Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public override async Task<IEnumerable<PlanoSimultaneo>> RetrieveAllAsync()
        {
            try
            {
                // Retorna todas as entidades direto do banco (sem cache estático)
                return await db.Set<PlanoSimultaneo>()
                    .Include(p => p.CdPlanoPrincNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .Include(p => p.CdPlanoReplicaNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Enumerable.Empty<PlanoSimultaneo>();
            }
        }
    }
}
