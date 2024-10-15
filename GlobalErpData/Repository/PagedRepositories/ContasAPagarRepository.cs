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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContasAPagarRepository : GenericPagedRepository<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>
    {
        public ContasAPagarRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContasAPagar, GlobalErpFiscalBaseContext, int, ContasAPagarDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
