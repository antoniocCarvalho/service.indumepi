using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Family;

namespace service.indumepi.Application.Service.FamilyRequest
{
    public class FamilyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FamilyService> _logger;

        public FamilyService(HttpClient httpClient, ILogger<FamilyService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        public async Task<List<Family>> ListarTodasAsFamiliasAsync()
        {
            var url = "https://app.omie.com.br/api/v1/geral/familias/";
            var todasAsFamilias = new List<Family>();
            int registrosPorPagina = 200; 
            int paginasPorBatch = 5; 
            int pagina = 1;
            bool continuarBuscando = true;

            while (continuarBuscando)
            {
                var tasks = new List<Task<List<Family>>>();

                for (int i = 0; i < paginasPorBatch; i++)
                {
                    tasks.Add(ObterFamiliasPorPaginaAsync(url, pagina + i, registrosPorPagina));
                }

                var resultados = await Task.WhenAll(tasks);

                foreach (var familiasPagina in resultados)
                {
                    if (familiasPagina.Count > 0)
                    {
                        todasAsFamilias.AddRange(familiasPagina);
                    }
                    else
                    {
                        continuarBuscando = false;
                        break;
                    }
                }

                pagina += paginasPorBatch;
            }

            return todasAsFamilias;
        }

        private async Task<List<Family>> ObterFamiliasPorPaginaAsync(string url, int pagina, int registrosPorPagina)
        {
            var data = new
            {
                call = "PesquisarFamilias",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina,
                        registros_por_pagina = registrosPorPagina
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
                List<Family> familiasPagina = new List<Family>();

                foreach (var family in responseData.famCadastro)
                {
                    familiasPagina.Add(new Family
                    {
                        CodFamilia = family.codFamilia,
                        CodInt = family.codInt,
                        Codigo = Convert.ToInt64(family.codigo),
                        Inativo = family.inativo,
                        NomeFamilia = family.nomeFamilia
                    });
                }

                return familiasPagina;
            }
            else
            {
                _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}");
                return new List<Family>();
            }
        }
    }
}
