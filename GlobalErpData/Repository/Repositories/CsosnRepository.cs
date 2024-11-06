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
    public class CsosnRepository : GenericRepositoryDto<Csosn, GlobalErpFiscalBaseContext, string, CsosnDto>
    {
        public CsosnRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Csosn, GlobalErpFiscalBaseContext, string, CsosnDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
