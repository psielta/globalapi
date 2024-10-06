using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class ProductDetailRepository : GenericRepositoryDto<ProductDetail, GlobalErpFiscalBaseContext, int, ProductDetailDto>
    {
        public ProductDetailRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ProductDetail, GlobalErpFiscalBaseContext, int, ProductDetailDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsyncPerEmpresa(int IdEmpresa)
        {
            return await db.Set<ProductDetail>()
                .Where(e => e.IdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
