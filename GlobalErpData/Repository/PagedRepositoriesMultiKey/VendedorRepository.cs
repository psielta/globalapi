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
    public class VendedorRepository : GenericPagedRepositoryMultiKey<Vendedor, GlobalErpFiscalBaseContext, int, int, VendedorDto>
    {
        public VendedorRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<Vendedor, GlobalErpFiscalBaseContext, int, int, VendedorDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
