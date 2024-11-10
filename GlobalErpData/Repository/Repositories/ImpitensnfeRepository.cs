using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using GlobalLib.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class ImpitensnfeRepository : GenericPagedRepositoryMultiKey<Impitensnfe, GlobalErpFiscalBaseContext, string, string, ImpitensnfeDto>
    {
        public ImpitensnfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Impitensnfe, GlobalErpFiscalBaseContext, string, string, ImpitensnfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
