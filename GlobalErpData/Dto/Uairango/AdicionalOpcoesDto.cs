using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// DTO que representa cada opção de adicional (rota: /adicionalOpcoes).
    /// Exemplo de retorno:
    /// {
    ///   "id_adicional": 196063,
    ///   "codigo": "",
    ///   "nome": "Creme de amendoin",
    ///   "valor": 50,
    ///   "status": 0
    /// }
    /// </summary>
    public class AdicionalOpcaoDto
    {
        [JsonProperty("id_adicional")]
        public int IdAdicional { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valor")]
        public decimal Valor { get; set; }

        // 0 ou 1
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO para criar/alterar uma nova opção de adicional (POST / PUT).
    /// Exemplo body:
    /// {
    ///   "nome": "Chantily",
    ///   "valor": 1.50
    /// }
    /// </summary>
    public class OpcaoAdicionalNovoDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valor")]
        public decimal Valor { get; set; }

        // Caso a API suportar "codigo", inclua se necessário
        // [JsonProperty("codigo", NullValueHandling = NullValueHandling.Ignore)]
        // public string Codigo { get; set; }
    }

    /// <summary>
    /// Modelo genérico para respostas do tipo:
    /// { "success": bool, "message": "", "id_adicional": N }
    /// Exemplo em:
    /// { "success": true, "id_adicional": 436789, "message": "Opcao de adicional cadastrado com sucesso!" }
    /// </summary>
    public class BaseResponseOpcaoAdicionalDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("id_adicional")]
        public int? IdAdicional { get; set; }
    }
}