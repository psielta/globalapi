using AutoMapper;
using GlobalPdvData.Dto;
using GlobalPdvData.Models;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using GlobalLib.Repository;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalPdvData.Data;

namespace GlobalErpData.Repository.Repositories
{
    public class UsuarioPermissaoRepositoryDto : GenericRepositoryDto<UsuarioPermissao, GlobalErpPdvV1Context, int, UsuarioPermissaoDto>
    {
        public UsuarioPermissaoRepositoryDto(GlobalErpPdvV1Context injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<UsuarioPermissao, GlobalErpPdvV1Context, int, UsuarioPermissaoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<IEnumerable<UsuarioPermissao>> RetrieveAllAsyncPerUser(string NmUser)
        {
            return await db.Set<UsuarioPermissao>()
                .Include(e => e.IdPermissaoNavigation)
                .Where(e => e.IdUsuarioNavigation.NmUsuario.Equals(NmUser))
                .ToListAsync();
        }
    }
}
