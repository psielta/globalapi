using AutoMapper;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLib.Repository
{
    /// <summary>
    /// Exemplo de repositório sem cache estático, usando nomes de chave
    /// obtidos pela interface IIdentifiableMultiKey (GetKeyName1/2).
    /// </summary>
    public abstract class GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto>
        : IQueryRepositoryMultiKey<TEntity, TKey1, TKey2, TDto>
        where TEntity : class, IIdentifiableMultiKey<TKey1, TKey2>
        where TContext : DbContext
    {
        protected readonly TContext db;
        protected readonly IMapper mapper;
        protected readonly ILogger<GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto>> logger;

        public GenericPagedRepositoryMultiKey(
            TContext injectedContext,
            IMapper mapper,
            ILogger<GenericPagedRepositoryMultiKey<TEntity, TContext, TKey1, TKey2, TDto>> logger)
        {
            db = injectedContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public virtual Task<IQueryable<TEntity>> RetrieveAllAsync()
        {
            try
            {
                // Retorna IQueryable direto do EF
                return Task.FromResult(db.Set<TEntity>().AsQueryable());
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<TEntity>().AsQueryable());
            }
        }

        public virtual async Task<TEntity?> RetrieveAsync(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e =>
                        EF.Property<TKey1>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<TKey2>(e, keyName2).Equals(idCadastro));

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }

        public virtual async Task<TEntity?> RetrieveAsyncAsNoTracking(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<TEntity>()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e =>
                        EF.Property<TKey1>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<TKey2>(e, keyName2).Equals(idCadastro));

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
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

                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error while creating entity");
                throw;
            }
        }

        public virtual async Task<TEntity?> UpdateAsync(TKey1 idEmpresa, TKey2 idCadastro, TDto dto)
        {
            try
            {
                // Criamos uma instância "vazia" só para descobrir nomes de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName1 = tempEntity.GetKeyName1();
                string keyName2 = tempEntity.GetKeyName2();

                // Mapeia o DTO para uma nova instância
                TEntity updatedEntity = mapper.Map<TEntity>(dto);

                // Ajusta dinamicamente os valores das chaves
                //updatedEntity.GetType().GetProperty(keyName1)?.SetValue(updatedEntity, idEmpresa);
                updatedEntity.GetType().GetProperty(keyName2)?.SetValue(updatedEntity, idCadastro);

                db.Set<TEntity>().Update(updatedEntity);
                int affected = await db.SaveChangesAsync();
                if (affected == 1)
                {
                    logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                    return updatedEntity;
                }
                else
                {
                    logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating entity {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                throw;
            }
        }

        public virtual async Task<bool?> DeleteAsync(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                // Descobre nomes de chave
                var tempEntity = Activator.CreateInstance<TEntity>();
                string keyName1 = tempEntity.GetKeyName1();
                string keyName2 = tempEntity.GetKeyName2();

                // Localiza a entidade
                var entity = await db.Set<TEntity>()
                    .SingleOrDefaultAsync(e =>
                        EF.Property<TKey1>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<TKey2>(e, keyName2).Equals(idCadastro));

                if (entity == null)
                {
                    return null; // não encontrada
                }

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
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting entity {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                throw;
            }
        }
    }
}
