using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Adicional
{
    public interface IAdicionalService
    {
        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionais
        /// </summary>
        Task<List<CategoriaAdicionalDto>?> ListarAdicionaisPorEstabelecimentoAsync(string token, int idEstabelecimento);

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionais/{id_categoria}
        /// </summary>
        Task<CategoriaAdicionalDto?> ListarAdicionaisPorCategoriaAsync(string token, int idEstabelecimento, int idCategoria);

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicional/{id_tipo}
        /// </summary>
        Task<TipoAdicionalDto?> ObterAdicionalPorTipoAsync(string token, int idEstabelecimento, int idTipo);

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/novo
        /// </summary>
        Task<int?> CriarAdicionalAsync(string token, int idEstabelecimento, int idCategoria, AdicionalNovoDto dados);

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/alterar/{id_tipo}
        /// </summary>
        Task<bool> AlterarAdicionalAsync(string token, int idEstabelecimento, int idCategoria, int idTipo, AdicionalAlterarDto dados);

        /// <summary>
        /// DELETE /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/remover/{id_tipo}
        /// </summary>
        Task<bool> RemoverAdicionalAsync(string token, int idEstabelecimento, int idCategoria, int idTipo);
    }
}
