using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.Repositories
{
    public class UsuarioPermissaoRepositoryDto : GenericRepositoryDto<UsuarioPermissao, GlobalErpFiscalBaseContext, int, UsuarioPermissaoDto>
    {
        public UsuarioPermissaoRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<UsuarioPermissao, GlobalErpFiscalBaseContext, int, UsuarioPermissaoDto>> logger) : base(injectedContext, mapper, logger)
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
