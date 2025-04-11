using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalLib.Files;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GlobalErpData.Uairango.Dto;

namespace WFA_UaiRango_Global.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginService> _logger;
        private readonly HttpClient _httpClient;

        public LoginService(IConfiguration config, ILogger<LoginService> logger, HttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<LoginUaiRangoResponseDto> LoginAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando processo de login na API UaiRango");

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var loginEndpoint = $"{uaiRangoConfig.base_url}/login";

                var loginRequest = new LoginUaiRangoRequestDto
                {
                    Token = uaiRangoConfig.token_de_acesso
                };

                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogDebug("Enviando requisição para: {Endpoint}", loginEndpoint);
                var response = await _httpClient.PostAsync(loginEndpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao fazer login: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginUaiRangoResponseDto>(responseContent);

                _logger.LogInformation("Login realizado com sucesso");
                return loginResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar fazer login na API UaiRango");
                throw;
            }
        }
    }
}
