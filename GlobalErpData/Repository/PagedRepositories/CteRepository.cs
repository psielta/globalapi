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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class CteRepository : GenericPagedRepository<Cte, GlobalErpFiscalBaseContext, int, CteDto>
    {
        public CteRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cte, GlobalErpFiscalBaseContext, int, CteDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
