using AutoMapper;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using GlobalPdvData.Data;

namespace GlobalErpData.Repository.Repositories
{
    public class PermissaoRepositoryDto : GenericRepositoryDto<Permissao, GlobalErpPdvV1Context, int, PermissaoDto>
    {
        public PermissaoRepositoryDto(GlobalErpPdvV1Context injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Permissao, GlobalErpPdvV1Context, int, PermissaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
