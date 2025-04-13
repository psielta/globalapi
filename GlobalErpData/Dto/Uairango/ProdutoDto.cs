using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// Representa um Produto retornado pela API
    /// </summary>
    public class ProdutoDto
    {
        [JsonProperty("id_produto")]
        public int IdProduto { get; set; }

        [JsonProperty("codigo_produto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        // 0 ou 1
        [JsonProperty("status")]
        public int Status { get; set; }

        // Lista de opções com preços
        [JsonProperty("opcoes")]
        public List<ProdutoOpcaoDto> Opcoes { get; set; }
    }

    /// <summary>
    /// Representa o objeto 'opcoes' dentro de um Produto.
    /// </summary>
    public class ProdutoOpcaoDto
    {
        [JsonProperty("id_preco")]
        public int IdPreco { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("id_opcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("codigo_opcao")]
        public string CodigoOpcao { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        // Valor normal
        [JsonProperty("valor")]
        public decimal? Valor { get; set; }

        // Valor2, se a API retornar
        [JsonProperty("valor2")]
        public decimal? Valor2 { get; set; }

        // O valorAtual pode ser calculado pela API
        [JsonProperty("valorAtual")]
        public decimal? ValorAtual { get; set; }

        // 0 ou 1
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO para criar novo produto (POST).
    /// </summary>
    public class ProdutoNovoDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        // A API exige "opcoes", que é um array de objetos
        [JsonProperty("opcoes")]
        public List<ProdutoOpcaoCriarDto> Opcoes { get; set; }
    }

    /// <summary>
    /// DTO para atualizar produto (PUT).
    /// </summary>
    public class ProdutoAlterarDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        // Similar ao DTO de criação, mas a API pede "opcoes" com o "id_opcao", "codigo" e "preco"
        [JsonProperty("opcoes")]
        public List<ProdutoOpcaoCriarDto> Opcoes { get; set; }
    }

    /// <summary>
    /// Sub-DTO para informar as opções na criação/alteração.
    /// (id_opcao, codigo, preco)
    /// </summary>
    public class ProdutoOpcaoCriarDto
    {
        [JsonProperty("id_opcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        // "preco" é obrigatório conforme a documentação
        [JsonProperty("preco")]
        public decimal Preco { get; set; }
    }

    /// <summary>
    /// Modelo base para capturar "success", "message" e possivelmente "id_produto".
    /// </summary>
    public class BaseResponseProdutoDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Algumas rotas retornam o ID criado
        [JsonProperty("id_produto")]
        public int? IdProduto { get; set; }
    }
}
