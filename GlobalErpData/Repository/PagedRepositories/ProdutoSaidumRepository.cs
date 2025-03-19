using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Services;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ProdutoSaidumRepository : GenericPagedRepository<ProdutoSaidum, GlobalErpFiscalBaseContext, int, ProdutoSaidumDto>
    {
        private readonly ProdutoSaidumService produtoSaidumService;
        private readonly IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto> queryRepositoryMultiKey;
        private readonly IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto> unidadeMedidaService;
        public ProdutoSaidumRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProdutoSaidum, GlobalErpFiscalBaseContext, int, ProdutoSaidumDto>> logger,
                ProdutoSaidumService produtoSaidumService, IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto> queryRepositoryMultiKey, IQueryRepository<UnidadeMedida, int, UnidadeMedidaDto> unidadeMedidaService) : base(injectedContext, mapper, logger)
        {
            this.produtoSaidumService = produtoSaidumService;
            this.queryRepositoryMultiKey = queryRepositoryMultiKey;
            this.unidadeMedidaService = unidadeMedidaService;
        }

        public Task<IQueryable<ProdutoSaidum>> GetProdutoSaidumAsyncPorSaida(int NrSaida)
        {
            try
            {
                return Task.FromResult(db.Set<ProdutoSaidum>().Where(e => e.NrSaida == NrSaida)
                    .Include(e => e.ProdutoEstoque)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ProdutoSaidum>().AsQueryable());
            }
        }

        public async override Task<ProdutoSaidum?> CreateAsync(ProdutoSaidumDto dto)
        {
            ProdutoEstoque? produto = await db.ProdutoEstoques
                .FirstOrDefaultAsync(e => e.CdProduto == dto.CdProduto && e.Unity == dto.Unity);

            if (produto is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Produto not found.");
                return null;
            }
            Saida? saida = await db.Saidas
                        .AsNoTracking()
                        .Include(s => s.ClienteNavigation).ThenInclude(c => c.CdCidadeNavigation)
                        .FirstOrDefaultAsync(obj => obj.NrLanc == dto.NrSaida && obj.Empresa == dto.CdEmpresa);
            if (saida is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Saida not found.");
                return null;
            }
            await AtualizarCadastroProduto(dto, produto, saida.ClienteNavigation);
            //await produtoSaidumService.InserirDadosProduto(new InsercaoProdutoSaidumDto()
            //{
            //    NrSaida = saida.NrLanc,
            //    CdEmpresa = dto.CdEmpresa,
            //    CdProduto = dto.CdProduto,
            //    Quant = dto.Quant,
            //    CdPlano = saida.CdGrupoEstoque
            //}, dto, produto, saida.ClienteNavigation);
            await produtoSaidumService.RealizarCalculoImpostoSaida(dto, produto, saida.ClienteNavigation);
            ProdutoSaidum entity = mapper.Map<ProdutoSaidum>(dto);
            EntityEntry<ProdutoSaidum> added = await db.Set<ProdutoSaidum>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                if (EntityCache is null) return entity;
                logger.LogInformation("Entity created and added to cache with ID: {Id}", entity.GetId());
                EntityCache.AddOrUpdate(entity.GetId(), entity, UpdateCache);

                return await db.Set<ProdutoSaidum>().Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.Nr == entity.Nr);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }

        private async Task AtualizarCadastroProduto(ProdutoSaidumDto dto, ProdutoEstoque? produto, Cliente clienteNavigation)
        {
            if ((!(produto?.CdBarra ?? "").Equals(dto.CdBarra)) && ((dto.CdBarra ?? "").Length >= 8 || dto.CdBarra.Equals("SEM GTIN")))
            {
                produto.CdBarra = dto.CdBarra ?? "";
            }
            if ((!(produto.CdCsosn ?? "").Equals(dto.CdCsosn)) && (dto?.CdCsosn ?? "").Length == 4)
            {
                produto.CdCsosn = dto?.CdCsosn ?? "";
            }
            if ((!(produto.CdClassFiscal ?? "").Equals(dto?.Ncm ?? "")) && (dto?.Ncm ?? "").Length == 8)
            {
                produto.CdClassFiscal = dto?.Ncm ?? "";
            }
            if ((!(produto.Cest ?? "").Equals(dto?.Cest ?? "")) && (dto.Cest ?? "").Length == 7)
            {
                produto.Cest = dto.Cest;
            }


            Empresa? empresa = await db.Empresas.AsNoTracking().Include(c => c.CdCidadeNavigation).FirstOrDefaultAsync(e => e.CdEmpresa == dto.CdEmpresa);
            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada");
            }

            if (empresa.CdCidadeNavigation.Uf.Equals(clienteNavigation.Uf))
            {
                if ((!(produto.CfoDentro ?? "").Equals(dto.Cfop ?? "")) && (dto.Cfop ?? "").StartsWith("5"))
                {
                    produto.CfoDentro = dto.Cfop;
                }
                if ((!(produto.CstDentro1 ?? "").Equals(dto.Cst ?? "")) && (dto?.Cst ?? "").Length == 3)
                {
                    produto.CstDentro1 = dto.Cst;
                }
            }
            else
            {
                if ((!(produto.CfoFora ?? "").Equals(dto.Cfop ?? "")) && (dto.Cfop ?? "").StartsWith("6"))
                {
                    produto.CfoFora = dto.Cfop ?? "";
                }
                if ((!(produto.CstFora1 ?? "").Equals(dto.Cst ?? "")) && (dto?.Cst ?? "").Length == 3)
                {
                    produto.CstFora1 = dto.Cst ?? "";
                }
            }

            db.ProdutoEstoques.Update(produto);
            await db.SaveChangesAsync();
            ((ProdutoEstoquePagedRepositoryMultiKey)queryRepositoryMultiKey).UpdateCache((produto.Unity, produto.CdProduto), produto);

            UnidadeMedida? unidade = db.UnidadeMedidas.Where(u => u.CdUnidade == dto.Un && u.Unity == produto.Unity).FirstOrDefault();
            if (unidade == null)
            {
                UnidadeMedida unidadeNova = new UnidadeMedida();
                unidadeNova.CdUnidade = dto.Un ?? "";
                unidadeNova.Unity = produto.Unity;
                unidadeNova.Descricao = dto.Un ?? "";

                db.UnidadeMedidas.Add(unidadeNova);
                await db.SaveChangesAsync();
                ((UnidadeMedidaPagedRepository)unidadeMedidaService).UpdateCache(unidadeNova.Id, unidadeNova);
            }
        }

        public async override Task<ProdutoSaidum?> UpdateAsync(int id, ProdutoSaidumDto dto)
        {
            ProdutoEstoque? produto = await db.ProdutoEstoques
                .FirstOrDefaultAsync(e => e.CdProduto == dto.CdProduto && e.Unity == dto.Unity);
            if (produto is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Produto not found.");
                return null;
            }
            Saida? saida = await db.Saidas
                        .AsNoTracking()
                        .Include(s => s.ClienteNavigation).ThenInclude(c => c.CdCidadeNavigation)
                        .FirstOrDefaultAsync(obj => obj.NrLanc == dto.NrSaida && obj.Empresa == dto.CdEmpresa);
            if (saida is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Saida not found.");
                return null;
            }
            await AtualizarCadastroProduto(dto, produto, saida.ClienteNavigation);
            //await produtoSaidumService.InserirDadosProduto(new InsercaoProdutoSaidumDto()
            //{
            //    NrSaida = saida.NrLanc,
            //    CdEmpresa = dto.CdEmpresa,
            //    CdProduto = dto.CdProduto,
            //    Quant = dto.Quant,
            //    CdPlano = saida.CdGrupoEstoque
            //}, dto, produto, saida.ClienteNavigation);
            await produtoSaidumService.RealizarCalculoImpostoSaida(dto, produto, saida.ClienteNavigation);
            ProdutoSaidum entity = mapper.Map<ProdutoSaidum>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ProdutoSaidum>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<ProdutoSaidum>().Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.Nr == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
        
        public async Task<ProdutoSaidum?> UpdateAsyncSemRecalcular(int id, ProdutoSaidumDto dto)
        {
            ProdutoEstoque? produto = await db.ProdutoEstoques
                .FirstOrDefaultAsync(e => e.CdProduto == dto.CdProduto && e.Unity == dto.Unity);
            if (produto is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Produto not found.");
                return null;
            }
            Saida? saida = await db.Saidas
                        .AsNoTracking()
                        .Include(s => s.ClienteNavigation).ThenInclude(c => c.CdCidadeNavigation)
                        .FirstOrDefaultAsync(obj => obj.NrLanc == dto.NrSaida && obj.Empresa == dto.CdEmpresa);
            if (saida is null)
            {
                logger.LogWarning("Failed to create entity from DTO. Saida not found.");
                return null;
            }
            await AtualizarCadastroProduto(dto, produto, saida.ClienteNavigation);
            //await produtoSaidumService.RealizarCalculoImpostoSaida(dto, produto, saida.ClienteNavigation);
            ProdutoSaidum entity = mapper.Map<ProdutoSaidum>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<ProdutoSaidum>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                UpdateCache(id, entity);
                return await db.Set<ProdutoSaidum>().Include(e => e.ProdutoEstoque)
                    .FirstOrDefaultAsync(e => e.Nr == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
