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

namespace WFA_UaiRango_Global.Services.Produto
{
    public class ProdutoService : IProdutoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProdutoService> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private SConfUaiRango configuracao;

        public ProdutoService(
            HttpClient httpClient,
            ILogger<ProdutoService> logger,
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
        /// GET /auth/cardapio/{id_estabelecimento}/produtos/{id_categoria}
        /// </summary>
        public async Task<List<ProdutoDto>?> ObterProdutosDaCategoriaAsync(string token, int idEstabelecimento, int idCategoria)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/produtos/{idCategoria}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET ObterProdutosDaCategoria falhou ({Status}). Body: {Body}", response.StatusCode, errorBody);
                    return null; // ou lançar exceção, a seu critério
                }

                string respBody = await response.Content.ReadAsStringAsync();
                // Se vier "[]" => lista vazia
                var produtos = JsonConvert.DeserializeObject<List<ProdutoDto>>(respBody);
                return produtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produtos da categoria (Estab={0}, Categoria={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/produto/{id_produto}
        /// </summary>
        public async Task<ProdutoDto?> ObterProdutoAsync(string token, int idEstabelecimento, int idProduto)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/produto/{idProduto}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET ObterProduto falhou ({Status}). Body: {Body}", response.StatusCode, errorBody);
                    return null;
                }

                string respBody = await response.Content.ReadAsStringAsync();
                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    // A API retorna "null" literal se não encontrou nada
                    return null;
                }

                var produto = JsonConvert.DeserializeObject<ProdutoDto>(respBody);
                return produto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produto (Estab={0}, Produto={1})", idEstabelecimento, idProduto);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/produto/{id_categoria}/novo
        /// </summary>
        public async Task<int?> CriarProdutoAsync(string token, int idEstabelecimento, int idCategoria, ProdutoNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/produto/{idCategoria}/novo";
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
                    _logger.LogWarning("POST CriarProduto falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // Exemplo de sucesso: { "success": true, "id_produto": 673597, "message": "Produto adicionado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseProdutoDto>(respBody);
                if (result?.Success == true && result.IdProduto.HasValue)
                    return result.IdProduto.Value;

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto (Estab={0}, Categoria={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/produto/{id_categoria}/alterar/{id_produto}
        /// </summary>
        public async Task<bool> AlterarProdutoAsync(
            string token,
            int idEstabelecimento,
            int idCategoria,
            int idProduto,
            ProdutoAlterarDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/produto/{idCategoria}/alterar/{idProduto}";
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
                    _logger.LogWarning("PUT AlterarProduto falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Exemplo de sucesso: { "success": true, "message": "Produto alterado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseProdutoDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar produto (Estab={0}, Cat={1}, Prod={2})", idEstabelecimento, idCategoria, idProduto);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/produto/{id_produto}/status/{status}
        /// status = 0 ou 1
        /// </summary>
        public async Task<bool> AlterarStatusProdutoAsync(string token, int idEstabelecimento, int idProduto, int status)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/produto/{idProduto}/status/{status}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarStatusProduto falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Exemplo de sucesso: { "success": true, "message": "Produto Habilitado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseProdutoDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do produto (Estab={0}, Prod={1}, Status={2})", idEstabelecimento, idProduto, status);
                throw;
            }
        }
    }
}
