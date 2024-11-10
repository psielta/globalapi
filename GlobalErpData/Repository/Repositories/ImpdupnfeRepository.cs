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
    public class ImpdupnfeRepository : GenericRepositoryDto<Impdupnfe, GlobalErpFiscalBaseContext, string, ImpdupnfeDto>
    {
        public ImpdupnfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Impdupnfe, GlobalErpFiscalBaseContext, string, ImpdupnfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
