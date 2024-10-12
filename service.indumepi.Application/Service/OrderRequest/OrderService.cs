using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.indumepi.Domain.Aggregates.Order;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Application.Service.OrderRequest
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(HttpClient httpClient, ILogger<OrderService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        public async Task<(List<OrderList>, List<OrderProduct>)> RetrieveOrdersAsync()
        {
            _logger.LogInformation("Iniciando a recuperação de pedidos...");
            int pagina = 1;
            int registrosPorPagina = 100;
            bool continuarBuscando = true;

            var listaPed = new List<OrderList>();
            var pedItems = new List<OrderProduct>();

            while (continuarBuscando)
            {
                var pedidos = await ListarPedidosAsync(pagina, registrosPorPagina);
                if (pedidos.Count == 0)
                {
                    continuarBuscando = false;
                    break;
                }

                foreach (var pedido in pedidos)
                {
                    var cabecalho = pedido.cabecalho;
                    listaPed.Add(new OrderList
                    {
                        NumeroPedido = cabecalho.numero_pedido,
                        CodigoCliente = cabecalho.codigo_cliente,
                        CodigoEmpresa = cabecalho.codigo_empresa,
                        Etapa = cabecalho.etapa,
                        CodigoPedido = cabecalho.codigo_pedido,
                        Encerrado = cabecalho.encerrado,
                        DtPrevisao = DateTime.Parse(cabecalho.data_previsao.ToString())
                    });

                    foreach (var detalhe in pedido.det)
                    {
                        var produto = detalhe.produto;
                        pedItems.Add(new OrderProduct
                        {
                            NumeroPedido = cabecalho.numero_pedido,
                            CodigoCliente = Convert.ToInt64(produto.codigo_cliente),
                            CodigoEmpresa = Convert.ToInt64(produto.codigo_empresa),
                            Etapa = cabecalho.etapa,
                            Cfop = produto.cfop,
                            CnpjFabricante = produto.cnpj_fabricante,
                            CodigoProduto = Convert.ToInt64(produto.codigo_produto),
                            CodigoTabelaPreco = Convert.ToInt64(produto.codigo_tabela_preco),
                            Codigo = produto.codigo,
                            Ean = produto.ean,
                            IndicadorEscala = produto.indicador_escala,
                            MotivoIcmsDesonerado = produto.motivo_icms_desonerado,
                            Ncm = produto.ncm,
                            PercentualDesconto = Convert.ToInt64(produto.percentual_desconto),
                            Quantidade = Convert.ToInt64(produto.quantidade),
                            Reservado = produto.reservado,
                            TipoDesconto = produto.tipo_desconto,
                            Unidade = produto.unidade,
                            ValorDeducao = Convert.ToInt64(produto.valor_deducao),
                            ValorDesconto = Convert.ToInt64(produto.valor_desconto),
                            ValorIcmsDesonerado = Convert.ToInt64(produto.valor_icms_desonerado),
                            ValorMercadoria = Convert.ToInt64(produto.valor_mercadoria),
                            ValorTotal = Convert.ToInt64(produto.valor_total),
                            ValorUnitario = produto.valor_unitario,
                            PrimeiraSeparacao = Convert.ToInt64(produto.primeira_separacao),
                            SegundaSeparacao = Convert.ToInt64(produto.segunda_separacao),
                            Conferido = Convert.ToInt64(produto.conferido),
                            Enviado = Convert.ToInt64(produto.enviado),
                            Conferencia = Convert.ToInt64(produto.conferencia),
                            UserName = produto.user_name
                        });
                    }
                }

                pagina++;
            }

            return (listaPed, pedItems);
        }

        private async Task<List<dynamic>> ListarPedidosAsync(int pagina, int registrosPorPagina)
        {
            var url = "https://app.omie.com.br/api/v1/produtos/pedido/";
            var data = new
            {
                call = "ListarPedidos",
                app_key = "2801542865121",
                app_secret = "8a6559988fbe270c093bc62e90a6e063",
                param = new[]
                {
                    new
                    {
                        pagina,
                        registros_por_pagina = registrosPorPagina,
                        apenas_importado_api = "N",
                        etapa = "20"
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

                List<dynamic> pedidosPagina = new List<dynamic>();

                if (responseData != null && responseData.pedido_venda_produto != null)
                {
                    foreach (var pedido in responseData.pedido_venda_produto)
                    {
                        pedidosPagina.Add(pedido);
                    }
                }
                else
                {
                    _logger.LogWarning("Nenhum pedido encontrado na resposta da API.");
                }

                return pedidosPagina;
            }
            else
            {
                _logger.LogError($"Falha na requisição. Código de status: {response.StatusCode}. Página: {pagina}");
                return new List<dynamic>();
            }
        }
    }
}
