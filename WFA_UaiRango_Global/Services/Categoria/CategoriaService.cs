using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static GlobalLib.Files.ConfiguracoesUaiRango;
using Newtonsoft.Json;
using GlobalLib.Files;

namespace WFA_UaiRango_Global.Services.Categoria
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriaService> _logger;
        private readonly IConfiguration _config;
        private SConfUaiRango configuracao;

        // Aqui assumimos que exista alguma seção no appsettings.json com "UaiRango:BaseUrl"
        // para recuperar a URL base. Ajuste conforme sua necessidade.
        private readonly string _baseUrl;

        public CategoriaService(
            HttpClient httpClient,
            ILogger<CategoriaService> logger,
            IConfiguration config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            configuracao = GetConfUaiRango(config);
            _baseUrl = configuracao.base_url;
            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                throw new InvalidOperationException("URL base do UaiRango não foi configurada em UaiRango:BaseUrl.");
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/categoria/{id_categoria}
        /// </summary>
        public async Task<CategoriaDto?> ObterCategoriaAsync(string token, int idEstabelecimento, int idCategoria)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/categoria/{idCategoria}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);

                // Se for 200 mas o body é "null", a API retorna "null" literal
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET ObterCategoria falhou: ({Status}) {Body}", response.StatusCode, errorBody);
                    // Você decide se lança exceção ou retorna null
                    return null;
                }

                string conteudo = await response.Content.ReadAsStringAsync();
                if (conteudo.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    return null; // A API retornou "null" => não encontrado
                }

                var categoria = JsonConvert.DeserializeObject<CategoriaDto>(conteudo);
                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria (IdEstab={idEstabelecimento}, IdCat={idCategoria})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/categorias
        /// </summary>
        public async Task<List<CategoriaDto>?> ObterCategoriasAsync(string token, int idEstabelecimento)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/categorias";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET ObterCategorias falhou: ({Status}) {Body}", response.StatusCode, errorBody);
                    return null;
                }

                string conteudo = await response.Content.ReadAsStringAsync();
                var categorias = JsonConvert.DeserializeObject<List<CategoriaDto>>(conteudo);
                return categorias;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lista de categorias (IdEstab={idEstabelecimento})", idEstabelecimento);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/categoria/{id_categoria}/alterar
        /// </summary>
        public async Task<bool> AlterarCategoriaAsync(string token, int idEstabelecimento, int idCategoria, CategoriaAlterarDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/categoria/{idCategoria}/alterar";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serializa o body em JSON
                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarCategoria falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    // A API normalmente devolve algo como { "success": false, "message": "..." }
                    // Você pode desserializar e tratar a mensagem se quiser:
                    var erro = JsonConvert.DeserializeObject<BaseResponseDto>(respBody);
                    return false;
                }

                var resultado = JsonConvert.DeserializeObject<BaseResponseDto>(respBody);
                return resultado?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar categoria (IdEstab={idEstabelecimento}, IdCat={idCategoria})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/categoria/{id_categoria}/status/{status}
        /// status = 1 (habilita) ou 0 (desabilita)
        /// </summary>
        public async Task<bool> AlterarStatusCategoriaAsync(string token, int idEstabelecimento, int idCategoria, int status)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/categoria/{idCategoria}/status/{status}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarStatusCategoria falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                var resultado = JsonConvert.DeserializeObject<BaseResponseDto>(respBody);
                return resultado?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status da categoria (IdEstab={idEstabelecimento}, IdCat={idCategoria}, status={status})",
                                 idEstabelecimento, idCategoria, status);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/categoria/novo
        /// Retorna o id_categoria criado, ou null se falhar.
        /// </summary>
        public async Task<int?> CriarCategoriaAsync(string token, int idEstabelecimento, CategoriaNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/categoria/novo";
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
                    _logger.LogWarning("POST CriarCategoria falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // A API de sucesso retorna algo como:
                // { "success": true, "id_categoria": 90447, "message": "Categoria adicionada com sucesso!" }
                var resultado = JsonConvert.DeserializeObject<BaseResponseDto>(respBody);
                if (resultado?.Success == true && resultado.IdCategoria.HasValue)
                {
                    return resultado.IdCategoria.Value;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar categoria (IdEstab={idEstabelecimento})", idEstabelecimento);
                throw;
            }
        }
    }
}
