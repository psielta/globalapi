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
    public abstract class GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto> : IQueryRepositoryMultiKey<TEntity, TKey1, TKey2, TDto>
        where TEntity : class, IIdentifiableMultiKey<TKey1, TKey2>
        where TContext : DbContext
    {
        protected TContext db;
        protected IMapper mapper;
        protected readonly ILogger<GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto>> logger;

        public GenericPagedRepositoryMultiKey(TContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto>> logger)
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

        public virtual Task<TEntity?> RetrieveAsync(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                return Task.FromResult(db.Set<TEntity>().Find(idEmpresa, idCadastro));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return Task.FromResult<TEntity?>(null);
            }
        }

        public virtual async Task<TEntity?> UpdateAsync(TKey1 idEmpresa, TKey2 idCadastro, TDto dto)
        {
            TEntity entity = mapper.Map<TEntity>(dto);
            entity.GetType().GetProperty(entity.GetKeyName1())?.SetValue(entity, idEmpresa);
            entity.GetType().GetProperty(entity.GetKeyName2())?.SetValue(entity, idCadastro);
            db.Set<TEntity>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return entity;
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }

        public virtual async Task<bool?> DeleteAsync(TKey1 idEmpresa, TKey2 idCadastro)
        {
            TEntity? entity = null;
            await foreach (var e in db.Set<TEntity>().AsAsyncEnumerable())
            {
                var keyValue1 = e.GetType().GetProperty(e.GetKeyName1())?.GetValue(e);
                var keyValue2 = e.GetType().GetProperty(e.GetKeyName2())?.GetValue(e);

                if (keyValue1 != null && keyValue1.Equals(idEmpresa) && keyValue2 != null && keyValue2.Equals(idCadastro))
                {
                    entity = e;
                    break;
                }
            }

            if (entity is null) return null;

            db.Set<TEntity>().Remove(entity);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                logger.LogInformation("Entity deleted with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to delete entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
