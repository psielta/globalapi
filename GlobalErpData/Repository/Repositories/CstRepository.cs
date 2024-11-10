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
    public class CstRepository : GenericRepositoryDto<Cst, GlobalErpFiscalBaseContext, string, CstDto>
    {
        public CstRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cst, GlobalErpFiscalBaseContext, string, CstDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
