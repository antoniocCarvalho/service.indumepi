using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Item;

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

        public async Task<List<Item>> ListarTodosOsProdutosAsync()
        {
            var url = "https://app.omie.com.br/api/v1/geral/produtos/";
            var todosOsProdutos = new List<Item>();
            int registrosPorPagina = 200; 
            int paginasPorBatch = 5; 
            int pagina = 1;
            bool continuarBuscando = true;

            while (continuarBuscando)
            {
                var tasks = new List<Task<List<Item>>>();

                for (int i = 0; i < paginasPorBatch; i++)
                {
                    tasks.Add(ObterProdutosPorPaginaAsync(url, pagina + i, registrosPorPagina));
                }

                var resultados = await Task.WhenAll(tasks);

                foreach (var produtosPagina in resultados)
                {
                    if (produtosPagina.Count > 0)
                    {
                        todosOsProdutos.AddRange(produtosPagina);
                    }
                    else
                    {
                        continuarBuscando = false;
                        break;
                    }
                }

                pagina += paginasPorBatch;

            }

            return todosOsProdutos;
        }

        private async Task<List<Item>> ObterProdutosPorPaginaAsync(string url, int pagina, int registrosPorPagina)
        {
            var data = new
            {
                call = "ListarProdutos",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina,
                        registros_por_pagina = registrosPorPagina,
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

            _httpClient.DefaultRequestHeaders.Clear();
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                List<Item> produtosPagina = new List<Item>();

                foreach (var produto in responseData.produto_servico_cadastro)
                {
                    produtosPagina.Add(new Item
                    {
                        Codigo = produto.codigo,
                        CodigoProduto = Convert.ToInt64(produto.codigo_produto),
                        Descricao = produto.descricao,
                        ValorUnitario = (decimal)produto.valor_unitario,
                        CodigoFamilia = Convert.ToInt64(produto.codigo_familia)
                    });
                }

                return produtosPagina;
            }
            else
            {
                _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}");
                return new List<Item>();
            }
        }
    }
}

