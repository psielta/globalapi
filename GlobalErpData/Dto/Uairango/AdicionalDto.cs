using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// DTO retornado por "/auth/cardapio/{id_estabelecimento}/adicionais" e "/auth/cardapio/{id_estabelecimento}/adicionais/{id_categoria}"
    /// Representa a Categoria e dentro dela um array de Tipos.
    /// </summary>
    public class CategoriaAdicionalDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("id_categoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("adicionais")]
        public List<TipoAdicionalDto> Adicionais { get; set; }  // "adicionais" = array de tipos
    }

    /// <summary>
    /// DTO que representa um tipo de adicional: "id_tipo", "nome", "selecao", "opcoes", etc.
    /// Também usado pela rota GET /auth/cardapio/{id_estabelecimento}/adicional/{id_tipo}
    /// </summary>
    public class TipoAdicionalDto
    {
        [JsonProperty("id_tipo")]
        public int IdTipo { get; set; }

        [JsonProperty("codigo_tipo")]
        public string CodigoTipo { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        // "U" = único; "M" = múltiplo; "Q" = quantidade múltipla
        [JsonProperty("selecao")]
        public string Selecao { get; set; }

        // pode ser 0 ou um valor qualquer
        [JsonProperty("limite")]
        public int? Limite { get; set; }

        // 0 ou 1
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("opcoes")]
        public List<OpcaoAdicionalDto> Opcoes { get; set; }
    }

    /// <summary>
    /// DTO que representa cada "opção" de um tipo de adicional
    /// ex: "id_adicional", "nome", "valor", "status", etc.
    /// </summary>
    public class OpcaoAdicionalDto
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
    /// DTO para criar um novo adicional (POST).
    /// Exemplo de body:
    /// {
    ///   "nome": "Escolha um adicional",
    ///   "selecao": "Q",
    ///   "minimo": 0,
    ///   "limite": 1,
    ///   "opcoes": [
    ///     { "nome": "Chocolate", "valor": 1 },
    ///     { "nome": "Morango", "valor": 2, "codigo": "" }
    ///   ]
    /// }
    /// </summary>
    public class AdicionalNovoDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("selecao")]
        public string Selecao { get; set; }

        // Pode ser obrigatório quando selecao = "Q"
        [JsonProperty("minimo", NullValueHandling = NullValueHandling.Ignore)]
        public int? Minimo { get; set; }

        // "limite" pode ser nulo ou zero dependendo da lógica
        [JsonProperty("limite", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limite { get; set; }

        [JsonProperty("opcoes")]
        public List<AdicionalOpcaoNovoDto> Opcoes { get; set; }
    }

    /// <summary>
    /// Sub-DTO para cada item do array "opcoes" no AdicionalNovoDto.
    /// Exemplo: { "nome": "Chocolate", "valor": 1, "codigo": "" }
    /// </summary>
    public class AdicionalOpcaoNovoDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("valor")]
        public decimal? Valor { get; set; }

        [JsonProperty("codigo", NullValueHandling = NullValueHandling.Ignore)]
        public string Codigo { get; set; }
    }

    /// <summary>
    /// DTO para alteração de um adicional (PUT).
    /// Exemplo:
    /// {
    ///   "codigo_tipo": "codtipoadicional",
    ///   "nome": "Teste Adicional",
    ///   "selecao": "M",
    ///   "limite": 8,
    ///   "opcoes": [ ... ] // se a API exigir
    /// }
    /// Observando que a doc fala também em 'opcoes' para alterar (similar ao POST).
    /// </summary>
    public class AdicionalAlterarDto
    {
        [JsonProperty("codigo_tipo", NullValueHandling = NullValueHandling.Ignore)]
        public string CodigoTipo { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("selecao")]
        public string Selecao { get; set; }

        [JsonProperty("minimo", NullValueHandling = NullValueHandling.Ignore)]
        public int? Minimo { get; set; }

        [JsonProperty("limite", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limite { get; set; }

        // O doc menciona "opcoes" mas nem sempre é obrigatório.
        // Depende se iremos alterar as opções também.
        [JsonProperty("opcoes", NullValueHandling = NullValueHandling.Ignore)]
        public List<AdicionalOpcaoNovoDto> Opcoes { get; set; }
    }

    /// <summary>
    /// Modelo genérico para { "success": bool, "message": "", "id_tipo": N }
    /// </summary>
    public class BaseResponseAdicionalDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Exemplo no POST /novo -> "id_tipo"
        [JsonProperty("id_tipo")]
        public int? IdTipo { get; set; }
    }
}
