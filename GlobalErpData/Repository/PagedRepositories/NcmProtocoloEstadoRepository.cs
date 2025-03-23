using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class NcmProtocoloEstadoRepository : GenericPagedRepository<NcmProtocoloEstado, GlobalErpFiscalBaseContext, int, NcmProtocoloEstadoDto>
    {
        public NcmProtocoloEstadoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<NcmProtocoloEstado, GlobalErpFiscalBaseContext, int, NcmProtocoloEstadoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<NcmProtocoloEstado>> GetNcmProtocoloEstadoAsyncPorEmpresa(int unity)
        {
            return Task.FromResult(db.Set<NcmProtocoloEstado>()
                .Include(n => n.IdCabProtocoloNavigation)
                .Where(e => e.Unity == unity).AsQueryable());
        }

        public async override Task<NcmProtocoloEstado?> CreateAsync(NcmProtocoloEstadoDto dto)
        {
            NcmProtocoloEstado entity = mapper.Map<NcmProtocoloEstado>(dto);
            EntityEntry<NcmProtocoloEstado> added = await db.Set<NcmProtocoloEstado>().AddAsync(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                return await db.Set<NcmProtocoloEstado>().Include(e => e.IdCabProtocoloNavigation)
                    .FirstOrDefaultAsync(e => e.Id == entity.Id);
            }
            else
            {
                logger.LogWarning("Failed to create entity from DTO.");
                return null;
            }
        }
        public async override Task<NcmProtocoloEstado?> UpdateAsync(int id, NcmProtocoloEstadoDto dto)
        {
            NcmProtocoloEstado entity = mapper.Map<NcmProtocoloEstado>(dto);
            entity.GetType().GetProperty(entity.GetKeyName())?.SetValue(entity, id);
            db.Set<NcmProtocoloEstado>().Update(entity);
            int affected = await db.SaveChangesAsync();
            if (affected == 1)
            {
                logger.LogInformation("Entity updated with ID: {Id}", id);
                return await db.Set<NcmProtocoloEstado>().Include(e => e.IdCabProtocoloNavigation)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                logger.LogWarning("Failed to update entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
