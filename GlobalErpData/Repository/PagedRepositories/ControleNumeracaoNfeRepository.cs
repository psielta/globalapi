using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ControleNumeracaoNfeRepository : GenericPagedRepository<ControleNumeracaoNfe, GlobalErpFiscalBaseContext, int, ControleNumeracaoNfeDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        public ControleNumeracaoNfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ControleNumeracaoNfe, GlobalErpFiscalBaseContext, int, ControleNumeracaoNfeDto>> logger,
            GlobalErpFiscalBaseContext context) : base(injectedContext, mapper, logger)
        {
            _context = context;
            // Para acessar o set Controle de numerações usar '_context.ControleNumeracaoNves.'
        }

        public Task<IQueryable<ControleNumeracaoNfe>> GetControleNumeracaoNfeAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ControleNumeracaoNfe>()
                .Include(c => c.IdEmpresaNavigation)
                .Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
        }
        
        public Task<IQueryable<ControleNumeracaoNfe>> GetControleNumeracaoNfeAsyncPorUnity(int unity)
        {
            return Task.FromResult(db.Set<ControleNumeracaoNfe>()
                .Include(c => c.IdEmpresaNavigation)
                .Where(e => e.Unity == unity).AsQueryable());
        }

        public async override Task<ControleNumeracaoNfe?> CreateAsync(ControleNumeracaoNfeDto dto)
        {
            ControleNumeracaoNfe entity = mapper.Map<ControleNumeracaoNfe>(dto);

            if (entity.Padrao)
            {
                // Encontrar registros existentes para o mesmo IdEmpresa onde Padrao é verdadeiro
                var existingPadrao = await _context.ControleNumeracaoNves
                .Include(c => c.IdEmpresaNavigation)
                    .Where(e => e.IdEmpresa == entity.IdEmpresa && e.Padrao)
                    .ToListAsync();

                // Definir Padrao como falso para os registros existentes
                foreach (var existing in existingPadrao)
                {
                    existing.Padrao = false;
                    _context.ControleNumeracaoNves.Update(existing);
                }
            }

            EntityEntry<ControleNumeracaoNfe> added = await db.Set<ControleNumeracaoNfe>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected >= 1) // Pode ser maior que 1 se atualizamos registros existentes
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entidade criada e adicionada ao cache com ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ControleNumeracaoNfe>()
                .Include(c => c.IdEmpresaNavigation)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id);
            }
            else
            {
                logger.LogWarning("Falha ao criar entidade a partir do DTO.");
                return null;
            }
        }

        public async override Task<ControleNumeracaoNfe?> UpdateAsync(int id, ControleNumeracaoNfeDto dto)
        {
            ControleNumeracaoNfe entity = mapper.Map<ControleNumeracaoNfe>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);

            if (entity.Padrao)
            {
                // Encontrar registros existentes para o mesmo IdEmpresa onde Padrao é verdadeiro, excluindo a entidade atual
                var existingPadrao = await _context.ControleNumeracaoNves
                .Include(c => c.IdEmpresaNavigation)
                    .Where(e => e.IdEmpresa == entity.IdEmpresa && e.Padrao && e.Id != id)
                    .ToListAsync();

                // Definir Padrao como falso para os registros existentes
                foreach (var existing in existingPadrao)
                {
                    existing.Padrao = false;
                    _context.ControleNumeracaoNves.Update(existing);
                }
            }

            db.Set<ControleNumeracaoNfe>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected >= 1) // Pode ser maior que 1 se atualizamos registros existentes
            {
                logger.LogInformation("Entidade atualizada com ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<ControleNumeracaoNfe>()
                .Include(c => c.IdEmpresaNavigation)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                logger.LogWarning("Falha ao atualizar entidade com ID: {Id}", id);
                return null;
            }
        }
    }
}
