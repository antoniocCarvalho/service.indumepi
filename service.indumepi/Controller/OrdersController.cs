using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.OrderRequest;
using service.indumepi.Infra.Data.Features;
using System.Threading.Tasks;

namespace service.indumepi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _itemRepository;

        public OrdersController(OrderService orderService, OrderRepository orderRepository, ProductRepository itemRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> SynchronizeOrders()
        {
            var (orders, items) = await _orderService.RetrieveOrdersAsync();

            if (orders.Count > 0)
            {
                _orderRepository.SaveOrders(orders, items);
                return Ok(new { message = "Pedidos listados e salvos com sucesso!", totalOrders = orders.Count, totalItems = items.Count });
            }
            else
            {
                return NotFound("Nenhum pedido encontrado na API Omie.");
            }

        }

        [HttpGet("pedidos")]
        public async Task<IActionResult> GetPedidos()
        {
            var orders = _orderRepository.GetOrders();

            if (orders.Count > 0)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound("Nenhum pedido encontrado no banco de dados.");
            }
        }

        [HttpGet("pedidos/{numeropedido}")]
        public async Task<IActionResult> GetPedido(string numeropedido)
        {
            var orders = _orderRepository.GetOrderItems()
                                         .Where(o => o.NumeroPedido == numeropedido)
                                         .ToList();

            if (orders.Count > 0)
            {
                foreach (var order in orders)
                {
                    var item = _itemRepository.GetItemByCodigoProduto(order.CodigoProduto);
                    if (item != null)
                    {
                        order.NomeProduto = item.Descricao;
                    }
                }

                return Ok(orders);
            }
            else
            {
                return NotFound("Nenhum pedido encontrado no banco de dados.");
            }
        }

    }
}
