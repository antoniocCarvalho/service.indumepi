using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace service.indumepi.Application.Service.EnviaPed
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(HttpClient httpClient, ILogger<PedidoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task EnviarEtapaAsync(int numeroPedido, string etapa)
        {
            var url = "https://app.omie.com.br/api/v1/produtos/pedido/";

            var data = new
            {
                call = "TrocarEtapaPedido",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        numero_pedido = numeroPedido,
                        codigo_pedido_integracao = "",
                        etapa
                    }
                }
            };

            var jsonPayload = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Informações enviadas com sucesso.");
                }
                else
                {
                    _logger.LogError($"Erro ao enviar informações. Código de status: {response.StatusCode}");
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Resposta: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao conectar com a API: {ex.Message}");
            }
        }
    }
}
