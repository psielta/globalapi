﻿using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContasAPagarRepository : GenericPagedRepository<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>
    {
        public ContasAPagarRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContasAPagar>> GetContasAPagarAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<ContasAPagar>().Where(e => e.CdEmpresa == IdEmpresa)
                    .Include(e => e.Fornecedor)
                    .Include(e=> e.PagtosParciaisCps)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ContasAPagar>().AsQueryable());
            }
        }
        
        public Task<IQueryable<ContasAPagar>> GetContasAPagarAsyncPorUnity(int unity)
        {
            try
            {
                return Task.FromResult(db.Set<ContasAPagar>().Where(e => e.Unity == unity)
                    .Include(e => e.Fornecedor)
                    .Include(e=> e.PagtosParciaisCps)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ContasAPagar>().AsQueryable());
            }
        }

        public async override Task<ContasAPagar?> CreateAsync(ContasAPagarDto dto)
        {
            ContasAPagar entity = mapper.Map<ContasAPagar>(dto);
            EntityEntry<ContasAPagar> added = await db.Set<ContasAPagar>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());

                return await db.Set<ContasAPagar>().Include(e => e.Fornecedor)
                    .Include(e=> e.PagtosParciaisCps)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<ContasAPagar?> UpdateAsync(int id, ContasAPagarDto dto)
        {
            ContasAPagar entity = mapper.Map<ContasAPagar>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ContasAPagar>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                return await db.Set<ContasAPagar>().Include(e => e.Fornecedor)
                    .Include(e=> e.PagtosParciaisCps)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }

        public async override Task<IEnumerable<ContasAPagar>?> CreateBulkAsync(IEnumerable<ContasAPagarDto> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return null;
            }

            var entities = dtos.Select(dto => mapper.Map<ContasAPagar>(dto)).ToList();
            await db.Set<ContasAPagar>().AddRangeAsync(entities);
            int affected = await db.SaveChangesAsync();

            if (affected >= entities.Count)
            {
                //return entities;
                return await db.Set<ContasAPagar>().Include(e => e.Fornecedor)
                    .Include(e=> e.PagtosParciaisCps)
                    .Include(e => e.HistoricoCaixa).ThenInclude(h => h.PlanoDeCaixa)
                    .Where(e => entities.Select(x => x.Id).Contains(e.Id) && e.CdEmpresa == entities.First().CdEmpresa)
                    .ToListAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
