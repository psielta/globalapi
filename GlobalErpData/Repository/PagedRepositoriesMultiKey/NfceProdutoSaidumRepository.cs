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
    public class NfceProdutoSaidumRepository : GenericPagedRepositoryMultiKey<NfceProdutoSaidum, GlobalErpFiscalBaseContext, int, int, NfceProdutoSaidumDto>
    {
        public NfceProdutoSaidumRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceProdutoSaidum, GlobalErpFiscalBaseContext, int, int, NfceProdutoSaidumDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
