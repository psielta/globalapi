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
    public class CfopCsosnV2Repository : GenericPagedRepository<CfopCsosnV2, GlobalErpFiscalBaseContext, int, CfopCsosnV2Dto>
    {
        public CfopCsosnV2Repository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<CfopCsosnV2, GlobalErpFiscalBaseContext, int, CfopCsosnV2Dto>> logger) : base(injectedContext, mapper, logger)
        {
        }
        public Task<IQueryable<CfopCsosnV2>> GetCfopCsosnV2AsyncPorEmpresa(int IdEmpresa)
        {
            return Task.FromResult(db.Set<CfopCsosnV2>().Where(e => e.IdEmpresa == IdEmpresa).AsQueryable());
        }
    }
}
