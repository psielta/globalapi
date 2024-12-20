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
    public class TipoNfRepository : GenericRepositoryDto<TipoNf, GlobalErpFiscalBaseContext, string, TipoNfDto>
    {
        public TipoNfRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<TipoNf, GlobalErpFiscalBaseContext, string, TipoNfDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
