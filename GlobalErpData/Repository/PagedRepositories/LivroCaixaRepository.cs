using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
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
    public class LivroCaixaRepository : GenericPagedRepository<LivroCaixa, GlobalErpFiscalBaseContext, long, LivroCaixaDto>
    {
        public LivroCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<LivroCaixa, GlobalErpFiscalBaseContext, long, LivroCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<LivroCaixa>> GetLivroCaixaAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<LivroCaixa>()
                .Include(l => l.HistoricoCaixa)
                .Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }

        public async override Task<LivroCaixa?> CreateAsync(LivroCaixaDto dto)
        {
            LivroCaixa entity = mapper.Map<LivroCaixa>(dto);
            EntityEntry<LivroCaixa> added = await db.Set<LivroCaixa>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<LivroCaixa>().Include(e => e.HistoricoCaixa)
                    .FirstOrDefaultAsync(e => e.NrLanc == entity.NrLanc);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<LivroCaixa?> UpdateAsync(long id, LivroCaixaDto dto)
        {
            LivroCaixa entity = mapper.Map<LivroCaixa>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<LivroCaixa>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<LivroCaixa>().Include(e => e.HistoricoCaixa)
                    .FirstOrDefaultAsync(e => e.NrLanc == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
