using System;
using Microsoft.Extensions.Configuration;

namespace GlobalLib.Files
{
    public struct SConfUaiRango
    {
        public string token_de_acesso { get; set; }
        public string base_url { get; set; }
        public string client_id { get; set; }

        public SConfUaiRango(string client_id, string base_url, string token_de_acesso)
        {
            this.token_de_acesso = token_de_acesso;
            this.client_id = client_id;
            this.base_url = base_url;
        }
    }

    public static class ConfiguracoesUaiRango
    {
        public static SConfUaiRango GetConfUaiRango(IConfiguration _config)
        {
            string baseUrl = _config["base_url_uairango"];
            string token = _config["token_de_acesso"];
            string clientId = _config["client_id"];

            return new SConfUaiRango(clientId, baseUrl, token);
        }
    }
}
