using AutoMapper;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using GlobalPdvData.Data;

namespace GlobalErpData.Repository.Repositories
{
    public class UsuarioRepositoryDto : GenericRepositoryDto<Usuario, GlobalErpPdvV1Context, string, UsuarioDto>
    {
        public UsuarioRepositoryDto(GlobalErpPdvV1Context injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Usuario, GlobalErpPdvV1Context, string, UsuarioDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsyncPerEmpresa(int IdEmpresa)
        {
            return await db.Set<Usuario>()
                .Where(e => e.CdEmpresa == IdEmpresa)
                .ToListAsync();
        }
    }
}
