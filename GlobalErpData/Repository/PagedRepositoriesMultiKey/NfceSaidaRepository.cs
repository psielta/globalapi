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
    public class NfceSaidaRepository : GenericPagedRepositoryMultiKey<NfceSaida, GlobalErpFiscalBaseContext, int, int, NfceSaidaDto>
    {
        public NfceSaidaRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericPagedRepositoryMultiKey<NfceSaida, GlobalErpFiscalBaseContext, int, int, NfceSaidaDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        public Task<IQueryable<NfceSaida>> RetrieveByUnity(int unity)
        {
            try
            {
                // Retorna IQueryable direto do EF
                return Task.FromResult(db.Set<NfceSaida>()
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
                    .Where(n => n.Unity == unity)
                    .AsQueryable());
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<NfceSaida>().AsQueryable());
            }
        }

        public override Task<IQueryable<NfceSaida>> RetrieveAllAsync()
        {
            try
            {
                // Retorna IQueryable direto do EF
                return Task.FromResult(db.Set<NfceSaida>()
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
                    .AsQueryable());
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving all entities.");
                return Task.FromResult(Enumerable.Empty<NfceSaida>().AsQueryable());
            }
        }

        public override async Task<NfceSaida?> RetrieveAsync(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<NfceSaida>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<NfceSaida>()
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
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

        public override async Task<NfceSaida?> RetrieveAsyncAsNoTracking(int idEmpresa, int idCadastro)
        {
            try
            {
                // Descobrindo dinamicamente os nomes de propriedade de chave
                var tempEntity = Activator.CreateInstance<NfceSaida>();
                string keyName1 = tempEntity.GetKeyName1(); // nome da chave 1
                string keyName2 = tempEntity.GetKeyName2(); // nome da chave 2

                // Consulta usando EF.Property<> para colunas com nomes dinâmicos
                var entity = await db.Set<NfceSaida>()
                    .Include(e => e.EmpresaNavigation)
                    .Include(e => e.ClienteNavigation)
                    .Include(e => e.NfceProdutoSaida)
                    .Include(e => e.NfceFormaPgts)
                    .AsNoTracking()
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
