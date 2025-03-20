using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class CfopImportacaoPagedRepository : GenericPagedRepository<CfopImportacao, GlobalErpFiscalBaseContext, int, CfopImportacaoDto>
    {
        public CfopImportacaoPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<CfopImportacao, GlobalErpFiscalBaseContext, int, CfopImportacaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<CfopImportacao>> GetCfopImportacaoPorUnity(int unity)
        {
            try
            {
                return Task.FromResult(db.Set<CfopImportacao>().Where(e => e.Unity == unity).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<CfopImportacao>().AsQueryable());
            }
        }
    }
}
