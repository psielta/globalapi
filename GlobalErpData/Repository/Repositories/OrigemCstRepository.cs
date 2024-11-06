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
    public class OrigemCstRepository : GenericRepositoryDto<OrigemCst, GlobalErpFiscalBaseContext, string, OrigemCstDto>
    {
        public OrigemCstRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<OrigemCst, GlobalErpFiscalBaseContext, string, OrigemCstDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
