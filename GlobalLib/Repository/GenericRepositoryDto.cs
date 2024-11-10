using AutoMapper;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLib.Repository
{
    public abstract class GenericRepositoryDto<TEntity, TContext, TKey, TDto> : IRepositoryDto<TEntity, TKey, TDto>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        private static ConcurrentDictionary<TKey, TEntity>? EntityCache;
        protected TContext db;
        protected IMapper mapper;
        protected readonly ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger;

        public GenericRepositoryDto(TContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger)
        {
            db = injectedContext;
            this.mapper = mapper;
            this.logger = logger;

            if (EntityCache is null)
            {
                EntityCache = new ConcurrentDictionary<TKey, TEntity>(
                    db.Set<TEntity>().ToDictionary(e => e.GetId()));
            }
        }

        public async Task<TEntity?> CreateAsync(TDto dto)
        {
            try
            {
                TEntity entity = mapper.Map<TEntity>(dto);
                EntityEntry<TEntity> added = await db.Set<TEntity>().AddAsync(entity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    if (EntityCache is null) return entity;
                    logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                    return EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);
                }
                else
                {
                    logger.LogWarning("Failed to create entity from DTO.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating entity from DTO.");
                return null;
            }
        }

        public Task<IEnumerable<TEntity>> RetrieveAllAsync()
        {
            try
            {
                var entities = EntityCache is null ? Enumerable.Empty<TEntity>() : EntityCache.Values.AsEnumerable();
                return Task.FromResult(entities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<TEntity>());
            }
        }

        public Task<TEntity?> RetrieveAsync(TKey id)
        {
            try
            {
                if (EntityCache is null) return null!;
                EntityCache.TryGetValue(id, out TEntity? entity);
                return Task.FromResult(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return Task.FromResult<TEntity?>(null);
            }
        }

        private TEntity UpdateCache(TKey id, TEntity entity)
        {
            try
            {
                TEntity? old;
                if (EntityCache is not null)
                {
                    if (EntityCache.TryGetValue(id, out old))
                    {
                        if (EntityCache.TryUpdate(id, entity, old))
                        {
                            logger.LogInformation("Entity cache updated for ID: {Id}", id);
                            return entity;
                        }
                    }
                }
                return null!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating cache for entity with ID: {Id}", id);
                return null!;
            }
        }

        public async Task<TEntity?> UpdateAsync(TKey id, TDto dto)
        {
            try
            {
                TEntity entity = mapper.Map<TEntity>(dto);
                entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
                db.Set<TEntity>().Update(entity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    logger.LogInformation("Entity updated with ID: {Id}", id);
                    return UpdateCache(id, entity);
                }
                else
                {
                    logger.LogWarning("Failed to update entity with ID: {Id}", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity with ID: {Id}", id);
                throw ex;
            }
        }

        public async Task<bool?> DeleteAsync(TKey id)
        {
            try
            {
                TEntity? entity = db.Set<TEntity>().Find(id);
                if (entity is null) return null;
                db.Set<TEntity>().Remove(entity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    if (EntityCache is null) return null;
                    logger.LogInformation("Entity deleted with ID: {Id}", id);
                    return EntityCache.TryRemove(id, out entity);
                }
                else
                {
                    logger.LogWarning("Failed to delete entity with ID: {Id}", id);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
