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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ContaCaixaRepository : GenericPagedRepository<ContaDoCaixa, GlobalErpFiscalBaseContext, int, ContaCaixaDto>
    {
        public ContaCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ContaDoCaixa, GlobalErpFiscalBaseContext, int, ContaCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ContaDoCaixa>> GetContaDoCaixaAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ContaDoCaixa>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
