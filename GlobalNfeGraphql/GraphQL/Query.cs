using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalNfeGraphql.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Empresa> GetEmpresas([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.Empresas;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        //[UsePaging(IncludeTotalCount = true, ConnectionName = "ProdutoEstoqueConnection")]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ProdutoEstoque> GetProdutos([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.ProdutoEstoques;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<GrupoEstoque> GetGrupos([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.GrupoEstoques;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ReferenciaEstoque> GetReferencias([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.ReferenciaEstoques;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<UnidadeMedida> GetUnidades([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.UnidadeMedidas;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Usuario> GetUsuarios([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.Usuarios;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<UsuarioPermissao> GetUsuarioPermissaos([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.UsuarioPermissaos;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Permissao> GetPermissaos([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.Permissaos;
        }
        [UseDbContext(typeof(GlobalErpFiscalBaseContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Cidade> GetCidades([ScopedService] GlobalErpFiscalBaseContext context)
        {
            return context.Cidades;
        }
    }

}
