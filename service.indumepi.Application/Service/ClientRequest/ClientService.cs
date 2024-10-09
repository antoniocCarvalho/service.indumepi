using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Domain.Aggregates.Client;
using service.indumepi.Domain.Aggregates.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Application.Service.ClientRequest
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ItemService> _logger;

        public ClientService(HttpClient httpClient, ILogger<ItemService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Client>> ListarClientesAsync(int pagina = 1)
        {
            var url = "https://app.omie.com.br/api/v1/geral/clientes/";

            var data = new
            {
                call = "ListarClientesResumido",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina = 1,
                        registros_por_pagina = 100,
                        apenas_importado_api = "N",
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
                    List<Client> clientes = new List<Client>();



                    foreach (var cliente in responseData.clientes_cadastro_resumido)
                    {
                        clientes.Add(new Client
                        {
                            CnpjCpf = cliente.cnpj_cpf,
                            CodigoCliente = Convert.ToInt64(cliente.codigo_cliente),
                            NomeFantasia = cliente.nome_fantasia,
                            RazaoSocial = cliente.razao_social,
                        });

                    }

                    return clientes;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Detalhes: {errorResponse}");
                    return new List<Client>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro na requisição HTTP: {ex.Message}");
                return new List<Client>();
            }



        }
        }
    }
