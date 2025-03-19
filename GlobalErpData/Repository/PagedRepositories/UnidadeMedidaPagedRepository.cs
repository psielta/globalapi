using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class UnidadeMedidaPagedRepository : GenericPagedRepository<UnidadeMedida, GlobalErpFiscalBaseContext, int, UnidadeMedidaDto>
    {
        public UnidadeMedidaPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<UnidadeMedida, GlobalErpFiscalBaseContext, int, UnidadeMedidaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<UnidadeMedida>> GetUnidadeMedidaPorEmpresa(int unity)
        {
            return Task.FromResult(db.Set<UnidadeMedida>().Where(e => e.Unity == unity).AsQueryable());
        }
    }
}
