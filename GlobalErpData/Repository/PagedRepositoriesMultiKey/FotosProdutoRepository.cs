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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class FotosProdutoRepository : GenericPagedRepositoryMultiKey<FotosProduto, GlobalErpFiscalBaseContext, int, int, FotosProdutoDto>
    {
        public FotosProdutoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<FotosProduto, GlobalErpFiscalBaseContext, int, int, FotosProdutoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
