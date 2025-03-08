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

namespace GlobalErpData.Repository.Repositories
{
    public class UnityRepository : GenericRepositoryDto<Unity, GlobalErpFiscalBaseContext, int, UnityDto>
    {
        public UnityRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Unity, GlobalErpFiscalBaseContext, int, UnityDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
