using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Config
{
    public interface IConfigService
    {
        /// <summary>
        /// Busca informações de um estabelecimento específico.
        /// </summary>
        /// <param name="token">Token de autenticação (Bearer)</param>
        /// <param name="idEstabelecimento">ID do estabelecimento</param>
        /// <returns>Objeto com informações do estabelecimento</returns>
        Task<EstabelecimentoConfigDto> ObterEstabelecimentoAsync(string token, int idEstabelecimento);

        /// <summary>
        /// Obtém todos os prazos disponíveis (id_tempo, min, max, etc.).
        /// </summary>
        /// <param name="token">Token de autenticação (Bearer)</param>
        /// <returns>Lista de prazos possíveis</returns>
        Task<List<PrazoDto>> ObterPrazosAsync(string token);

        /// <summary>
        /// Altera uma informação de um estabelecimento. Ex: PUT /auth/info/{id_estabelecimento}/status_estabelecimento/1
        /// </summary>
        /// <param name="token">Token de autenticação (Bearer)</param>
        /// <param name="idEstabelecimento">ID do estabelecimento</param>
        /// <param name="campo">Campo a ser alterado (status_estabelecimento, status_delivery, etc.)</param>
        /// <param name="valor">Valor a ser definido (0 ou 1, ou id_tempo, etc.)</param>
        /// <returns>True se sucesso, caso contrário lança exceção ou retorna false (dependendo da sua abordagem)</returns>
        Task<bool> AtualizarEstabelecimentoAsync(string token, int idEstabelecimento, string campo, string valor);
    }
}
