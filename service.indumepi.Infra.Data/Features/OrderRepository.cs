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
    }
}
