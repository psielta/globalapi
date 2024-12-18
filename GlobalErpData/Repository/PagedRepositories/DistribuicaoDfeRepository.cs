using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class DistribuicaoDfeRepository : GenericPagedRepository<DistribuicaoDfe, GlobalErpFiscalBaseContext, Guid, DistribuicaoDfeDto>
    {
        public DistribuicaoDfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<DistribuicaoDfe, GlobalErpFiscalBaseContext, Guid, DistribuicaoDfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
