using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.OrderRequest;
using service.indumepi.Domain.Aggregates.Order;
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
                _orderRepository.DeleteAll();
                _orderRepository.SaveOrders(orders, items);
                return Ok();
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

        [HttpPut("pedidos/{numeropedido}")]
        public async Task<IActionResult> UpdatePedido(string numeropedido, [FromBody] SeparacaoDto separacaoDto)
        {
            // Busca o pedido existente com base no número do pedido
            var existingOrders = _orderRepository.GetOrderItems()
                                                 .Where(o => o.NumeroPedido == numeropedido)
                                                 .ToList();

            if (existingOrders.Count == 0)
            {
                return NotFound("Nenhum pedido encontrado com esse número.");
            }

            foreach (var existingOrder in existingOrders)
            {
                if (existingOrder.Id == separacaoDto.Id)
                {
                    bool hasChanged = existingOrder.PrimeiraSeparacao != separacaoDto.PrimeiraSeparacao ||
                                      existingOrder.SegundaSeparacao != separacaoDto.SegundaSeparacao ||
                                      existingOrder.Conferido != separacaoDto.Conferido;

                    if (hasChanged)
                    {
                        existingOrder.PrimeiraSeparacao = separacaoDto.PrimeiraSeparacao;
                        existingOrder.SegundaSeparacao = separacaoDto.SegundaSeparacao;
                        existingOrder.Conferido = separacaoDto.Conferido;
                        existingOrder.UserName = separacaoDto.UserName;
                        existingOrder.Editado = true; // Marca como editado

                        _orderRepository.UpdateOrderProduct(existingOrder);
                    }
                }
            }

            return Ok("Pedido atualizado com sucesso.");
        }



        [HttpGet("produto/{codigoProduto}")]
        public async Task<IActionResult> GetProduto(long codigoProduto)
        {
            var produto = _orderRepository.GetByProductCode(codigoProduto);
            if (produto != null)
            {
                return Ok(produto);
            }
            else
            {
                return NotFound("Produto não encontrado.");
            }
        }

    }
}
