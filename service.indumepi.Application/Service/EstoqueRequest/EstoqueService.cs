using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Estoque;

namespace service.indumepi.Application.Service.EstoqueRequest
{
    public class EstoqueService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EstoqueService> _logger;

        public EstoqueService(HttpClient httpClient, ILogger<EstoqueService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Estoque>> ListarTodosOsProdutosAsync()
        {
            var url = "https://app.omie.com.br/api/v1/estoque/consulta/";
            var todosOsProdutos = new List<Estoque>();
            int registrosPorPagina = 200; 
            int paginasPorBatch = 5; 
            int pagina = 1;
            bool continuarBuscando = true;

            while (continuarBuscando)
            {
                var tasks = new List<Task<List<Estoque>>>();

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

        private async Task<List<Estoque>> ObterProdutosPorPaginaAsync(string url, int pagina, int registrosPorPagina)
        {
            var data = new
            {
                call = "ListarPosEstoque",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        nPagina = pagina,
                        nRegPorPagina = registrosPorPagina,
                        dDataPosicao = DateTime.Now.ToString("dd/MM/yyyy"),
                        cExibeTodos = "S"
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
                List<Estoque> produtosPagina = new List<Estoque>();

                foreach (var produto in responseData.produto_servico_cadastro)
                {
                    produtosPagina.Add(new Estoque
                    {
                            CCodigo = produto.cCodigo,
                            CDescricao = produto.cDescricao,
                            CodigoLocalEstoque = Convert.ToInt64(produto.codigo_local_estoque),
                            EstoqueMinimo = Convert.ToInt32(produto.estoque_minimo),
                            Fisico = Convert.ToInt32(produto.fisico),
                            NCMC = Convert.ToDouble(produto.nCMC),
                            NCodProd = Convert.ToInt64(produto.nCodProd),
                            NPendente = Convert.ToInt32(produto.nPendente),
                            NPrecoUnitario = (decimal)produto.nPrecoUnitario,
                            NSaldo = Convert.ToInt32(produto.nSaldo),
                            Reservado = Convert.ToInt32(produto.reservado)
                    });
                }

                return produtosPagina;
            }
            else
            {
                _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}");
                return new List<Estoque>();
            }
        }
    }
}

