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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class EntradaPagedRepository : GenericPagedRepositoryMultiKey<Entrada, GlobalErpFiscalBaseContext, int, int, EntradaDto>
    {
        public EntradaPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Entrada, GlobalErpFiscalBaseContext, int, int, EntradaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<Entrada>> GetEntradaAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Entrada>().Where(e => e.CdEmpresa == IdEmpresa)
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Entrada>().AsQueryable());
            }
        }
        public async override Task<Entrada?> CreateAsync(EntradaDto dto)
        {
            Entrada entity = mapper.Map<Entrada>(dto);
            EntityEntry<Entrada> added = await db.Set<Entrada>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());

                return await db.Set<Entrada>()
                    .Where(e =>
                        e.Nr == entity.Nr
                        && e.CdEmpresa == entity.CdEmpresa)
                    .OrderByDescending(e => e.Data) // Adiciona uma ordenação
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .LastOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<Entrada?> UpdateAsync(int idEmpresa, int idCadastro, EntradaDto dto)
        {
            Entrada entity = mapper.Map<Entrada>(dto);
            entity.GetType().GetProperty(entity.GetKeyName1())?.SetValue(entity, idEmpresa);
            entity.GetType().GetProperty(entity.GetKeyName2())?.SetValue(entity, idCadastro);
            db.Set<Entrada>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return await db.Set<Entrada>().Where(e => e.CdEmpresa == idEmpresa && e.CdForn == idCadastro && e.Data == entity.Data).Include(e => e.Fornecedor).Include(e => e.CdGrupoEstoqueNavigation).FirstOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
