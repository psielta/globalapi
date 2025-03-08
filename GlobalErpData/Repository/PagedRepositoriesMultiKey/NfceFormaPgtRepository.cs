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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class NfceFormaPgtRepository : GenericPagedRepositoryMultiKey<NfceFormaPgt, GlobalErpFiscalBaseContext, int, int, NfceFormaPgtDto>
    {
        public NfceFormaPgtRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceFormaPgt, GlobalErpFiscalBaseContext, int, int, NfceFormaPgtDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
