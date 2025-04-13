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

namespace WFA_UaiRango_Global.Services.CategoriaOpcao
{
    public class CategoriaOpcaoService : ICategoriaOpcaoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriaOpcaoService> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private SConfUaiRango configuracao;

        public CategoriaOpcaoService(
            HttpClient httpClient,
            ILogger<CategoriaOpcaoService> logger,
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
        /// GET /auth/cardapio/{id_estabelecimento}/opcoes/{id_categoria}
        /// </summary>
        public async Task<List<CategoriaOpcaoDto>?> ObterOpcoesDaCategoriaAsync(string token, int idEstabelecimento, int idCategoria)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/opcoes/{idCategoria}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET Opcoes falhou: ({Status}) {Body}", response.StatusCode, errorBody);
                    // pode retornar null ou lançar exceção, a critério
                    return null;
                }

                // Se for 200 mas vier "[]", a desserialização resultará em lista vazia
                // Se vier erro, a gente já trata acima
                string respBody = await response.Content.ReadAsStringAsync();
                var lista = JsonConvert.DeserializeObject<List<CategoriaOpcaoDto>>(respBody);
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter opcoes de categoria (IdEstab={0}, IdCategoria={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/opcao/{id_opcao}
        /// </summary>
        public async Task<CategoriaOpcaoDto?> ObterOpcaoAsync(string token, int idEstabelecimento, int idOpcao)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/opcao/{idOpcao}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    string errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("GET Opcao falhou: ({Status}) {Body}", response.StatusCode, errorBody);
                    return null;
                }

                string respBody = await response.Content.ReadAsStringAsync();
                // A API pode retornar "null" literal, então checamos isso:
                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    return null; // Opção não encontrada
                }

                var opcao = JsonConvert.DeserializeObject<CategoriaOpcaoDto>(respBody);
                return opcao;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter opcao (IdEstab={0}, IdOpcao={1})", idEstabelecimento, idOpcao);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/opcao/{id_categoria}/novo
        /// Retorna o id_opcao criado, ou null se falhar
        /// </summary>
        public async Task<int?> CriarOpcaoAsync(string token, int idEstabelecimento, int idCategoria, CategoriaOpcaoNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/opcao/{idCategoria}/novo";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Body em JSON
                string bodyJson = JsonConvert.SerializeObject(dados);
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = new StringContent(bodyJson, System.Text.Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST CriarOpcao falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return null;
                }

                // Sucesso: { "success": true, "id_opcao": 185940, "message": "Opção cadastrada com sucesso!" }
                var resultado = JsonConvert.DeserializeObject<BaseResponseCategoriaOpcaoDto>(respBody);
                if (resultado?.Success == true && resultado.IdOpcao.HasValue)
                    return resultado.IdOpcao.Value;

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar opcao (IdEstab={0}, IdCategoria={1})", idEstabelecimento, idCategoria);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/opcao/{id_categoria}/alterar/{id_opcao}
        /// </summary>
        public async Task<bool> AlterarOpcaoAsync(
            string token,
            int idEstabelecimento,
            int idCategoria,
            int idOpcao,
            CategoriaOpcaoAlterarDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/opcao/{idCategoria}/alterar/{idOpcao}";
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
                    _logger.LogWarning("PUT AlterarOpcao falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Em caso de sucesso: { "success": true, "message": "Opção alterada com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseCategoriaOpcaoDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar opcao (IdEstab={0}, IdCat={1}, IdOpcao={2})", idEstabelecimento, idCategoria, idOpcao);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/opcao/{id_opcao}/status/{status}
        /// status = 0 ou 1
        /// </summary>
        public async Task<bool> AlterarStatusOpcaoAsync(string token, int idEstabelecimento, int idOpcao, int status)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/opcao/{idOpcao}/status/{status}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var response = await _httpClient.SendAsync(request);

                string respBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarStatusOpcao falhou ({Status}). Body: {Body}", response.StatusCode, respBody);
                    return false;
                }

                // Ex. Sucesso: { "success": true, "message": "Opção Desabilitada com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseCategoriaOpcaoDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status da opcao (IdEstab={0}, IdOpcao={1}, Status={2})", idEstabelecimento, idOpcao, status);
                throw;
            }
        }
    }
}
