using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ProdutoEstoquePagedRepository : GenericPagedRepository<ProdutoEstoque, GlobalErpFiscalBaseContext, int, ProdutoEstoqueDto>
    {
        public ProdutoEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProdutoEstoque, GlobalErpFiscalBaseContext, int, ProdutoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<ProdutoEstoque>> GetProdutoEstoqueAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ProdutoEstoque>().Where(e => e.Unity == IdEmpresa).AsQueryable());
        }
    }
}
