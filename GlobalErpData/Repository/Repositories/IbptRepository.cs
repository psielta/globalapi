using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class IbptRepository : GenericRepositoryDto<Ibpt, GlobalErpFiscalBaseContext, int, IbptDto>
    {
        public IbptRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Ibpt, GlobalErpFiscalBaseContext, int, IbptDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
