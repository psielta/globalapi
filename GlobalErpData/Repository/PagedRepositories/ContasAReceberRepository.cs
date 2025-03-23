using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using GlobalLib.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContasAReceberRepository : GenericPagedRepository<ContasAReceber, GlobalErpFiscalBaseContext, int, ContasAReceberDto>
    {
        public ContasAReceberRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContasAReceber, GlobalErpFiscalBaseContext, int, ContasAReceberDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContasAReceber>> GetContasAReceberAsyncPorUnity(int unity)
        {
            return Task.FromResult(db.Set<ContasAReceber>().Where(e => e.Unity == unity)
                .Include(e => e.PagtosParciaisCrs)
                .Include(p => p.CdClienteNavigation)
                .AsQueryable());
        }

        public async override Task<ContasAReceber?> CreateAsync(ContasAReceberDto dto)
        {
            ContasAReceber entity = mapper.Map<ContasAReceber>(dto);
            EntityEntry<ContasAReceber> added = await db.Set<ContasAReceber>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ContasAReceber>().Include(e => e.CdClienteNavigation)
                    .Include(e => e.PagtosParciaisCrs)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.NrConta == entity.NrConta);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<ContasAReceber?> UpdateAsync(int id, ContasAReceberDto dto)
        {
            ContasAReceber entity = mapper.Map<ContasAReceber>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ContasAReceber>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<ContasAReceber>().Include(e => e.CdClienteNavigation)
                    .Include(e => e.PagtosParciaisCrs)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.NrConta == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }

        public async override Task<IEnumerable<ContasAReceber>?> CreateBulkAsync(IEnumerable<ContasAReceberDto> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return null;
            }

            var entities = dtos.Select(dto => mapper.Map<ContasAReceber>(dto)).ToList();
            await db.Set<ContasAReceber>().AddRangeAsync(entities);
            int affected = await db.SaveChangesAsync();

            if (affected >= entities.Count)
            {
                foreach (var entity in entities)
                {
                    EntityCache?.AddOrUpdate(entity.GetId(), entity, UpdateCache);
                }
                //return entities;
                return await db.Set<ContasAReceber>().Include(e => e.CdClienteNavigation)
                    .Include(e => e.PagtosParciaisCrs)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .Where(e => entities.Select(x => x.NrConta).Contains(e.NrConta) && e.CdEmpresa == entities.First().CdEmpresa)
                    .ToListAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
