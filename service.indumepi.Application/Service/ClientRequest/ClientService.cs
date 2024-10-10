﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Application.Service.ClientRequest
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClientService> _logger;

        public ClientService(HttpClient httpClient, ILogger<ClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Configurar HttpClient para aceitar respostas compactadas
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        public async Task<List<Client>> ListarTodosOsClientesAsync()
        {
            var url = "https://app.omie.com.br/api/v1/geral/clientes/";
            var todosOsClientes = new List<Client>();
            int registrosPorPagina = 200;  
            int paginasPorBatch = 5; 
            int pagina = 1;
            bool continuarBuscando = true;

            while (continuarBuscando)
            {
                var tasks = new List<Task<List<Client>>>();

                for (int i = 0; i < paginasPorBatch; i++)
                {
                    tasks.Add(ObterClientesPorPaginaAsync(url, pagina + i, registrosPorPagina));
                }

                var resultados = await Task.WhenAll(tasks);

                foreach (var clientesPagina in resultados)
                {
                    if (clientesPagina.Count > 0)
                    {
                        todosOsClientes.AddRange(clientesPagina);
                    }
                    else
                    {
                        continuarBuscando = false;
                        break;
                    }
                }

                pagina += paginasPorBatch;
            }

            return todosOsClientes;
        }

        private async Task<List<Client>> ObterClientesPorPaginaAsync(string url, int pagina, int registrosPorPagina)
        {
            var data = new
            {
                call = "ListarClientesResumido",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina,
                        registros_por_pagina = registrosPorPagina,
                        apenas_importado_api = "N"
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
                List<Client> clientesPagina = new List<Client>();

                foreach (var cliente in responseData.clientes_cadastro_resumido)
                {
                    clientesPagina.Add(new Client
                    {
                        CnpjCpf = cliente.cnpj_cpf,
                        CodigoCliente = Convert.ToInt64(cliente.codigo_cliente),
                        NomeFantasia = cliente.nome_fantasia,
                        RazaoSocial = cliente.razao_social
                    });
                }

                return clientesPagina;
            }
            else
            {
                _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}");
                return new List<Client>();
            }
        }
    }
}
