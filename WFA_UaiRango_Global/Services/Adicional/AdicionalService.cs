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

namespace WFA_UaiRango_Global.Services.Adicional
{
    public class AdicionalService : IAdicionalService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdicionalService> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private SConfUaiRango configuracao;

        public AdicionalService(
            HttpClient httpClient,
            ILogger<AdicionalService> logger,
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
        /// GET /auth/cardapio/{id_estabelecimento}/adicionais
        /// </summary>
        public async Task<List<CategoriaAdicionalDto>?> ListarAdicionaisPorEstabelecimentoAsync(string token, int idEstabelecimento)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionais";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET ListarAdicionaisPorEstabelecimento falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // Se vier "null", significa não encontrou nada
                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                var lista = JsonConvert.DeserializeObject<List<CategoriaAdicionalDto>>(respBody);
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar adicionais por estabelecimento (idEstab={0})", idEstabelecimento);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionais/{id_categoria}
        /// </summary>
        public async Task<CategoriaAdicionalDto?> ListarAdicionaisPorCategoriaAsync(string token, int idEstabelecimento, int idCategoria)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionais/{idCategoria}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET ListarAdicionaisPorCategoria falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                var cat = JsonConvert.DeserializeObject<CategoriaAdicionalDto>(respBody);
                return cat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar adicionais por categoria (idEstab={0}, idCat={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicional/{id_tipo}
        /// </summary>
        public async Task<TipoAdicionalDto?> ObterAdicionalPorTipoAsync(string token, int idEstabelecimento, int idTipo)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicional/{idTipo}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET ObterAdicionalPorTipo falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                var tipo = JsonConvert.DeserializeObject<TipoAdicionalDto>(respBody);
                return tipo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter adicional por tipo (idEstab={0}, idTipo={1})", idEstabelecimento, idTipo);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/novo
        /// </summary>
        public async Task<int?> CriarAdicionalAsync(string token, int idEstabelecimento, int idCategoria, AdicionalNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicional/{idCategoria}/novo";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST CriarAdicional falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // Esperado algo como { "success": true, "id_tipo": 56373, "message": "Adicional cadastrado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseAdicionalDto>(respBody);
                if (result?.Success == true && result.IdTipo.HasValue)
                {
                    return result.IdTipo.Value;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar adicional (Estab={0}, Cat={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/alterar/{id_tipo}
        /// </summary>
        public async Task<bool> AlterarAdicionalAsync(string token, int idEstabelecimento, int idCategoria, int idTipo, AdicionalAlterarDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicional/{idCategoria}/alterar/{idTipo}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarAdicional falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Em caso de sucesso: { "success": true, "message": "Adicional alterado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseAdicionalDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar adicional (Estab={0}, Cat={1}, Tipo={2})", idEstabelecimento, idCategoria, idTipo);
                throw;
            }
        }

        /// <summary>
        /// DELETE /auth/cardapio/{id_estabelecimento}/adicional/{id_categoria}/remover/{id_tipo}
        /// </summary>
        public async Task<bool> RemoverAdicionalAsync(string token, int idEstabelecimento, int idCategoria, int idTipo)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicional/{idCategoria}/remover/{idTipo}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("DELETE RemoverAdicional falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Em caso de sucesso: { "success": true, "message": "Adicional removido com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseAdicionalDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover adicional (Estab={0}, Cat={1}, Tipo={2})", idEstabelecimento, idCategoria, idTipo);
                throw;
            }
        }
    }
}
