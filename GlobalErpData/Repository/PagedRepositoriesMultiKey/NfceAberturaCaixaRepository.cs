using AutoMapper;
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
    public class NfceAberturaCaixaRepository : GenericPagedRepositoryMultiKey<NfceAberturaCaixa, GlobalErpFiscalBaseContext, int, int, NfceAberturaCaixaDto>
    {
        public NfceAberturaCaixaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceAberturaCaixa, GlobalErpFiscalBaseContext, int, int, NfceAberturaCaixaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public virtual Task<IQueryable<NfceAberturaCaixa>> RetrieveAllAsync()
        {
            try
            {
                // Retorna IQueryable direto do EF
                return Task.FromResult(db
                    .Set<NfceAberturaCaixa>()
                    .Include(e => e.CdEmpresaNavigation)
                    .AsQueryable());
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<NfceAberturaCaixa>().AsQueryable());
            }
        }

        public virtual async Task<NfceAberturaCaixa?> RetrieveAsync(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<NfceAberturaCaixa>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<NfceAberturaCaixa>()
                    .Include(e => e.CdEmpresaNavigation)
                    .SingleOrDefaultAsync(e =>
                        EF.Property<int>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<int>(e, keyName2).Equals(idCadastro));

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }

        public virtual async Task<NfceAberturaCaixa?> RetrieveAsyncAsNoTracking(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<NfceAberturaCaixa>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<NfceAberturaCaixa>()
                    .AsNoTracking()
                    .Include(e => e.CdEmpresaNavigation)
                    .SingleOrDefaultAsync(e =>
                        EF.Property<int>(e, keyName1).Equals(idEmpresa) &&
                        EF.Property<int>(e, keyName2).Equals(idCadastro));

                return entity;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, idCadastro);
                return null;
            }
        }
    }
}
