using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class CertificadoPagedRepository : GenericPagedRepository<Certificado, GlobalErpFiscalBaseContext, int, CertificadoDto>
    {
        public CertificadoPagedRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Certificado, GlobalErpFiscalBaseContext, int, CertificadoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<Certificado>> GetCertificadoPorEmpresa(int idEmpresa)
        {
            try
            {
                return Task.FromResult(db.Set<Certificado>().Where(e => e.IdEmpresa == idEmpresa).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Certificado>().AsQueryable());
            }
        }
    }
}
