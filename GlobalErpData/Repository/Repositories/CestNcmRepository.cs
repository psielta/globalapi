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

namespace GlobalErpData.Repository.Repositories
{
    public class CestNcmRepository : GenericRepositoryDto<CestNcm, GlobalErpFiscalBaseContext, int, CestNcmDto>
    {
        public CestNcmRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<CestNcm, GlobalErpFiscalBaseContext, int, CestNcmDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
