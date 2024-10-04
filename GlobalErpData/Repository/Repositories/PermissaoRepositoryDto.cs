using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.Repositories
{
    public class PermissaoRepositoryDto : GenericRepositoryDto<Permissao, GlobalErpFiscalBaseContext, int, PermissaoDto>
    {
        public PermissaoRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Permissao, GlobalErpFiscalBaseContext, int, PermissaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
