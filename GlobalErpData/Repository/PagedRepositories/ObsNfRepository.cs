﻿using AutoMapper;
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

namespace GlobalErpData.Repository.PagedRepositories
{
    public class ObsNfRepository : GenericPagedRepository<ObsNf, GlobalErpFiscalBaseContext, int, ObsNfDto>
    {
        public ObsNfRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<ObsNf, GlobalErpFiscalBaseContext, int, ObsNfDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<ObsNf>> GetObsNfAsyncPorUnity(int Unity)
        {
            return Task.FromResult(db.Set<ObsNf>().Where(e => e.Unity == Unity).AsQueryable());
        }
    }
}
