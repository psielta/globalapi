using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class SaldoEstoquePagedRepository : GenericPagedRepository<SaldoEstoque, GlobalErpFiscalBaseContext, int, SaldoEstoqueDto>
    {
        public SaldoEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<SaldoEstoque, GlobalErpFiscalBaseContext, int, SaldoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<SaldoEstoque>> GetSaldoEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<SaldoEstoque>().Where(e => e.CdEmpresa == idEmpresa)
                    .Include(e => e.ProdutoEstoque)
                    .Include(e => e.CdPlanoNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<SaldoEstoque>().AsQueryable());
            }
        }

        public async override Task<SaldoEstoque?> CreateAsync(SaldoEstoqueDto dto)
        {
            SaldoEstoque entity = mapper.Map<SaldoEstoque>(dto);
            EntityEntry<SaldoEstoque> added = await db.Set<SaldoEstoque>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());

                return await db.Set<SaldoEstoque>()
                    .Where(e =>
                        e.CdEmpresa == entity.CdEmpresa
                        && e.Id == entity.GetId())
                    .Include(e => e.ProdutoEstoque)
                    .Include(e => e.CdPlanoNavigation)
                    .OrderBy(e => e.Id)
                    .LastOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<SaldoEstoque?> UpdateAsync(int idCadastro, SaldoEstoqueDto dto)
        {
            SaldoEstoque entity = mapper.Map<SaldoEstoque>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, idCadastro);
            db.Set<SaldoEstoque>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idCadastro}", idCadastro);
                return await db.Set<SaldoEstoque>().Where(e => e.Id == idCadastro)
                    .Include(e => e.ProdutoEstoque)
                    .Include(e => e.CdPlanoNavigation)
                    .FirstOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idCadastro}", idCadastro);
                return null;
            }
        }
    }
}
