using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WFA_UaiRango_Global.Dto;
using static GlobalLib.Files.ConfiguracoesUaiRango;

namespace WFA_UaiRango_Global.Services.Culinaria
{
    public class CulinariaService : ICulinariaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CulinariaService> _logger;
        private readonly GlobalErpFiscalBaseContext _db;
        private readonly IConfiguration _config;
        private SConfUaiRango configuracao;

        public CulinariaService(HttpClient httpClient, ILogger<CulinariaService> logger, GlobalErpFiscalBaseContext db,
            IConfiguration config)
        {
            _config = config;
            configuracao = GetConfUaiRango(config);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri(configuracao.base_url);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuracao.token_de_acesso);

            _logger = logger;
            _db = db;
        }

        public async Task<List<CulinariaDto>> ObterCulinariasAsync()
        {
            try
            {
                // Verificando se o token está configurado
                if (_httpClient.DefaultRequestHeaders.Authorization == null)
                {
                    _logger.LogWarning("Token de autorização não configurado");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuracao.token_de_acesso);
                }

                HttpResponseMessage response = await _httpClient.GetAsync("/auth/culinarias");
                response.EnsureSuccessStatusCode();
                string conteudo = await response.Content.ReadAsStringAsync();
                List<CulinariaDto> culinarias = JsonConvert.DeserializeObject<List<CulinariaDto>>(conteudo);
                return culinarias;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Erro ao fazer requisição HTTP: {Message}", ex.Message);
                throw;
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
    }
}