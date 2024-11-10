using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class ProdutoEstoquePagedRepositoryMultiKey : GenericPagedRepositoryMultiKey<ProdutoEstoque, GlobalErpFiscalBaseContext, int, int, ProdutoEstoqueDto>
    {
        public ProdutoEstoquePagedRepositoryMultiKey(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<ProdutoEstoque, GlobalErpFiscalBaseContext, int, int, ProdutoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<ProdutoEstoque>> GetProdutoEstoqueAsyncPorEmpresa(int IdEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<ProdutoEstoque>().Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<ProdutoEstoque>().AsQueryable());
            }
        }
    }
}
