using AutoMapper;
using GlobalErpData.Data;
using GlobalLib.Repository;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class PlanoEstoquePagedRepository : GenericPagedRepository<PlanoEstoque, GlobalErpFiscalBaseContext, int, PlanoEstoqueDto>
    {
        public PlanoEstoquePagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<PlanoEstoque, GlobalErpFiscalBaseContext, int, PlanoEstoqueDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<PlanoEstoque>> GetPlanoEstoquePorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<PlanoEstoque>().Where(e => e.CdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<PlanoEstoque>().AsQueryable());
            }
        }
    }
}
