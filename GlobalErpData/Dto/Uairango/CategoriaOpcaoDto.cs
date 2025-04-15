using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    public class CategoriaOpcaoDto
    {
        [JsonProperty("id_opcao")]
        public int IdOpcao { get; set; }

        [JsonProperty("codigo_opcao")]
        public string CodigoOpcao { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        // 0 ou 1, indica se está desabilitado/habilitado
        [JsonProperty("status")]
        public int Status { get; set; }
    }

    /// <summary>
    /// DTO para criar nova opção (POST).
    /// A API exige apenas 'nome'. Pode ter mais campos, como 'codigo_opcao', se desejar.
    /// </summary>
    public class CategoriaOpcaoNovoDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        // Se quiser incluir "codigo_opcao" no body, pode fazer assim:
        [JsonProperty("codigo_opcao", NullValueHandling = NullValueHandling.Ignore)]
        public string CodigoOpcao { get; set; }
    }

    /// <summary>
    /// DTO para alterar a opção (PUT).
    /// Geralmente o corpo é igual ao da criação, mas pode ter diferenças se a API permitir mais campos.
    /// </summary>
    public class CategoriaOpcaoAlterarDto
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        //[JsonProperty("codigo_opcao", NullValueHandling = NullValueHandling.Ignore)]
        //public string CodigoOpcao { get; set; }
    }

    /// <summary>
    /// Modelo genérico para capturar "success" e "message" em respostas.
    /// (Pode ser o mesmo BaseResponseDto que você usa no outro serviço)
    /// </summary>
    public class BaseResponseCategoriaOpcaoDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Em alguns endpoints de criação, a API retorna o ID gerado
        [JsonProperty("id_opcao")]
        public int? IdOpcao { get; set; }
    }
}
