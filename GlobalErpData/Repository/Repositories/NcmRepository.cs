using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using GlobalLib.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class NcmRepository : GenericRepositoryDto<Ncm, GlobalErpFiscalBaseContext, int, NcmDto>
    {
        public NcmRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Ncm, GlobalErpFiscalBaseContext, int, NcmDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
