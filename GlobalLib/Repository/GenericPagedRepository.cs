using AutoMapper;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GlobalLib.Repository
{
    /// <summary>
    /// Repositório genérico para entidades com chave única (IIdentifiable<TKey>), 
    /// sem cache estático e usando nomes de chave obtidos de GetKeyName().
    /// </summary>
    public abstract class GenericPagedRepository<TEntity, TContext, TKey, TDto>
        : IQueryRepository<TEntity, TKey, TDto>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        protected readonly TContext db;
        protected readonly IMapper mapper;
        protected readonly ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger;

        public GenericPagedRepository(
            TContext injectedContext,
            IMapper mapper,
            ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger)
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating entity.");
                throw; // ou retorne null, dependendo da política de erro
            }
        }

        public virtual Task<IQueryable<TEntity>> RetrieveAllAsync()
        {
            try
            {
                // Retorna o IQueryable do EF, sem cache estático
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
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName)!.Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }
        
        public virtual async Task<TEntity?> RetrieveAsyncAsNoTracking(TKey id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<TEntity>().AsNoTracking()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName)!.Equals(id));

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
                // Descobre nome de propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                var keyName = tempEntity.GetKeyName();

                // Mapeia o DTO para a nova instância
                TEntity entity = mapper.Map<TEntity>(dto);

                // Ajusta dinamicamente a chave no objeto antes de chamar Update
                entity.GetType().GetProperty(keyName)?.SetValue(entity, id);

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
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity with ID: {Id}", id);
                throw; // ou retorne null
            }
        }

        public virtual async Task<bool?> DeleteAsync(TKey id)
        {
            try
            {
                // Descobre nome de propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                var keyName = tempEntity.GetKeyName();

                // Localiza entidade dinamicamente
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName)!.Equals(id));

                if (entity is null)
                {
                    return null; // Não existe
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
                throw; // ou retorne false
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
                    // Criados com sucesso
                    logger.LogInformation("Bulk create of {Count} entities completed.", entities.Count);
                    return entities;
                }
                else
                {
                    logger.LogWarning("Bulk create partially failed. Only {Affected} of {Total} saved.", affected, entities.Count);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while bulk creating entities.");
                throw;
            }
        }
    }
}
