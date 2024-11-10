using AutoMapper;
using GlobalErpData.Data;
using GlobalLib.Repository;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.Repositories
{
    public class EmpresaRepositoryDto : GenericRepositoryDto<Empresa, GlobalErpFiscalBaseContext, int, EmpresaDto>
    {
        public EmpresaRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Empresa, GlobalErpFiscalBaseContext, int, EmpresaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
