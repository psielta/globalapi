using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// Representa o dado retornado pela rota GET /preco/{id_preco}.
    /// </summary>
    public class PrecoDto
    {
        [JsonProperty("id_preco")]
        public int IdPreco { get; set; }

        [JsonProperty("id_opcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valor")]
        public decimal? Valor { get; set; }

        [JsonProperty("valor2")]
        public decimal? Valor2 { get; set; }

        [JsonProperty("valorAtual")]
        public decimal? ValorAtual { get; set; }

        // 0 ou 1
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO para criar um novo preço (POST).
    /// A API exige no corpo algo como:
    /// {
    ///   "id_opcao": 160733,
    ///   "preco": 2
    /// }
    /// </summary>
    public class PrecoNovoDto
    {
        [JsonProperty("id_opcao", NullValueHandling = NullValueHandling.Ignore)]
        public int? IdOpcao { get; set; }

        // A API chama de "preco" (float), mas usaremos decimal
        [JsonProperty("preco")]
        public decimal Preco { get; set; }
    }

    /// <summary>
    /// DTO para alterar um preço (PUT).
    /// Exemplo de body { "preco": 3 }
    /// </summary>
    public class PrecoAlterarDto
    {
        [JsonProperty("preco")]
        public decimal Preco { get; set; }
    }

    /// <summary>
    /// Modelo genérico para "success", "message", e possivelmente "id_preco".
    /// </summary>
    public class PrecoBaseResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Algumas rotas retornam ID criado
        [JsonProperty("id_preco")]
        public int? IdPreco { get; set; }
    }
}
