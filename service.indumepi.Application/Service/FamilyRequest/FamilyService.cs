using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Domain.Aggregates.Family;
using service.indumepi.Domain.Aggregates.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Application.Service.FamilyRequest
{
    public class FamilyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ItemService> _logger;

        public FamilyService(HttpClient httpClient, ILogger<ItemService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Family>> ListarFamiliaAsync()
        {
            var url = "https://app.omie.com.br/api/v1/geral/familias/";

            var data = new
            {
                call = "PesquisarFamilias",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina = 1,
                        registros_por_pagina = 50,
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
                    List<Family> familys = new List<Family>();



                    foreach (var family in responseData.famCadastro)
                    {
                        familys.Add(new Family
                        {
                            CodFamilia = family.codFamilia,
                            CodInt = family.codInt,
                            Codigo = Convert.ToInt64(family.codigo),
                            Inativo = family.inativo,
                            NomeFamilia = family.nomeFamilia,
                        });
                    }

                    return familys;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Detalhes: {errorResponse}");
                    return new List<Family>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro na requisição HTTP: {ex.Message}");
                return new List<Family>();
            }
        }
    }
}

