using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using Npgsql.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class SangriaCaixaRepository : GenericPagedRepositoryMultiKey<SangriaCaixa, GlobalErpFiscalBaseContext, int, int, SangriaCaixaDto>
    {
        public SangriaCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<SangriaCaixa, GlobalErpFiscalBaseContext, int, int, SangriaCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
