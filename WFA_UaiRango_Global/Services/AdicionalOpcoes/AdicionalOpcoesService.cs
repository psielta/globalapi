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

namespace WFA_UaiRango_Global.Services.AdicionalOpcoes
{
    public class AdicionalOpcoesService : IAdicionalOpcoesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdicionalOpcoesService> _logger;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private SConfUaiRango configuracao;

        public AdicionalOpcoesService(
            HttpClient httpClient,
            ILogger<AdicionalOpcoesService> logger,
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
        /// GET /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}
        /// </summary>
        public async Task<List<AdicionalOpcaoDto>?> ListarOpcoesAdicionalAsync(string token, int idEstabelecimento, int idTipo)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionalOpcoes/{idTipo}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET ListarOpcoesAdicional falhou ({Status}). Body: {Body}",
                        response.StatusCode, respBody);
                    return null;
                }

                // Se vier "[]" => lista vazia
                // Se vier "null" => também pode retornar null no deserial
                if (string.IsNullOrWhiteSpace(respBody) || respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                var lista = JsonConvert.DeserializeObject<List<AdicionalOpcaoDto>>(respBody);
                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar opções de adicional (idEstabelecimento={0}, idTipo={1})",
                    idEstabelecimento, idTipo);
                throw;
            }
        }

        /// <summary>
        /// GET /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/opcao/{id_adicional}
        /// </summary>
        public async Task<AdicionalOpcaoDto?> ObterOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionalOpcoes/{idTipo}/opcao/{idAdicional}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GET ObterOpcaoAdicional falhou ({Status}). Body: {Body}",
                        response.StatusCode, respBody);
                    return null;
                }

                if (respBody.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;

                var opcao = JsonConvert.DeserializeObject<AdicionalOpcaoDto>(respBody);
                return opcao;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao obter opcao do adicional (idEstabelecimento={0}, idTipo={1}, idAdicional={2})",
                    idEstabelecimento, idTipo, idAdicional);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/novo
        /// </summary>
        public async Task<int?> CriarOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, OpcaoAdicionalNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionalOpcoes/{idTipo}/novo";
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
                    _logger.LogWarning("POST CriarOpcaoAdicional falhou ({Status}). Body: {Body}",
                        response.StatusCode, respBody);
                    return null;
                }

                // Exemplo de sucesso:
                // { "success": true, "id_adicional": 436789, "message": "Opcao de adicional cadastrado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseOpcaoAdicionalDto>(respBody);
                if (result?.Success == true && result.IdAdicional.HasValue)
                {
                    return result.IdAdicional.Value;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar opção de adicional (Estab={0}, Tipo={1})",
                    idEstabelecimento, idTipo);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/alterar/{id_adicional}
        /// </summary>
        public async Task<bool> AlterarOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional, OpcaoAdicionalNovoDto dados)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionalOpcoes/{idTipo}/alterar/{idAdicional}";
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
                    _logger.LogWarning("PUT AlterarOpcaoAdicional falhou ({Status}). Body: {Body}",
                        response.StatusCode, respBody);
                    return false;
                }

                // Exemplo de sucesso:
                // { "success": true, "message": "Opcao do adicional alterado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseOpcaoAdicionalDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao alterar opcao do adicional (Estab={0}, Tipo={1}, Adicional={2})",
                    idEstabelecimento, idTipo, idAdicional);
                throw;
            }
        }

        /// <summary>
        /// PUT /auth/cardapio/{id_estabelecimento}/adicionalOpcoes/{id_tipo}/opcao/{id_adicional}/status/{status}
        /// </summary>
        public async Task<bool> AlterarStatusOpcaoAdicionalAsync(string token, int idEstabelecimento, int idTipo, int idAdicional, int status)
        {
            string endpoint = $"{_baseUrl}/auth/cardapio/{idEstabelecimento}/adicionalOpcoes/{idTipo}/opcao/{idAdicional}/status/{status}";
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
                var response = await _httpClient.SendAsync(request);
                var respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PUT AlterarStatusOpcaoAdicional falhou ({Status}). Body: {Body}",
                        response.StatusCode, respBody);
                    return false;
                }

                // Exemplo de sucesso:
                // { "success": true, "message": "Opcao do adicional Habilitado com sucesso!" }
                var result = JsonConvert.DeserializeObject<BaseResponseOpcaoAdicionalDto>(respBody);
                return result?.Success ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao alterar status da opcao do adicional (Estab={0}, Tipo={1}, Adicional={2}, status={3})",
                    idEstabelecimento, idTipo, idAdicional, status);
                throw;
            }
        }
    }
}
