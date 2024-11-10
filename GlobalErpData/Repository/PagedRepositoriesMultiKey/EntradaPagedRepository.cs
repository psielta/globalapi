using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using GlobalLib.Repository;
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
        private readonly EntradaCalculationService _calculationService;
        private readonly IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> produtoEntradaRepository;

        public EntradaPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Entrada, GlobalErpFiscalBaseContext, int, int, EntradaDto>> logger,
            EntradaCalculationService calculationService, IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> produtoEntradaRepository) : base(injectedContext, mapper, logger)
        {
            _calculationService = calculationService;
            this.produtoEntradaRepository = produtoEntradaRepository;
        }
        public Task<IQueryable<Entrada>> GetEntradaAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Entrada>().Where(e => e.CdEmpresa == IdEmpresa)
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
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
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                Entrada? entradaGerada = await db.Set<Entrada>()
                    .Where(e =>
                        e.Nr == entity.Nr
                        && e.CdEmpresa == entity.CdEmpresa)
                    .OrderByDescending(e => e.Data) // Adiciona uma ordenação
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
                    .LastOrDefaultAsync();
                if (entradaGerada is null)
                {
                    logger.LogWarning("Failed to retrieve entity from database.");
                    return null;
                }
                _calculationService.CalculateTotals(entradaGerada);
                return entradaGerada;
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        public async override Task<Entrada?> UpdateAsync(int idEmpresa, int idCadastro, EntradaDto dto)
        {
            Entrada? oldEntrada = await db.Set<Entrada>().AsNoTracking()
                .Where(e => e.CdEmpresa == idEmpresa && e.Nr == idCadastro)
                .FirstOrDefaultAsync();
            if (oldEntrada is null)
            {
                logger.LogWarning("Failed to retrieve entity from database.");
                return null;
            }
            Entrada entity = mapper.Map<Entrada>(dto);
            entity.GetType().GetProperty(entity.GetKeyName1())?.SetValue(entity, idEmpresa);
            entity.GetType().GetProperty(entity.GetKeyName2())?.SetValue(entity, idCadastro);
            db.Set<Entrada>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                UpdateCache((idEmpresa, idCadastro), entity);
                if (oldEntrada.CdGrupoEstoque != dto.CdGrupoEstoque)
                {
                    bool x = await ((ProdutoEntradaRepository)produtoEntradaRepository).UpdateAllCdGrupoEstoqueAsync(idEmpresa, idCadastro, dto.CdGrupoEstoque);
                    if (!x)
                    {
                        logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}.", idEmpresa, idCadastro);
                        //return null;
                    }

                }
                Entrada? entradaGerada = await db.Set<Entrada>()
                    .Where(e => e.CdEmpresa == idEmpresa && e.Nr == idCadastro)
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
                    .FirstOrDefaultAsync();
                if (entradaGerada is null)
                {
                    logger.LogWarning("Failed to retrieve entity from database.");
                    return null;
                }
                _calculationService.CalculateTotals(entradaGerada);
                return entradaGerada;
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
