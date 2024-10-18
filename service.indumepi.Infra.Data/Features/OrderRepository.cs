using Microsoft.Extensions.Logging;
using service.indumepi.Domain.Aggregates.Order;
using System.Collections.Generic;
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
            _logger.LogInformation("Removendo todos os registros de pedidos e itens...");
            _context.OrderList.RemoveRange(_context.OrderList.ToList());
            _context.OrderProduct.RemoveRange(_context.OrderProduct.ToList());
            _context.SaveChanges();
        }


        public void SaveOrders(List<OrderList> orders, List<OrderProduct> items)
        {
            _context.OrderList.AddRange(orders);
            _context.OrderProduct.AddRange(items);
            _context.SaveChanges();

            _logger.LogInformation($"Salvos {orders.Count} pedidos e {items.Count} itens com sucesso no banco de dados.");
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
                    UserName = item.UserName ?? "Usuário não especificado", // Tratamento de nulo para strings
                })
                .ToList();
        }

    }
}
