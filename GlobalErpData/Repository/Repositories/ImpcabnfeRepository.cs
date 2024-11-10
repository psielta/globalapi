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
    public class ImpcabnfeRepository : GenericRepositoryDto<Impcabnfe, GlobalErpFiscalBaseContext, string, ImpcabnfeDto>
    {
        public ImpcabnfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Impcabnfe, GlobalErpFiscalBaseContext, string, ImpcabnfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
