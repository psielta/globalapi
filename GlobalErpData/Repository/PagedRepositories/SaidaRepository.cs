using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using GlobalLib.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Services;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class SaidaRepository : GenericPagedRepository<Saida, GlobalErpFiscalBaseContext, int, SaidaDto>
    {
        private readonly SaidaCalculationService _calculationService;
        public SaidaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Saida, GlobalErpFiscalBaseContext, int, SaidaDto>> logger, SaidaCalculationService saidaCalculationService) : base(injectedContext, mapper, logger)
        {
            _calculationService = saidaCalculationService;
        }

        public Task<IQueryable<Saida>> GetSaidaAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Saida>().Where(e => e.Empresa == IdEmpresa)
                    .Include(f => f.Fretes)
                    .Include(e => e.ClienteNavigation)
                    .Include(p => p.CdGrupoEstoqueNavigation)
                    .Include(p => p.ProdutoSaida)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Saida>().AsQueryable());
            }
        }

        public async override Task<Saida?> CreateAsync(SaidaDto dto)
        {
            Saida entity = mapper.Map<Saida>(dto);
            EntityEntry<Saida> added = await db.Set<Saida>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                var saida = await db.Set<Saida>().Include(f => f.Fretes).Include(e => e.ClienteNavigation)
                    .Include(p => p.ProdutoSaida)
                    .Include(p => p.CdGrupoEstoqueNavigation)
                    .FirstOrDefaultAsync(e => e.NrLanc == entity.NrLanc);
                if (saida is null)
                {
                    logger.LogWarning("Failed to retrieve entity from database.");
                    return null;
                }
                _calculationService.CalculateTotals(saida);
                return saida;
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<Saida?> UpdateAsync(int id, SaidaDto dto)
        {
            Saida entity = mapper.Map<Saida>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<Saida>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                var saida = await db.Set<Saida>().Include(f => f.Fretes).Include(e => e.ClienteNavigation)
                    .Include(p => p.ProdutoSaida)
                    .Include(p => p.CdGrupoEstoqueNavigation)
                    .FirstOrDefaultAsync(e => e.NrLanc == id);
                if (saida is null)
                {
                    logger.LogWarning("Failed to retrieve entity from database.");
                    return null;
                }
                _calculationService.CalculateTotals(saida);
                return saida;
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
