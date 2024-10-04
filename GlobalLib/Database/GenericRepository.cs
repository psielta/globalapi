using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLib.Database
{
    public abstract class GenericRepository<TEntity, TContext, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        private static ConcurrentDictionary<TKey, TEntity>? EntityCache;
        protected TContext db;

        public GenericRepository(TContext injectedContext)
        {
            db = injectedContext;

            if (EntityCache is null)
            {
                EntityCache = new ConcurrentDictionary<TKey, TEntity>(
                    db.Set<TEntity>().ToDictionary(e => e.GetId()));
            }
        }

        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            EntityEntry<TEntity> added = await db.Set<TEntity>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                return EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);
            }
            else
            {
                return null;
            }
        }

        public Task<IEnumerable<TEntity>> RetrieveAllAsync()
        {
            return Task.FromResult(EntityCache is null
                ? Enumerable.Empty<TEntity>() : EntityCache.Values.AsEnumerable());
        }

        public Task<TEntity?> RetrieveAsync(TKey id)
        {
            if (EntityCache is null) return null!;
            EntityCache.TryGetValue(id, out TEntity? entity);
            return Task.FromResult(entity);
        }

        private TEntity UpdateCache(TKey id, TEntity entity)
        {
            TEntity? old;
            if (EntityCache is not null)
            {
                if (EntityCache.TryGetValue(id, out old))
                {
                    if (EntityCache.TryUpdate(id, entity, old))
                    {
                        return entity;
                    }
                }
            }
            return null!;
        }

        public async Task<TEntity?> UpdateAsync(TKey id, TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return UpdateCache(id, entity);
            }
            return null;
        }

        public async Task<bool?> DeleteAsync(TKey id)
        {
            TEntity? entity = db.Set<TEntity>().Find(id);
            if (entity is null) return null;
            db.Set<TEntity>().Remove(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return null;
                return EntityCache.TryRemove(id, out entity);
            }
            else
            {
                return null;
            }
        }
    }
}
