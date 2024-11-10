using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalLib.Repository;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class FreteRepository: GenericPagedRepository<Frete, GlobalErpFiscalBaseContext, int, FreteDto>
    {
        public FreteRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Frete, GlobalErpFiscalBaseContext, int, FreteDto>> logger) : base(injectedContext, mapper, logger)
        { }

        public Task<IQueryable<Frete>> GetFretePorSaida(int nrSaida)
        {
            return Task.FromResult(db.Set<Frete>().Where(e => e.NrSaida == nrSaida).AsQueryable());
        }
    }
}
