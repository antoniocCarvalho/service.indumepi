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

        public OrdersController(OrderService orderService, OrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
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
    }
}
