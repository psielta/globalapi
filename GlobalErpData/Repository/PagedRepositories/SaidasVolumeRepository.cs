using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class SaidasVolumeRepository : GenericPagedRepository<SaidasVolume, GlobalErpFiscalBaseContext, int, SaidasVolumeDto>
    {
        public SaidasVolumeRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<SaidasVolume, GlobalErpFiscalBaseContext, int, SaidasVolumeDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<SaidasVolume>> GetSaidasVolumePorSaida(int nrSaida)
        {
            return Task.FromResult(db.Set<SaidasVolume>().Where(e => e.NrSaida == nrSaida).AsQueryable());
        }
    }
}
