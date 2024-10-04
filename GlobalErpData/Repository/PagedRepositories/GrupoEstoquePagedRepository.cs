using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Data;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class GrupoEstoquePagedRepository : GenericPagedRepository<GrupoEstoque, GlobalErpFiscalBaseContext, int, GrupoEstoqueDto>
    {
        public GrupoEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<GrupoEstoque, GlobalErpFiscalBaseContext, int, GrupoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<GrupoEstoque>> GetGrupoEstoqueAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<GrupoEstoque>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
