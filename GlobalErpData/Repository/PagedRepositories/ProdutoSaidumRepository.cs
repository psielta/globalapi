using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ProdutoSaidumRepository : GenericPagedRepository<ProdutoSaidum, GlobalErpFiscalBaseContext, int, ProdutoSaidumDto>
    {
        public ProdutoSaidumRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProdutoSaidum, GlobalErpFiscalBaseContext, int, ProdutoSaidumDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ProdutoSaidum>> GetProdutoSaidumAsyncPorSaida(int NrSaida)
        {
            try
            {
                return Task.FromResult(db.Set<ProdutoSaidum>().Where(e => e.NrSaida == NrSaida)
                    .Include(e => e.ProdutoEstoque)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ProdutoSaidum>().AsQueryable());
            }
        }

        public async override Task<ProdutoSaidum?> CreateAsync(ProdutoSaidumDto dto)
        {
            ProdutoSaidum entity = mapper.Map<ProdutoSaidum>(dto);
            EntityEntry<ProdutoSaidum> added = await db.Set<ProdutoSaidum>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ProdutoSaidum>().Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.Nr == entity.Nr);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<ProdutoSaidum?> UpdateAsync(int id, ProdutoSaidumDto dto)
        {
            ProdutoSaidum entity = mapper.Map<ProdutoSaidum>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ProdutoSaidum>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<ProdutoSaidum>().Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.Nr == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
