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

namespace WFA_UaiRango_Global.Services.Preco
{
    public class PrecoService : IPrecoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PrecoService> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private SConfUaiRango configuracao;

        public PrecoService(
            HttpClient httpClient,
            ILogger<PrecoService> logger,
            IConfiguration config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            configuracao = GetConfUaiRango(config);
            _baseUrl = configuracao.base_url;
            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                throw new InvalidOperationException("URL base do UaiRango não foi configurada (UaiRango:BaseUrl).");
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/preco/{id_preco}
        /// </summary>
        public async Task<PrecoDto?> ObterPrecoAsync(string token, int idEstabelecimento, int idPreco)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/preco/{idPreco}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET ObterPreco falhou ({Status}). Body: {Body}", response.StatusCode, errorBody);
                    return null;
                }

                string respBody = await response.Content.ReadAsStringAsync();
                // Se vier "null", é que não encontrou nada
                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                var preco = JsonConvert.DeserializeObject<PrecoDto>(respBody);
                return preco;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter preco (Estab={0}, Preco={1})", idEstabelecimento, idPreco);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/preco/{id_produto}/novo
        /// </summary>
        public async Task<int?> CriarPrecoAsync(string token, int idEstabelecimento, int idProduto, PrecoNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/preco/{idProduto}/novo";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST CriarPreco falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // Sucesso: { "success": true, "id_preco": 1468277, "message": "Preço adicionado com sucesso!" }
                var result = JsonConvert.DeserializeObject<PrecoBaseResponseDto>(respBody);
                if (result?.Success == true && result.IdPreco.HasValue)
                {
                    return result.IdPreco.Value;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar preco (Estab={0}, Prod={1})", idEstabelecimento, idProduto);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/preco/{id_preco}/alterar
        /// </summary>
        public async Task<bool> AlterarPrecoAsync(string token, int idEstabelecimento, int idPreco, PrecoAlterarDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/preco/{idPreco}/alterar";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarPreco falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Em caso de sucesso: { "success": true, "message": "Preço alterado com sucesso!" }
                var result = JsonConvert.DeserializeObject<PrecoBaseResponseDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar preco (Estab={0}, Preco={1})", idEstabelecimento, idPreco);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/preco/{id_preco}/status/{status}
        /// </summary>
        public async Task<bool> AlterarStatusPrecoAsync(string token, int idEstabelecimento, int idPreco, int status)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/preco/{idPreco}/status/{status}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarStatusPreco falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Ex: { "success": true, "message": "Preço Desabilitado com sucesso!" }
                var result = JsonConvert.DeserializeObject<PrecoBaseResponseDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do preco (Estab={0}, Preco={1}, Status={2})",
                                 idEstabelecimento, idPreco, status);
                throw;
            }
        }
    }
}
