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
using static GlobalLib.Files.ConfiguracoesUaiRango;
using Newtonsoft.Json;

namespace WFA_UaiRango_Global.Services.FormasPagamento
{
    public class FormasPagamentoService : IFormasPagamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FormasPagamentoService> _logger;
        private readonly IConfiguration _config;
        private readonly SConfUaiRango _configuracao;

        public FormasPagamentoService(
            HttpClient httpClient,
            ILogger<FormasPagamentoService> logger,
            IConfiguration config
        )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            // Obtém a configuração específica, como base_url, token etc.
            _configuracao = GetConfUaiRango(_config);
        }

        /// <summary>
        /// GET /auth/formas_pagamento/{id_estabelecimento}?tipo_entrega=D
        /// </summary>
        public async Task<List<FormaPagamentoDto>> ObterFormasPagamentoAsync(int idEstabelecimento, string tipoEntrega, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(tipoEntrega))
                    throw new ArgumentException("tipo_entrega é obrigatório e precisa ser 'D' ou 'R'.");

                var endpoint = $"{_configuracao.base_url}/auth/formas_pagamento/{idEstabelecimento}?tipo_entrega={tipoEntrega}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var conteudo = await response.Content.ReadAsStringAsync();

                // Desserializa a resposta em uma lista de DTOs
                var formasPagamento = JsonConvert.DeserializeObject<List<FormaPagamentoDto>>(conteudo);

                return formasPagamento;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro ao fazer requisição HTTP (GET formas_pagamento): {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Erro ao desserializar JSON (GET formas_pagamento): {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado (GET formas_pagamento): {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// POST /auth/formas_pagamento/{id_estabelecimento}?tipo_entrega=D
        /// </summary>
        public async Task<bool> AtualizarFormasPagamentoAsync(int idEstabelecimento, string tipoEntrega, List<int> formas, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(tipoEntrega))
                    throw new ArgumentException("tipo_entrega é obrigatório e precisa ser 'D' ou 'R'.");

                var endpoint = $"{_configuracao.base_url}/auth/formas_pagamento/{idEstabelecimento}?tipo_entrega={tipoEntrega}";

                var bodyRequest = new AtualizarFormasPagamentoRequestDto
                {
                    Formas = formas
                };

                var jsonBody = JsonConvert.SerializeObject(bodyRequest);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var conteudo = await response.Content.ReadAsStringAsync();

                dynamic resultado = JsonConvert.DeserializeObject(conteudo);
                bool success = resultado.success;

                return success;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro ao fazer requisição HTTP (POST formas_pagamento): {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Erro ao desserializar JSON (POST formas_pagamento): {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado (POST formas_pagamento): {Message}", ex.Message);
                throw;
            }
        }
    }
}
