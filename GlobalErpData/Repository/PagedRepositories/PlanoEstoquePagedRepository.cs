using AutoMapper;
using GlobalErpData.Data;
using GlobalLib.Repository;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
                return Task.FromResult(db.Set<PlanoEstoque>().Where(e => e.CdEmpresa == idEmpresa)
                    .Include(p=> p.CdEmpresaNavigation)
                    .AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<PlanoEstoque>().AsQueryable());
            }
        }
        
        public Task<IQueryable<PlanoEstoque>> GetPlanoEstoquePorUnity(int unity)
        {
            try
            {
                return Task.FromResult(db.Set<PlanoEstoque>()
                    .Include(p=> p.CdEmpresaNavigation)
                    .Where(e => e.Unity == unity).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<PlanoEstoque>().AsQueryable());
            }
        }
    }
}
