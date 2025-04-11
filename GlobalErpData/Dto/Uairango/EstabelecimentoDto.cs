using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Uairango.Dto
{
    /// <summary>
    /// Modelo para a resposta de vinculação de estabelecimento
    /// </summary>
    public class VincularEstabelecimentoResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("id_estabelecimento")]
        public int IdEstabelecimento { get; set; }
    }

    /// <summary>
    /// Modelo para a resposta de remoção de estabelecimento
    /// </summary>
    public class RemoverEstabelecimentoResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// Modelo para a resposta de verificação de vínculo
    /// </summary>
    public class CheckVinculoResponse
    {
        [JsonProperty("vinculado")]
        public bool Vinculado { get; set; }
    }

    /// <summary>
    /// Modelo para representar um estabelecimento
    /// </summary>
    public class Estabelecimento
    {
        [JsonProperty("id_estabelecimento")]
        public int IdEstabelecimento { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
