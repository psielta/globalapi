using AutoMapper;
using GlobalPdvData.Data;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;

namespace GlobalPdvData.Repository.Repositories
{
    public class CidadeRepositoryDto : GenericRepositoryDto<Cidade, GlobalErpPdvV1Context, string, CidadeDto>
    {
        public CidadeRepositoryDto(GlobalErpPdvV1Context injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cidade, GlobalErpPdvV1Context, string, CidadeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
