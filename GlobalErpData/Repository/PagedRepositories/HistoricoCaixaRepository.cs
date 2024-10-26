using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class HistoricoCaixaRepository : GenericPagedRepository<HistoricoCaixa, GlobalErpFiscalBaseContext, int, HistoricoCaixaDto>
    {
        public HistoricoCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<HistoricoCaixa, GlobalErpFiscalBaseContext, int, HistoricoCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<HistoricoCaixa>> GetHistoricoCaixaPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<HistoricoCaixa>().Where(e => e.CdEmpresa == idEmpresa)
                    .Include(e => e.PlanoDeCaixa)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<HistoricoCaixa>().AsQueryable());
            }
        }

        public async override Task<HistoricoCaixa?> CreateAsync(HistoricoCaixaDto dto)
        {
            HistoricoCaixa entity = mapper.Map<HistoricoCaixa>(dto);
            EntityEntry<HistoricoCaixa> added = await db.Set<HistoricoCaixa>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<HistoricoCaixa>().Include(e => e.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<HistoricoCaixa?> UpdateAsync(int id, HistoricoCaixaDto dto)
        {
            HistoricoCaixa entity = mapper.Map<HistoricoCaixa>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<HistoricoCaixa>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<HistoricoCaixa>().Include(e => e.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }

    
}
