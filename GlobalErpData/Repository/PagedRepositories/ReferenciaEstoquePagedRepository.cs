using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ReferenciaEstoquePagedRepository : GenericPagedRepository<ReferenciaEstoque, GlobalErpFiscalBaseContext, int, ReferenciaEstoqueDto>
    {
        public ReferenciaEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ReferenciaEstoque, GlobalErpFiscalBaseContext, int, ReferenciaEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ReferenciaEstoque>> GetReferenciaEstoqueAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ReferenciaEstoque>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
