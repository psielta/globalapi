using AutoMapper;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLib.Repository
{
    public abstract class GenericRepositoryDto<TEntity, TContext, TKey, TDto>
        : IRepositoryDto<TEntity, TKey, TDto>
        where TEntity : class, IIdentifiable<TKey>
        where TContext : DbContext
    {
        protected readonly TContext db;
        protected readonly IMapper mapper;
        protected readonly ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger;

        public GenericRepositoryDto(
            TContext injectedContext,
            IMapper mapper,
            ILogger<GenericRepositoryDto<TEntity, TContext, TKey, TDto>> logger)
        {
            db = injectedContext;
            this.mapper = mapper;
            this.logger = logger;
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

        public async Task<IEnumerable<TEntity>> RetrieveAllAsync()
        {
            try
            {
                // Retorna todas as entidades direto do banco (sem cache estático)
                return await db.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Enumerable.Empty<TEntity>();
            }
        }

        public async Task<TEntity?> RetrieveAsync(TKey id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName = tempEntity.GetKeyName();

                // Filtra com EF.Property<TKey>(...) para encontrar o registro pela PK
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName).Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public async Task<TEntity?> UpdateAsync(TKey id, TDto dto)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName = tempEntity.GetKeyName();

                // Mapeia o DTO para a entidade
                TEntity entity = mapper.Map<TEntity>(dto);

                // Ajusta a chave dinamicamente (caso não tenha vindo do DTO)
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
                throw;
            }
        }

        public async Task<bool?> DeleteAsync(TKey id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName = tempEntity.GetKeyName();

                // Localiza a entidade para remoção
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName).Equals(id));

                if (entity == null)
                {
                    return null; // não encontrada
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

        public async Task<TEntity?> RetrieveAsyncAsNoTracking(TKey id)
        {
            try
            {
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName = tempEntity.GetKeyName();

                // NO TRACKING: Retorna a entidade sem cache estático
                var entity = await db.Set<TEntity>().AsNoTracking()
                    .SingleOrDefaultAsync(e => EF.Property<TKey>(e, keyName).Equals(id));

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
