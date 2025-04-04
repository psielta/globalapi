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

namespace GlobalErpData.Repository.PagedRepositoriesMultiKey
{
    public class UsuarioFuncionarioRepository : GenericPagedRepositoryMultiKey<UsuarioFuncionario, GlobalErpFiscalBaseContext, int, string, UsuarioFuncionarioDto>
    {
        public UsuarioFuncionarioRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<UsuarioFuncionario, GlobalErpFiscalBaseContext, int, string, UsuarioFuncionarioDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<UsuarioFuncionario?> GetUsuarioFuncionarioByUsuario(string nmUsuario)
        {
            var usuarioFuncionario = await db.UsuarioFuncionarios.Where(o => o.NmUsuario.Equals(nmUsuario)).FirstOrDefaultAsync();

            return usuarioFuncionario;
        }
    }
}
