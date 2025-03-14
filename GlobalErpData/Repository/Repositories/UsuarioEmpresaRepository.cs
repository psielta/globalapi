﻿using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<UsuarioEmpresa>> GetUsuarioEmpresaByUsername(string username)
        {
            return await db.Set<UsuarioEmpresa>().Where(ue => ue.CdUsuario.Equals(username)).ToListAsync();
        }

        public async Task<List<UsuarioEmpresa>?> RetrieveAllAsyncPerUser(string nmUsuario)
        {
            return await db.Set<UsuarioEmpresa>().Where(ue => ue.CdUsuario.Equals(nmUsuario)).ToListAsync();
        }
    }
}
