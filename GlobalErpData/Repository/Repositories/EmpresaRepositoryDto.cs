using AutoMapper;
using GlobalErpData.Data;
using GlobalLib.Repository;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Repository.Repositories
{
    public class EmpresaRepositoryDto : GenericRepositoryDto<Empresa, GlobalErpFiscalBaseContext, int, EmpresaDto>
    {
        public EmpresaRepositoryDto(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Empresa, GlobalErpFiscalBaseContext, int, EmpresaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public async Task<List<Empresa>> GetEmpresasByUnity(int unity)
        {
            return await db.Empresas.Where(e => e.Unity == unity).ToListAsync();
        }
        
        public IQueryable<Empresa> GetQueryableEmpresasByUnity(int unity)
        {
            return db.Empresas.Where(e => e.Unity == unity).AsQueryable();
        }

        public async Task<List<Empresa>> RetrieveAllAsyncPerUsuarioEmpresas(List<UsuarioEmpresa> usuarioEmpresas)
        {
            List<int> empresasIds = usuarioEmpresas.Select(ue => ue.CdEmpresa).ToList();
            return await db.Empresas.Where(e => empresasIds.Contains(e.CdEmpresa)).ToListAsync();
        }
    }
}
