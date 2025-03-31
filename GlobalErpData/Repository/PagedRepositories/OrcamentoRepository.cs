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
    public class OrcamentoRepository : GenericPagedRepository<OrcamentoCab, GlobalErpFiscalBaseContext, Guid, OrcamentoCabDto>
    {
        public OrcamentoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OrcamentoCab, GlobalErpFiscalBaseContext, Guid, OrcamentoCabDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<OrcamentoCab>> GetOrcamentoCabAsyncPorUnity(int unity)
        {
            return Task.FromResult(db.Set<OrcamentoCab>()
                .Include(p => p.OrcamentoItens)
                .Include(p => p.OrcamentoServicos)
                .Where(e => e.Unity == unity).AsQueryable());
        }

        public override Task<IQueryable<OrcamentoCab>> RetrieveAllAsync()
        {
            try
            {
                // Retorna o IQueryable do EF, sem cache estático
                return Task.FromResult(db.Set<OrcamentoCab>().Include(p => p.OrcamentoItens)
                .Include(p => p.OrcamentoServicos).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<OrcamentoCab>().AsQueryable());
            }
        }

        public override async Task<OrcamentoCab?> RetrieveAsync(Guid id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<OrcamentoCab>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<OrcamentoCab>().Include(p => p.OrcamentoItens)
                .Include(p => p.OrcamentoServicos)
                    .SingleOrDefaultAsync(e => EF.Property<Guid>(e, keyName)!.Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public virtual async Task<OrcamentoCab?> RetrieveAsyncAsNoTracking(Guid id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<OrcamentoCab>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<OrcamentoCab>().Include(p => p.OrcamentoItens)
                .Include(p => p.OrcamentoServicos).AsNoTracking()
                    .SingleOrDefaultAsync(e => EF.Property<Guid>(e, keyName)!.Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
