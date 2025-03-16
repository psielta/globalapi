using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class FornecedorPagedRepositoryMultiKey : GenericPagedRepositoryMultiKey<Fornecedor, GlobalErpFiscalBaseContext, int, int, FornecedorDto>
    {
        public FornecedorPagedRepositoryMultiKey(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Fornecedor, GlobalErpFiscalBaseContext, int, int, FornecedorDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<Fornecedor>> GetFornecedorPorUnity(int Unity)
        {
            try
            {
                return Task.FromResult(db.Set<Fornecedor>().Where(e => e.Unity == Unity)
                    .Include(e => e.CdCidadeNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Fornecedor>().AsQueryable());
            }
        }
        public async override Task<Fornecedor?> CreateAsync(FornecedorDto dto)
        {
            Fornecedor entity = mapper.Map<Fornecedor>(dto);
            EntityEntry<Fornecedor> added = await db.Set<Fornecedor>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<Fornecedor>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.CdForn == entity.CdForn && e.Unity == entity.Unity);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<Fornecedor?> UpdateAsync(int idEmpresa, int idCadastro, FornecedorDto dto)
        {
            Fornecedor entity = mapper.Map<Fornecedor>(dto);
            entity.GetType().GetProperty(entity.GetKeyName1())?.SetValue(entity, idEmpresa);
            entity.GetType().GetProperty(entity.GetKeyName2())?.SetValue(entity, idCadastro);
            db.Set<Fornecedor>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                UpdateCache((idEmpresa, idCadastro), entity);

                return await db.Set<Fornecedor>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.CdForn == entity.CdForn && e.Unity == entity.Unity);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
