using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ProdutoEntradaRepository : GenericPagedRepository<ProdutoEntradum, GlobalErpFiscalBaseContext,
        int, ProdutoEntradaDto>
    {
        public ProdutoEntradaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProdutoEntradum, GlobalErpFiscalBaseContext, int, ProdutoEntradaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ProdutoEntradum>> GetProdutoEntradaAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ProdutoEntradum>().Where(e => e.CdEmpresa == IdEmpresa)
                .Include(e => e.ProdutoEstoque)
                .AsQueryable());
        }

        public async override Task<ProdutoEntradum?> CreateAsync(ProdutoEntradaDto dto)
        {
            ProdutoEntradum entity = mapper.Map<ProdutoEntradum>(dto);
            EntityEntry<ProdutoEntradum> added = await db.Set<ProdutoEntradum>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ProdutoEntradum>()
                    .Where(e =>
                        e.CdEmpresa == entity.CdEmpresa
                        && e.Nr == entity.GetId()
                        && e.NrEntrada == entity.NrEntrada)
                    .Include(e => e.ProdutoEstoque)
                    .OrderBy(e => e.Nr)
                    .LastOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<ProdutoEntradum?> UpdateAsync(int idCadastro, ProdutoEntradaDto dto)
        {
            ProdutoEntradum entity = mapper.Map<ProdutoEntradum>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, idCadastro);
            db.Set<ProdutoEntradum>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idCadastro}", idCadastro);
                UpdateCache(idCadastro, entity);
                return await db.Set<ProdutoEntradum>().Where(e => e.Nr == idCadastro).Include(e => e.ProdutoEstoque).FirstOrDefaultAsync();
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idCadastro}", idCadastro);
                return null;
            }
        }

        public async Task<bool> UpdateAllCdGrupoEstoqueAsync(int idEmpresa, int nrEntrada, int newCdGrupoEstoque)
        {
            Entrada? entrada = await db.Set<Entrada>().Where(e => e.CdEmpresa == idEmpresa && e.Nr == nrEntrada).FirstOrDefaultAsync();
            if (entrada is null)
            {
                logger.LogWarning("Failed to find entity with ID: {nrEntrada}", nrEntrada);
                return false;
            }
            if (entrada.CdGrupoEstoque != newCdGrupoEstoque)
            {
                logger.LogWarning("Failed to update entity with ID: {nrEntrada}.", nrEntrada);
                return false;
            }

            List<ProdutoEntradum> entity = await db.Set<ProdutoEntradum>().Where(e => e.CdEmpresa == idEmpresa && e.NrEntrada == nrEntrada).ToListAsync();
            if (entity == null || entity.Count == 0)
            {
                return true;
            }
            foreach (var item in entity)
            {
                item.CdPlano = newCdGrupoEstoque;
                db.Set<ProdutoEntradum>().Update(item);
            }
            int affected = await db.SaveChangesAsync();
            if (affected == entity.Count)
            {
                logger.LogInformation("Entity updated with ID: {nrEntrada}", nrEntrada);
                foreach (var item in entity)
                {
                    UpdateCache(item.GetId(), item);
                }
                return true;
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {nrEntrada}", nrEntrada);
                return false;
            }
        }
    }
}
