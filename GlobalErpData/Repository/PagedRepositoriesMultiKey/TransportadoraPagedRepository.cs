using AutoMapper;
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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class TransportadoraPagedRepository : GenericPagedRepositoryMultiKey<Transportadora, GlobalErpFiscalBaseContext, int, int, TransportadoraDto>
    {
        public TransportadoraPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Transportadora, GlobalErpFiscalBaseContext, int, int, TransportadoraDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<Transportadora>> GetTransportadoraPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Transportadora>().Where(e => e.IdEmpresa == idEmpresa)
                    .Include(e => e.CdCidadeNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Transportadora>().AsQueryable());
            }
        }
        public async override Task<Transportadora?> CreateAsync(TransportadoraDto dto)
        {
            Transportadora entity = mapper.Map<Transportadora>(dto);
            EntityEntry<Transportadora> added = await db.Set<Transportadora>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<Transportadora>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.CdTransportadora == entity.CdTransportadora && e.IdEmpresa == entity.IdEmpresa);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<Transportadora?> UpdateAsync(int idEmpresa, int idCadastro, TransportadoraDto dto)
        {
            Transportadora entity = mapper.Map<Transportadora>(dto);
            entity.GetType().GetProperty(entity.GetKeyName1())?.SetValue(entity, idEmpresa);
            entity.GetType().GetProperty(entity.GetKeyName2())?.SetValue(entity, idCadastro);
            db.Set<Transportadora>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                UpdateCache((idEmpresa, idCadastro), entity);

                return await db.Set<Transportadora>().Include(e => e.CdCidadeNavigation)
                    .FirstOrDefaultAsync(e => e.CdTransportadora == entity.CdTransportadora && e.IdEmpresa == entity.IdEmpresa);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
