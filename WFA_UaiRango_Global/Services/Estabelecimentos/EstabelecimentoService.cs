using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GlobalLib.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WFA_UaiRango_Global.Dto;

namespace WFA_UaiRango_Global.Services.Estabelecimentos
{
    public class EstabelecimentoService : IEstabelecimentoService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EstabelecimentoService> _logger;
        private readonly HttpClient _httpClient;

        public EstabelecimentoService(IConfiguration config, ILogger<EstabelecimentoService> logger, HttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<VincularEstabelecimentoResponse> VincularEstabelecimentoAsync(string bearerToken, string tokenEstabelecimento)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de vinculação de estabelecimento");

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var endpoint = $"{uaiRangoConfig.base_url}/auth/estabelecimento/vincular/{tokenEstabelecimento}";

                // Configura o token de autenticação no cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                _logger.LogDebug("Enviando requisição para: {Endpoint}", endpoint);
                var response = await _httpClient.PostAsync(endpoint, null); // null porque não tem corpo na requisição

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao vincular estabelecimento: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var vincularResponse = JsonConvert.DeserializeObject<VincularEstabelecimentoResponse>(responseContent);

                _logger.LogInformation("Estabelecimento vinculado com sucesso. ID: {IdEstabelecimento}", vincularResponse.IdEstabelecimento);
                return vincularResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar vincular estabelecimento");
                throw;
            }
            finally
            {
                // Limpa o cabeçalho de autorização após a requisição
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<RemoverEstabelecimentoResponse> RemoverEstabelecimentoAsync(string bearerToken, int idEstabelecimento)
        {
            try
            {
                _logger.LogInformation("Iniciando processo de remoção de estabelecimento. ID: {IdEstabelecimento}", idEstabelecimento);

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var endpoint = $"{uaiRangoConfig.base_url}/auth/estabelecimento/remover/{idEstabelecimento}";

                // Configura o token de autenticação no cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                // Cria uma requisição DELETE
                var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);

                _logger.LogDebug("Enviando requisição para: {Endpoint}", endpoint);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao remover estabelecimento: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var removerResponse = JsonConvert.DeserializeObject<RemoverEstabelecimentoResponse>(responseContent);

                _logger.LogInformation("Estabelecimento removido com sucesso");
                return removerResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar remover estabelecimento. ID: {IdEstabelecimento}", idEstabelecimento);
                throw;
            }
            finally
            {
                // Limpa o cabeçalho de autorização após a requisição
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<List<Estabelecimento>> ListarEstabelecimentosAsync(string bearerToken)
        {
            try
            {
                _logger.LogInformation("Iniciando listagem de estabelecimentos");

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var endpoint = $"{uaiRangoConfig.base_url}/auth/estabelecimentos";

                // Configura o token de autenticação no cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                _logger.LogDebug("Enviando requisição para: {Endpoint}", endpoint);
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao listar estabelecimentos: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var estabelecimentos = JsonConvert.DeserializeObject<List<Estabelecimento>>(responseContent);

                _logger.LogInformation("Listagem de estabelecimentos concluída. Total: {Total}", estabelecimentos.Count);
                return estabelecimentos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar listar estabelecimentos");
                throw;
            }
            finally
            {
                // Limpa o cabeçalho de autorização após a requisição
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<CheckVinculoResponse> ChecarVinculoPorTokenAsync(string bearerToken, string tokenEstabelecimento)
        {
            try
            {
                _logger.LogInformation("Iniciando verificação de vínculo por token");

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var endpoint = $"{uaiRangoConfig.base_url}/auth/estabelecimento/checarVinculoToken/{tokenEstabelecimento}";

                // Configura o token de autenticação no cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                _logger.LogDebug("Enviando requisição para: {Endpoint}", endpoint);
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao verificar vínculo por token: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var checkResponse = JsonConvert.DeserializeObject<CheckVinculoResponse>(responseContent);

                _logger.LogInformation("Verificação de vínculo por token concluída. Vinculado: {Vinculado}", checkResponse.Vinculado);
                return checkResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar vínculo por token");
                throw;
            }
            finally
            {
                // Limpa o cabeçalho de autorização após a requisição
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<CheckVinculoResponse> ChecarVinculoPorIdAsync(string bearerToken, int idEstabelecimento)
        {
            try
            {
                _logger.LogInformation("Iniciando verificação de vínculo por ID: {IdEstabelecimento}", idEstabelecimento);

                var uaiRangoConfig = ConfiguracoesUaiRango.GetConfUaiRango(_config);
                var endpoint = $"{uaiRangoConfig.base_url}/auth/estabelecimento/checarVinculoID/{idEstabelecimento}";

                // Configura o token de autenticação no cabeçalho
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                _logger.LogDebug("Enviando requisição para: {Endpoint}", endpoint);
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Falha ao verificar vínculo por ID: {StatusCode}, Resposta: {Response}",
                        response.StatusCode, errorContent);
                    throw new HttpRequestException($"Falha na requisição: {response.StatusCode}. Detalhes: {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var checkResponse = JsonConvert.DeserializeObject<CheckVinculoResponse>(responseContent);

                _logger.LogInformation("Verificação de vínculo por ID concluída. Vinculado: {Vinculado}", checkResponse.Vinculado);
                return checkResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar vínculo por ID: {IdEstabelecimento}", idEstabelecimento);
                throw;
            }
            finally
            {
                // Limpa o cabeçalho de autorização após a requisição
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

    }
}
