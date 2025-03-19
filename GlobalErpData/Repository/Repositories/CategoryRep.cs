using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using GlobalLib.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class CategoryRep : GenericRepositoryDto<Category, GlobalErpFiscalBaseContext, int, CategoryDto>
    {
        public CategoryRep(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Category, GlobalErpFiscalBaseContext, int, CategoryDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public async Task<IEnumerable<Category>> GetCategorysByEmpresaAsync(int IdEmpresa)
        {
            return await db.Set<Category>()
                .Where(s => s.Unity == IdEmpresa)
                .ToListAsync();
        }
    }
}
