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
    public class NfceAberturaCaixaRepository : GenericPagedRepositoryMultiKey<NfceAberturaCaixa, GlobalErpFiscalBaseContext, int, int, NfceAberturaCaixaDto>
    {
        public NfceAberturaCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceAberturaCaixa, GlobalErpFiscalBaseContext, int, int, NfceAberturaCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
