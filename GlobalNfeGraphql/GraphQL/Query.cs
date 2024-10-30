using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalNfeGraphql.GraphQL
{
    public class Query
    {
        [UseFiltering]
        [UseSorting]
        public IQueryable<Empresa> GetEmpresas(GlobalErpFiscalBaseContext context)
        {
            return context.Empresas;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<ProdutoEstoque> GetProdutos(GlobalErpFiscalBaseContext context)
        {
            return context.ProdutoEstoques;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<GrupoEstoque> GetGrupos(GlobalErpFiscalBaseContext context)
        {
            return context.GrupoEstoques;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<ReferenciaEstoque> GetReferencias(GlobalErpFiscalBaseContext context)
        {
            return context.ReferenciaEstoques;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<UnidadeMedida> GetUnidades(GlobalErpFiscalBaseContext context)
        {
            return context.UnidadeMedidas;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<Usuario> GetUsuarios(GlobalErpFiscalBaseContext context)
        {
            return context.Usuarios;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<UsuarioPermissao> GetUsuarioPermissaos(GlobalErpFiscalBaseContext context)
        {
            return context.UsuarioPermissaos;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<Permissao> GetPermissaos( GlobalErpFiscalBaseContext context)
        {
            return context.Permissaos;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<Cidade> GetCidades(GlobalErpFiscalBaseContext context)
        {
            return context.Cidades;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Entrada> GetEntradas(GlobalErpFiscalBaseContext context)
        {
            return context.Entradas;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<ProdutoEntradum> GetProdutoEntrada(GlobalErpFiscalBaseContext context)
        {
            return context.ProdutoEntrada;
        }
        
        [UseFiltering]
        [UseSorting]
        public IQueryable<ContasAPagar> GetContasAPagar(GlobalErpFiscalBaseContext context)
        {
            return context.ContasAPagars;
        }
    }

}
