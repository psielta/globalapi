using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ClientePagedRepositoyDto : GenericPagedRepository<Cliente, GlobalErpFiscalBaseContext, int, ClienteDto>
    {
        public ClientePagedRepositoyDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cliente, GlobalErpFiscalBaseContext, int, ClienteDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<Cliente>> GetClientePorUnity(int Unity)
        {
            try
            {
                return Task.FromResult(db.Set<Cliente>().Where(e => e.Unity == Unity)
                    .Include(e => e.CdCidadeNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Cliente>().AsQueryable());
            }
        }
        public async override Task<Cliente?> CreateAsync(ClienteDto dto)
        {
            Cliente entity = mapper.Map<Cliente>(dto);
            EntityEntry<Cliente> added = await db.Set<Cliente>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<Cliente>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<Cliente?> UpdateAsync(int id, ClienteDto dto)
        {
            Cliente entity = mapper.Map<Cliente>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<Cliente>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<Cliente>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
