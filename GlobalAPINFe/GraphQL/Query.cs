using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.GraphQL
{
    public class Query
    {
        private readonly GlobalErpFiscalBaseContext _context;

        public Query(GlobalErpFiscalBaseContext context)
        {
            _context = context;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Empresa> GetEmpresas()
        {
            return _context.Empresas;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<ProdutoEstoque> GetProdutos()
        {
            return _context.ProdutoEstoques;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<GrupoEstoque> GetGrupos()
        {
            return _context.GrupoEstoques;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<ReferenciaEstoque> GetReferencias()
        {
            return _context.ReferenciaEstoques;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<UnidadeMedida> GetUnidades()
        {
            return _context.UnidadeMedidas;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Usuario> GetUsuarios()
        {
            return _context.Usuarios;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<UsuarioPermissao> GetUsuarioPermissaos()
        {
            return _context.UsuarioPermissaos;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Permissao> GetPermissaos()
        {
            return _context.Permissaos;
        }

        [UseFiltering]
        [UseSorting]
        public IQueryable<Cidade> GetCidades()
        {
            return _context.Cidades;
        }
    }
}
