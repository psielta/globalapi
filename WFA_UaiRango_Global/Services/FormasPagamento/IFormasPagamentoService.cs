using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.FormasPagamento
{
    public interface IFormasPagamentoService
    {
        /// <summary>
        /// Retorna a lista das formas de pagamento disponíveis e seu status no estabelecimento.
        /// </summary>
        /// <param name="idEstabelecimento">ID do estabelecimento que será checado.</param>
        /// <param name="tipoEntrega">Tipo de entrega ("D" para delivery ou "R" para retirada).</param>
        /// <param name="token">Token JWT para autenticação.</param>
        /// <returns>Lista de formas de pagamento.</returns>
        Task<List<FormaPagamentoDto>> ObterFormasPagamentoAsync(int idEstabelecimento, string tipoEntrega, string token);

        /// <summary>
        /// Atualiza o status das formas de pagamento para um estabelecimento.
        /// Somente as formas enviadas serão ativadas, as demais serão desativadas.
        /// </summary>
        /// <param name="idEstabelecimento">ID do estabelecimento a ser alterado.</param>
        /// <param name="tipoEntrega">Tipo de entrega ("D" para delivery ou "R" para retirada).</param>
        /// <param name="formas">Lista dos IDs das formas de pagamento que devem ser ativadas.</param>
        /// <param name="token">Token JWT para autenticação.</param>
        /// <returns>Retorna true se a operação foi bem-sucedida; caso contrário, dispara exceção.</returns>
        Task<bool> AtualizarFormasPagamentoAsync(int idEstabelecimento, string tipoEntrega, List<int> formas, string token);
    }
}
