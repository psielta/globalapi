using AutoMapper;
using GlobalPdvData.Data;
using GlobalLib.Repository;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;

namespace GlobalPdvData.Repository.Repositories
{
    public class EmpresaRepositoryDto : GenericRepositoryDto<Empresa, GlobalErpPdvV1Context, int, EmpresaDto>
    {
        public EmpresaRepositoryDto(GlobalErpPdvV1Context injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Empresa, GlobalErpPdvV1Context, int, EmpresaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
