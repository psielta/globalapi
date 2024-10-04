using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ProdutosFornRepository : GenericPagedRepository<ProdutosForn, GlobalErpFiscalBaseContext, int, ProdutosFornDto>
    {
        public ProdutosFornRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProdutosForn, GlobalErpFiscalBaseContext, int, ProdutosFornDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ProdutosForn>> GetProdutosFornAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<ProdutosForn>()
                    .Where(e => e.IdEmpresa == IdEmpresa)
                    .Include(e => e.ProdutoEstoque)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ProdutosForn>().AsQueryable());
            }
        }
        public async override Task<ProdutosForn?> CreateAsync(ProdutosFornDto dto)
        {
            ProdutosForn entity = mapper.Map<ProdutosForn>(dto);
            EntityEntry<ProdutosForn> added = await db.Set<ProdutosForn>().AddAsync(entity);
            int affected = await this.db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ProdutosForn>()
                    .Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.CdProduto == entity.CdProduto && e.CdForn == entity.CdForn);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<ProdutosForn?> UpdateAsync(int id, ProdutosFornDto dto)
        {
            ProdutosForn entity = mapper.Map<ProdutosForn>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ProdutosForn>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);

                return await db.Set<ProdutosForn>()
                    .Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.CdProduto == entity.CdProduto && e.CdForn == entity.CdForn);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
