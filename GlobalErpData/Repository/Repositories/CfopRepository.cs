using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class CfopRepository : GenericRepositoryDto<Cfop, GlobalErpFiscalBaseContext, string, CfopDto>
    {
        public CfopRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cfop, GlobalErpFiscalBaseContext, string, CfopDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
