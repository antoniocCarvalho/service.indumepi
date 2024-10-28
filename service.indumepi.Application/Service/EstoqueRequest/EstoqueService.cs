using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
        private readonly SemaphoreSlim _semaphore;

        public EstoqueService(HttpClient httpClient, ILogger<EstoqueService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _semaphore = new SemaphoreSlim(1); // Limite de 1 requisição simultânea para evitar bloqueios
        }

        public async Task<List<Estoque>> ListarTodosOsProdutosAsync()
        {
            var url = "https://app.omie.com.br/api/v1/estoque/consulta/";
            var todosOsProdutos = new List<Estoque>();
            int registrosPorPagina = 200;
            int pagina = 1;
            bool continuarBuscando = true;

            while (continuarBuscando)
            {
                var tasks = new List<Task<List<Estoque>>>();

                for (int i = 0; i < 1; i++) // Reduzido para 1 requisição por vez
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

                pagina += 1;

                // Aguardar 2 segundos antes de iniciar o próximo lote para evitar bloqueios
                await Task.Delay(1);
            }

            return todosOsProdutos;
        }

        private async Task<List<Estoque>> ObterProdutosPorPaginaAsync(string url, int pagina, int registrosPorPagina)
        {
            await _semaphore.WaitAsync(); // Garantir que não ultrapasse 1 requisição simultânea

            int retryCount = 0;
            const int maxRetries = 3;
            const int baseDelayMs = 2000;

            try
            {
                while (retryCount < maxRetries)
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

                    _logger.LogInformation($"Payload da requisição para a página {pagina}: {jsonPayload}");

                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    _httpClient.DefaultRequestHeaders.Clear();
                    var response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                        List<Estoque> produtosPagina = new List<Estoque>();

                        if (responseData.produtos != null && responseData.produtos.HasValues)
                        {
                            foreach (var produto in responseData.produtos)
                            {
                                produtosPagina.Add(new Estoque
                                {
                                    CCodigo = produto.cCodigo != null ? (string)produto.cCodigo : string.Empty,
                                    CDescricao = produto.cDescricao != null ? (string)produto.cDescricao : string.Empty,
                                    CodigoLocalEstoque = produto.codigo_local_estoque != null ? Convert.ToInt64(produto.codigo_local_estoque) : 0,
                                    EstoqueMinimo = produto.estoque_minimo != null ? Convert.ToInt32(produto.estoque_minimo) : 0,
                                    Fisico = produto.fisico != null ? Convert.ToInt32(produto.fisico) : 0,
                                    NCMC = produto.nCMC != null ? Convert.ToDouble(produto.nCMC) : 0,
                                    NCodProd = produto.nCodProd != null ? Convert.ToInt64(produto.nCodProd) : 0,
                                    NPendente = produto.nPendente != null ? Convert.ToInt32(produto.nPendente) : 0,
                                    NPrecoUnitario = produto.nPrecoUnitario != null ? (decimal)produto.nPrecoUnitario : 0,
                                    NSaldo = produto.nSaldo != null ? Convert.ToInt32(produto.nSaldo) : 0,
                                    Reservado = produto.reservado != null ? Convert.ToInt32(produto.reservado) : 0
                                });
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"A resposta JSON não contém produtos ou está vazia. Página: {pagina}");
                        }

                        return produtosPagina;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}. Resposta: {errorContent}");

                        // Verificar se o erro é do tipo "requisição já em execução" e implementar backoff exponencial
                        if (errorContent.Contains("Client-8020"))
                        {
                            retryCount++;
                            int delay = baseDelayMs * (int)Math.Pow(2, retryCount); // Exponential backoff
                            _logger.LogWarning($"Tentativa {retryCount} falhou devido ao limite de requisições. Aguardando {delay} ms antes de tentar novamente.");
                            await Task.Delay(delay);
                        }
                        else
                        {
                            return new List<Estoque>();
                        }
                    }
                }

                // Se todas as tentativas falharem
                _logger.LogError($"Falha após {maxRetries} tentativas para a página {pagina}");
                return new List<Estoque>();
            }
            finally
            {
                _semaphore.Release(); // Liberar o semáforo para permitir outra requisição
            }
        }
    }
}
