using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;

namespace GlobalErpData.Repository.Repositories
{
    public class CidadeRepositoryDto : GenericRepositoryDto<Cidade, GlobalErpFiscalBaseContext, string, CidadeDto>
    {
        public CidadeRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cidade, GlobalErpFiscalBaseContext, string, CidadeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
