using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ControleNumeracaoNfeRepository : GenericPagedRepository<ControleNumeracaoNfe, GlobalErpFiscalBaseContext, int, ControleNumeracaoNfeDto>
    {
        public ControleNumeracaoNfeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ControleNumeracaoNfe, GlobalErpFiscalBaseContext, int, ControleNumeracaoNfeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ControleNumeracaoNfe>> GetControleNumeracaoNfeAsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<ControleNumeracaoNfe>().Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
