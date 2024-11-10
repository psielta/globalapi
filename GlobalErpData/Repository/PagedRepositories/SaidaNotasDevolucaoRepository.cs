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
    public class SaidaNotasDevolucaoRepository : GenericPagedRepository<SaidaNotasDevolucao, GlobalErpFiscalBaseContext, int, SaidaNotasDevolucaoDto>
    {
        public SaidaNotasDevolucaoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<SaidaNotasDevolucao, GlobalErpFiscalBaseContext, int, SaidaNotasDevolucaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<SaidaNotasDevolucao>> GetSaidaNotasDevolucaoPorSaida(int nrSaida)
        {
            return Task.FromResult(db.Set<SaidaNotasDevolucao>().Where(e => e.NrSaida == nrSaida).AsQueryable());
        }
    }
}
