using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// Representa os dados de uma Categoria retornada pela API.
    /// </summary>
    public class CategoriaDto
    {
        [JsonProperty("id_categoria")]
        public int IdCategoria { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("inicio")]
        public string Inicio { get; set; }

        [JsonProperty("fim")]
        public string Fim { get; set; }

        [JsonProperty("ativo")]
        public int Ativo { get; set; }

        [JsonProperty("opcao_meia")]
        public string OpcaoMeia { get; set; }

        [JsonProperty("disponivel")]
        public DisponibilidadeDto Disponivel { get; set; }

        
    }

    /// <summary>
    /// DTO para criação de categoria (POST /novo).
    /// </summary>
    public class CategoriaNovoDto
    {
        [JsonProperty("id_culinaria")]
        public int IdCulinaria { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("opcao_meia")]
        public string OpcaoMeia { get; set; }

        // Exemplo de array ou object. Ajuste conforme necessidade.
        [JsonProperty("opcoes")]
        public List<object> Opcoes { get; set; }

        [JsonProperty("disponivel")]
        public DisponibilidadeDto Disponivel { get; set; }

        [JsonProperty("inicio")]
        public string Inicio { get; set; }

        [JsonProperty("fim")]
        public string Fim { get; set; }
    }

    /// <summary>
    /// DTO para alteração de categoria (PUT /alterar).
    /// </summary>
    public class CategoriaAlterarDto
    {
        [JsonProperty("id_culinaria")]
        public int? IdCulinaria { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("opcao_meia")]
        public string OpcaoMeia { get; set; }

        [JsonProperty("disponivel")]
        public DisponibilidadeDto Disponivel { get; set; }

        [JsonProperty("inicio")]
        public string Inicio { get; set; }

        [JsonProperty("fim")]
        public string Fim { get; set; }
    }

    /// <summary>
    /// Exemplo de DTO para o objeto "disponivel".
    /// </summary>
    public class DisponibilidadeDto
    {
        [JsonProperty("domingo")]
        public int Domingo { get; set; }

        [JsonProperty("segunda")]
        public int Segunda { get; set; }

        [JsonProperty("terca")]
        public int Terca { get; set; }

        [JsonProperty("quarta")]
        public int Quarta { get; set; }

        [JsonProperty("quinta")]
        public int Quinta { get; set; }

        [JsonProperty("sexta")]
        public int Sexta { get; set; }

        [JsonProperty("sabado")]
        public int Sabado { get; set; }
    }

    /// <summary>
    /// Modelo genérico para capturar "success" e "message" em respostas da API.
    /// </summary>
    public class BaseResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        // Algumas rotas de criação retornam também "id_categoria", por exemplo
        [JsonProperty("id_categoria")]
        public int? IdCategoria { get; set; }
    }
}
