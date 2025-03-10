using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;

namespace GlobalErpData.Repository.Repositories
{
    public class UsuarioRepositoryDto : GenericRepositoryDto<Usuario, GlobalErpFiscalBaseContext, string, UsuarioDto>
    {
        public UsuarioRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Usuario, GlobalErpFiscalBaseContext, string, UsuarioDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public IQueryable<Usuario> GetUsuariosAsyncPerUnity(int unity)
        {
            return db.Set<Usuario>()
                .Where(e => e.Unity == unity)
                .AsQueryable();
        }
    }
}
