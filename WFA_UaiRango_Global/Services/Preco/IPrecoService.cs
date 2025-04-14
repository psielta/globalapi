using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Preco
{
    public interface IPrecoService
    {
        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/preco/{id_preco}
        /// </summary>
        Task<PrecoDto?> ObterPrecoAsync(string token, int idEstabelecimento, int idPreco);

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/preco/{id_produto}/novo
        /// </summary>
        Task<int?> CriarPrecoAsync(string token, int idEstabelecimento, int idProduto, PrecoNovoDto dados);

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/preco/{id_preco}/alterar
        /// </summary>
        Task<bool> AlterarPrecoAsync(string token, int idEstabelecimento, int idPreco, PrecoAlterarDto dados);

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/preco/{id_preco}/status/{status}
        /// </summary>
        Task<bool> AlterarStatusPrecoAsync(string token, int idEstabelecimento, int idPreco, int status);
    }
}
