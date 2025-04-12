using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;
using GlobalLib.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static GlobalLib.Files.ConfiguracoesUaiRango;

namespace WFA_UaiRango_Global.Services.Config
{
    public class ConfigService : IConfigService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ConfigService> _logger;
        private readonly IConfiguration _config;
        private readonly SConfUaiRango _uaiRangoConfig;

        public ConfigService(
            HttpClient httpClient,
            ILogger<ConfigService> logger,
            IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _uaiRangoConfig = GetConfUaiRango(config);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Exemplo de método que obtém informações de um estabelecimento.
        /// </summary>
        public async Task<EstabelecimentoConfigDto> ObterEstabelecimentoAsync(string token, int idEstabelecimento)
        {
            try
            {
                string endpoint = $"{_uaiRangoConfig.base_url}/auth/info/{idEstabelecimento}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                // Se não for sucesso, gera exceção
                response.EnsureSuccessStatusCode();

                string conteudo = await response.Content.ReadAsStringAsync();
                var estabelecimento = JsonConvert.DeserializeObject<EstabelecimentoConfigDto>(conteudo);
                return estabelecimento;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro ao fazer requisição HTTP: {Message}", ex.Message);
                throw; // relança a exceção ou retorne null
            }
            catch (JsonException ex)
            {
                _logger.LogError("Erro ao desserializar JSON: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Exemplo de método que obtém os prazos possíveis.
        /// </summary>
        public async Task<List<PrazoDto>> ObterPrazosAsync(string token)
        {
            try
            {
                string endpoint = $"{_uaiRangoConfig.base_url}/auth/info/prazos";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                string conteudo = await response.Content.ReadAsStringAsync();
                var prazos = JsonConvert.DeserializeObject<List<PrazoDto>>(conteudo);
                return prazos;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro ao fazer requisição HTTP (prazos): {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Erro ao desserializar JSON (prazos): {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado (prazos): {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Exemplo de método que faz o PUT /auth/info/{id_estabelecimento}/{campo}/{valor}.
        /// Retorna true se sucesso.
        /// </summary>
        public async Task<bool> AtualizarEstabelecimentoAsync(string token, int idEstabelecimento, string campo, string valor)
        {
            try
            {
                string endpoint = $"{_uaiRangoConfig.base_url}/auth/info/{idEstabelecimento}/{campo}/{valor}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                string conteudo = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Nesse caso a API geralmente retorna { "success": false, "message": "..." }
                    var errorDto = JsonConvert.DeserializeObject<IConfigPutResponseDto>(conteudo);
                    string msg = errorDto?.Message ?? "Erro indefinido ao atualizar estabelecimento";
                    _logger.LogWarning("PUT falhou para {endpoint}: {msg}", endpoint, msg);
                    return false;
                }

                var result = JsonConvert.DeserializeObject<IConfigPutResponseDto>(conteudo);
                return result?.Success ?? false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro HTTP ao atualizar estabelecimento: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Erro JSON ao atualizar estabelecimento: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado ao atualizar estabelecimento: {Message}", ex.Message);
                throw;
            }
        }
    }
}
