using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Produto
{
    public interface IProdutoService
    {
        Task<List<ProdutoDto>?> ObterProdutosDaCategoriaAsync(string token, int idEstabelecimento, int idCategoria);

        Task<ProdutoDto?> ObterProdutoAsync(string token, int idEstabelecimento, int idProduto);

        Task<int?> CriarProdutoAsync(string token, int idEstabelecimento, int idCategoria, ProdutoNovoDto dados);

        Task<bool> AlterarProdutoAsync(string token, int idEstabelecimento, int idCategoria, int idProduto, ProdutoAlterarDto dados);

        Task<bool> AlterarStatusProdutoAsync(string token, int idEstabelecimento, int idProduto, int status);
    }
}
