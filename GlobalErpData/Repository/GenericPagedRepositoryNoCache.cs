using AutoMapper;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalErpData.Repository
{
    public abstract class GenericPagedRepositoryNoCache<TEntity, TContext, TKey, TDto> : IQueryRepositoryNoCache<TEntity, TKey, TDto>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        protected readonly TContext db;
        protected readonly IMapper mapper;
        protected readonly ILogger<GenericPagedRepositoryNoCache<TEntity, TContext, TKey, TDto>> logger;

        public GenericPagedRepositoryNoCache(TContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryNoCache<TEntity, TContext, TKey, TDto>> logger)
        {
            db = injectedContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual async Task<TEntity?> CreateAsync(TDto dto)
        {
            try
            {
                TEntity entity = mapper.Map<TEntity>(dto);
                await db.Set<TEntity>().AddAsync(entity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    logger.LogInformation("Entity created with ID: {Id}", entity.GetId());
                    return entity;
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

        public virtual Task<IQueryable<TEntity>> RetrieveAllAsync()
        {
            try
            {
                return Task.FromResult(db.Set<TEntity>().AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<TEntity>().AsQueryable());
            }
        }

        public virtual async Task<TEntity?> RetrieveAsync(TKey id)
        {
            try
            {
                TEntity? entity = await db.Set<TEntity>().FindAsync(id);
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public virtual async Task<TEntity?> UpdateAsync(TKey id, TDto dto)
        {
            try
            {
                TEntity? existingEntity = await db.Set<TEntity>().FindAsync(id);
                if (existingEntity == null)
                {
                    logger.LogWarning("Entity with ID {Id} not found.", id);
                    return null;
                }

                mapper.Map(dto, existingEntity);
                db.Set<TEntity>().Update(existingEntity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    logger.LogInformation("Entity updated with ID: {Id}", id);
                    return existingEntity;
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
                return null;
            }
        }

        public virtual async Task<bool?> DeleteAsync(TKey id)
        {
            try
            {
                TEntity? entity = await db.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    logger.LogWarning("Entity with ID {Id} not found.", id);
                    return null;
                }

                db.Set<TEntity>().Remove(entity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    logger.LogInformation("Entity deleted with ID: {Id}", id);
                    return true;
                }
                else
                {
                    logger.LogWarning("Failed to delete entity with ID: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting entity with ID: {Id}", id);
                return null;
            }
        }

        public virtual async Task<IEnumerable<TEntity>?> CreateBulkAsync(IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos == null || !dtos.Any())
                {
                    return null;
                }

                var entities = dtos.Select(dto => mapper.Map<TEntity>(dto)).ToList();
                await db.Set<TEntity>().AddRangeAsync(entities);
                int affected = await db.SaveChangesAsync();

                if (affected >= entities.Count)
                {
                    logger.LogInformation("{Count} entities created in bulk.", entities.Count);
                    return entities;
                }
                else
                {
                    logger.LogWarning("Failed to create entities in bulk.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating entities in bulk.");
                return null;
            }
        }
    }
}
