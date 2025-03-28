﻿using AutoMapper;
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
using GlobalLib.Strings;

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

        public override async Task<Entrada?> RetrieveAsync(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<Entrada>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<Entrada>()
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
                    .SingleOrDefaultAsync(e =>
                        EF.Property<int>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<int>(e, keyName2).Equals(idCadastro));

                if (entity == null)
                {
                    return null;
                }

                _calculationService.CalculateTotals(entity);

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
        
        public override async Task<Entrada?> RetrieveAsyncAsNoTracking(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<Entrada>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<Entrada>().AsNoTracking()
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
                    .SingleOrDefaultAsync(e =>
                        EF.Property<int>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<int>(e, keyName2).Equals(idCadastro));

                if (entity == null)
                {
                    return null;
                }

                _calculationService.CalculateTotals(entity);

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
        public Task<IQueryable<Entrada>> GetEntradaAsyncPorEmpresa(int unity)
        {
            try
            {
                return Task.FromResult(db.Set<Entrada>().Where(e => e.Unity == unity)
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
                    .AsNoTracking()
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
                if ((entradaGerada.CdEmpresa != dto.CdEmpresa)
                    || (entradaGerada.CdGrupoEstoque != dto.CdGrupoEstoque)
                    || (!entradaGerada.TpEntrada.Equals(dto.TpEntrada)))
                {
                    await this.db.Database.ExecuteSqlRawAsync(
                      @$"UPDATE entradas SET 
                            tp_entrada = {UtlStrings.QuotedStr(dto.TpEntrada)},
                            cd_empresa = {dto.CdEmpresa},
                            cd_grupo_estoque = {dto.CdGrupoEstoque}
                         WHERE nr = {idCadastro} and cd_empresa = {idEmpresa}");
                    entradaGerada = await db.Set<Entrada>()
                    .AsNoTracking()
                    .Where(e => e.CdEmpresa == idEmpresa && e.Nr == idCadastro)
                    .Include(e => e.Fornecedor)
                    .Include(e => e.CdGrupoEstoqueNavigation)
                    .Include(e => e.ProdutoEntrada)
                    .FirstOrDefaultAsync();
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
