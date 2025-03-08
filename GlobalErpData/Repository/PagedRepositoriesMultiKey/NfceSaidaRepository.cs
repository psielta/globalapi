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
    public class NfceSaidaRepository : GenericPagedRepositoryMultiKey<NfceSaida, GlobalErpFiscalBaseContext, int, int, NfceSaidaDto>
    {
        public NfceSaidaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceSaida, GlobalErpFiscalBaseContext, int, int, NfceSaidaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
