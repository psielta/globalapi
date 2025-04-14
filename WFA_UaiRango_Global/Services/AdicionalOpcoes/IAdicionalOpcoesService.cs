using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.AdicionalOpcoes
{
    public interface IAdicionalOpcoesService
    {
        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}
        /// </summary>
        Task<List<AdicionalOpcaoDto>?> ListarOpcoesAdicionalAsync(string token, int idEstabelecimento, int idTipo);

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/opcao/{id_adicional}
        /// </summary>
        Task<AdicionalOpcaoDto?> ObterOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional);

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/novo
        /// </summary>
        Task<int?> CriarOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, OpcaoAdicionalNovoDto dados);

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/alterar/{id_adicional}
        /// </summary>
        Task<bool> AlterarOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional, OpcaoAdicionalNovoDto dados);

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/opcao/{id_adicional}/status/{status}
        /// </summary>
        Task<bool> AlterarStatusOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional, int status);
    }
}
