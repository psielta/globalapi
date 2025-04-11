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
using GlobalErpData.Uairango.Dto;
using static GlobalLib.Files.ConfiguracoesUaiRango;

namespace WFA_UaiRango_Global.Services.Culinaria
{
    public class CulinariaService : ICulinariaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CulinariaService> _logger;
        private readonly IConfiguration _config;
        private SConfUaiRango configuracao;

        public CulinariaService(HttpClient httpClient, ILogger<CulinariaService> logger, 
            IConfiguration config)
        {
            _config = config;
            configuracao = GetConfUaiRango(config);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;
        }

        public async Task<List<CulinariaDto>> ObterCulinariasAsync(string token)
        {
            try
            {
                var Endpoint = $"{configuracao.base_url}/auth/culinarias";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.GetAsync(Endpoint);
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