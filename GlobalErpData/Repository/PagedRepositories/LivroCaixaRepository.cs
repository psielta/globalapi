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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class LivroCaixaRepository : GenericPagedRepository<LivroCaixa, GlobalErpFiscalBaseContext, long, LivroCaixaDto>
    {
        public LivroCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<LivroCaixa, GlobalErpFiscalBaseContext, long, LivroCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<LivroCaixa>> GetLivroCaixaAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<LivroCaixa>().Where(e => e.CdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
