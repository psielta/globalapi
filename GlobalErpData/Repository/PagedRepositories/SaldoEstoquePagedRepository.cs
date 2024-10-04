using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class SaldoEstoquePagedRepository: GenericPagedRepository<SaldoEstoque, GlobalErpFiscalBaseContext, int, SaldoEstoqueDto>
    {
        public SaldoEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<SaldoEstoque, GlobalErpFiscalBaseContext, int, SaldoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<SaldoEstoque>> GetSaldoEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<SaldoEstoque>().Where(e => e.CdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<SaldoEstoque>().AsQueryable());
            }
        }
    }
}
