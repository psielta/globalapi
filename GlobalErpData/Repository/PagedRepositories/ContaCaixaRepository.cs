using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using GlobalLib.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContaCaixaRepository : GenericPagedRepository<ContaDoCaixa, GlobalErpFiscalBaseContext, int, ContaCaixaDto>
    {
        public ContaCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContaDoCaixa, GlobalErpFiscalBaseContext, int, ContaCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContaDoCaixa>> GetContaDoCaixaAsyncPorUnity(int unity)
        {
            return Task.FromResult(db.Set<ContaDoCaixa>()
                .Include(c => c.CdEmpresaNavigation)
                .Where(e => e.Unity == unity).AsQueryable());
        }

        public Task<IQueryable<ContaDoCaixa>> GetContaDoCaixaAsyncPorEmpresa(int Empresa)
        {
            return Task.FromResult(db.Set<ContaDoCaixa>()
                .Include(c => c.CdEmpresaNavigation)
                .Where(e => e.CdEmpresa == Empresa).AsQueryable());
        }
    }
}
