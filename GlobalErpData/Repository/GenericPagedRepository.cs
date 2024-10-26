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
    public abstract class GenericPagedRepository<TEntity, TContext, TKey, TDto> : IQueryRepository<TEntity, TKey, TDto>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        protected TContext db;
        protected IMapper mapper;
        protected readonly ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger;

        public GenericPagedRepository(TContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger)
        {
            db = injectedContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual async Task<TEntity?> CreateAsync(TDto dto)
        {
            TEntity entity = mapper.Map<TEntity>(dto);
            EntityEntry<TEntity> added = await db.Set<TEntity>().AddAsync(entity);
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

        public virtual Task<TEntity?> RetrieveAsync(TKey id)
        {
            try
            {
                return Task.FromResult(db.Set<TEntity>().Find(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return Task.FromResult<TEntity?>(null);
            }
        }

        public virtual async Task<TEntity?> UpdateAsync(TKey id, TDto dto)
        {
            TEntity entity = mapper.Map<TEntity>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<TEntity>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                return entity;
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }

        public virtual async Task<bool?> DeleteAsync(TKey id)
        {
            TEntity? entity = db.Set<TEntity>().Find(id);
            if (entity is null) return null;
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
                return null;
            }
        }

        public virtual async Task<IEnumerable<TEntity>?> CreateBulkAsync(IEnumerable<TDto> dtos)
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
                return entities;
            }
            else
            {
                return null;
            }
        }
    }
}
