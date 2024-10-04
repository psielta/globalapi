using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class CfopImportacaoPagedRepository : GenericPagedRepository<CfopImportacao, GlobalErpFiscalBaseContext, int, CfopImportacaoDto>
    {
        public CfopImportacaoPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<CfopImportacao, GlobalErpFiscalBaseContext, int, CfopImportacaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<CfopImportacao>> GetCfopImportacaoPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<CfopImportacao>().Where(e => e.IdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<CfopImportacao>().AsQueryable());
            }
        }
    }
}
