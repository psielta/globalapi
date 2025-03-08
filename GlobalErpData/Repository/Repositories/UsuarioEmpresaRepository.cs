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
    public class UsuarioEmpresaRepository : GenericRepositoryDto<UsuarioEmpresa, GlobalErpFiscalBaseContext, int, UsuarioEmpresaDto>
    {
        public UsuarioEmpresaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<UsuarioEmpresa, GlobalErpFiscalBaseContext, int, UsuarioEmpresaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }
    }
}
