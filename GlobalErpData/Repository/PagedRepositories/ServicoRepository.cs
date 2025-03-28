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
    public class ServicoRepository : GenericPagedRepository<Servico, GlobalErpFiscalBaseContext, long, ServicoDto>
    {
        public ServicoRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Servico, GlobalErpFiscalBaseContext, long, ServicoDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public IQueryable<Servico> GetServicosPorUnity(int unity)
        {
            return db.Set<Servico>().Include(x => x.IdDepartamentoNavigation)
                .Where(e => e.Unity == unity)
                .AsQueryable();
        }

        public override Task<IQueryable<Servico>> RetrieveAllAsync()
        {
            try
            {
                // Retorna o IQueryable do EF, sem cache estático
                return Task.FromResult(db.Set<Servico>().Include(x => x.IdDepartamentoNavigation).AsQueryable());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<Servico>().AsQueryable());
            }
        }

        public override async Task<Servico?> RetrieveAsync(long id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<Servico>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<Servico>().Include(x => x.IdDepartamentoNavigation)
                    .SingleOrDefaultAsync(e => EF.Property<long>(e, keyName)!.Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }

        public override async Task<Servico?> RetrieveAsyncAsNoTracking(long id)
        {
            try
            {
                // Descobre dinamicamente o nome da propriedade de chave
                var tempEntity = Activator.CreateInstance<Servico>();
                var keyName = tempEntity.GetKeyName();

                // Filtra por EF.Property<>
                var entity = await db.Set<Servico>().AsNoTracking().Include(x => x.IdDepartamentoNavigation)
                    .SingleOrDefaultAsync(e => EF.Property<long>(e, keyName)!.Equals(id));

                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {Id}", id);
                return null;
            }
        }
    }
}
