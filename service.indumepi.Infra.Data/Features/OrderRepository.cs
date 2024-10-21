using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using service.indumepi.Domain.Aggregates.Order;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

namespace service.indumepi.Infra.Data.Features
{
    public class OrderRepository
    {
        private readonly Context _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(Context context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void DeleteAll()
        {
            try
            {
                _logger.LogInformation("Removendo todos os registros de pedidos e itens...");

                // Seleciona todos os pedidos da tabela OrderList
                var orderList = _context.OrderList
                                        .Select(o => new OrderList
                                        {
                                            Id = o.Id,
                                            NumeroPedido = o.NumeroPedido ?? "N/A",
                                            CodigoCliente = o.CodigoCliente.HasValue ? o.CodigoCliente.Value : 0,
                                            CodigoEmpresa = o.CodigoEmpresa.HasValue ? o.CodigoEmpresa.Value : 0,
                                            Etapa = o.Etapa ?? "Desconhecido",
                                            CodigoPedido = o.CodigoPedido.HasValue ? o.CodigoPedido.Value : 0,
                                            Cancelada = o.Cancelada ?? "Não",
                                            Encerrado = o.Encerrado ?? "Não",
                                            DtPrevisao = o.DtPrevisao ?? DateTime.MinValue
                                        })
                                        .AsNoTracking()
                                        .ToList();

                // Seleciona todos os produtos da tabela OrderProduct, exceto os que têm Editado = true
                var orderProducts = _context.OrderProduct
                                            .Where(op => op.Editado == false)
                                            .Select(op => new OrderProduct
                                            {
                                                Id = op.Id,
                                                NumeroPedido = op.NumeroPedido ?? "N/A",
                                                CodigoProduto = op.CodigoProduto,
                                                Codigo = op.Codigo ?? "N/A",
                                                Conferencia = op.Conferencia,
                                                Quantidade = op.Quantidade,
                                                UserName = op.UserName ?? "Usuário não especificado",
                                                Editado = op.Editado
                                            })
                                            .AsNoTracking()
                                            .ToList();

                if (orderList.Any())
                {
                    _context.OrderList.RemoveRange(orderList);
                }

                if (orderProducts.Any())
                {
                    _context.OrderProduct.RemoveRange(orderProducts);
                }

                _context.SaveChanges();
                _logger.LogInformation("Todos os registros de pedidos e itens foram removidos com sucesso, exceto os que possuem Editado = true.");
            }
            catch (SqlNullValueException ex)
            {
                _logger.LogError(ex, "Erro ao remover registros do banco de dados: valor nulo encontrado.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover registros do banco de dados.");
                throw;
            }
        }




        public void SaveOrders(List<OrderList> orders, List<OrderProduct> items)
        {
            var filteredOrders = orders.Where(order => order.Encerrado != "S").ToList();
            var filteredItems = items.Where(item => filteredOrders.Any(order => order.NumeroPedido == item.NumeroPedido)).ToList();

            foreach (var order in filteredOrders)
            {
                var existingOrder = _context.OrderList
                                            .FirstOrDefault(o => o.NumeroPedido == order.NumeroPedido);

                if (existingOrder != null)
                {
                    // Atualiza os campos do pedido existente
                    existingOrder.CodigoCliente = order.CodigoCliente;
                    existingOrder.CodigoEmpresa = order.CodigoEmpresa;
                    existingOrder.Etapa = order.Etapa;
                    existingOrder.CodigoPedido = order.CodigoPedido;
                    existingOrder.Cancelada = order.Cancelada;
                    existingOrder.Encerrado = order.Encerrado;
                    existingOrder.DtPrevisao = order.DtPrevisao;
                    _context.OrderList.Update(existingOrder);
                }
                else
                {
                    // Adiciona um novo pedido se não existir
                    _context.OrderList.Add(order);
                }
            }

            foreach (var item in filteredItems)
            {
                var existingProduct = _context.OrderProduct
                                              .FirstOrDefault(op => op.NumeroPedido == item.NumeroPedido && op.CodigoProduto == item.CodigoProduto);

                if (existingProduct != null)
                {
                    // Se o produto já existe e foi editado (Editado == true), não faz nada
                    if (existingProduct.Editado)
                    {
                        continue;
                    }

                    // Atualiza os campos do produto existente
                    existingProduct.NumeroPedido = item.NumeroPedido;
                    existingProduct.CodigoProduto = item.CodigoProduto;
                    existingProduct.Codigo = item.Codigo;
                    existingProduct.Conferencia = item.Conferencia;
                    existingProduct.PrimeiraSeparacao = item.PrimeiraSeparacao;
                    existingProduct.SegundaSeparacao = item.SegundaSeparacao;
                    existingProduct.Quantidade = item.Quantidade;
                    existingProduct.UserName = item.UserName;
                    existingProduct.Editado = item.Editado;
                    _context.OrderProduct.Update(existingProduct);
                }
                else
                {
                    // Adiciona um novo produto se não existir e se não houver outro com a mesma combinação de NumeroPedido e CodigoProduto que foi editado
                    var conflictingProduct = _context.OrderProduct
                                                     .FirstOrDefault(op => op.NumeroPedido == item.NumeroPedido &&
                                                                           op.CodigoProduto == item.CodigoProduto &&
                                                                           op.Editado);
                    if (conflictingProduct == null)
                    {
                        _context.OrderProduct.Add(item);
                    }
                }
            }

            _context.SaveChanges();

            _logger.LogInformation($"Salvos {filteredOrders.Count} pedidos e {filteredItems.Count} itens com sucesso no banco de dados.");
        }



        public List<OrderList> GetOrders()
        {
            return _context.OrderList
                .Select(order => new OrderList
                {
                    Id = order.Id,
                    NumeroPedido = order.NumeroPedido ?? "N/A",
                    CodigoCliente = order.CodigoCliente.HasValue ? order.CodigoCliente.Value : 0, // Valor padrão se for nulo
                    CodigoEmpresa = order.CodigoEmpresa.HasValue ? order.CodigoEmpresa.Value : 0, // Valor padrão se for nulo
                    Etapa = order.Etapa ?? "Indefinido",
                    CodigoPedido = order.CodigoPedido.HasValue ? order.CodigoPedido.Value : 0, // Valor padrão se for nulo
                    Cancelada = order.Cancelada ?? "Não",
                    Encerrado = order.Encerrado ?? "Não",
                    DtPrevisao = order.DtPrevisao ?? DateTime.MinValue // Valor padrão se for nulo
                })
                .ToList();
        }


        public List<OrderProduct> GetOrderItems()
        {
            return _context.OrderProduct
                .Select(item => new OrderProduct
                {
                    Id = item.Id,
                    NumeroPedido = item.NumeroPedido,
                    CodigoProduto = item.CodigoProduto,
                    Codigo = item.Codigo,
                    Conferencia = item.Conferencia,
                    Quantidade = item.Quantidade,
                    PrimeiraSeparacao = item.PrimeiraSeparacao,
                    SegundaSeparacao = item.SegundaSeparacao,
                    UserName = item.UserName ?? "Usuário não especificado", // Tratamento de nulo para strings
                })
                .ToList();
        }



        public void UpdateOrderProduct(OrderProduct orderProduct)
        {
            var existingOrder = _context.OrderProduct.Find(orderProduct.Id);
            if (existingOrder != null)
            {
                // Atualiza apenas os campos específicos
                existingOrder.PrimeiraSeparacao = orderProduct.PrimeiraSeparacao;
                existingOrder.SegundaSeparacao = orderProduct.SegundaSeparacao;
                existingOrder.Editado = true;

                // Salva as mudanças
                _context.SaveChanges();
            }
        }


    }
}
