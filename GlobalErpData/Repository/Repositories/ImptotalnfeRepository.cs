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
    public class ImptotalnfeRepository : GenericRepositoryDto<Imptotalnfe, GlobalErpFiscalBaseContext, string, ImptotalnfeDto>
    {
        public ImptotalnfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Imptotalnfe, GlobalErpFiscalBaseContext, string, ImptotalnfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
