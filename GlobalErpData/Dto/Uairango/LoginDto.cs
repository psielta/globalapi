using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Uairango.Dto
{
    /// <summary>
    /// Modelo para a requisição de login
    /// </summary>
    public class LoginUaiRangoRequestDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    /// <summary>
    /// Modelo para a resposta do login
    /// </summary>
    public class LoginUaiRangoResponseDto
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
