using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Item;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;

namespace service.indumepi.Application.Service.ItemRequest
{
    public class ItemService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ItemService> _logger;

        public ItemService(HttpClient httpClient, ILogger<ItemService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Item>> ListarProdutosAsync(int pagina = 1)
        {
            var url = "https://app.omie.com.br/api/v1/geral/produtos/";

            var data = new
            {
                call = "ListarProdutos",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina = 1,
                        registros_por_pagina = 50,
                        apenas_importado_api = "N",
                        filtrar_apenas_omiepdv = "N"
                    }
                }
            };

            var jsonPayload = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            });

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resposta recebida com sucesso da API Omie.");

                    var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                    List<Item> produtos = new List<Item>();



                    foreach (var produto in responseData.produto_servico_cadastro)
                    {
                        produtos.Add(new Item
                        {
                            Codigo = produto.codigo,
                            CodigoProduto = Convert.ToInt64(produto.codigo_produto),
                            Descricao = produto.descricao,
                            ValorUnitario = (decimal)produto.valor_unitario,
                            CodigoFamilia = Convert.ToInt64(produto.codigo_familia)
                        });

                    }

                    return produtos;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Detalhes: {errorResponse}");
                    return new List<Item>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro na requisição HTTP: {ex.Message}");
                return new List<Item>();
            }
        }
    }
}