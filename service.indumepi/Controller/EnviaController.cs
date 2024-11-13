using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using service.indumepi.Application.Service.EnviaPed;

namespace service.indumepi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(PedidoService pedidoService, ILogger<PedidoController> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }

        [HttpPost("pedidos/{numeroPedido}/etapa/{etapa}")]
        public async Task<IActionResult> EnviarEtapa(int numeroPedido, string etapa)
        {
            if (numeroPedido <= 0 || string.IsNullOrWhiteSpace(etapa))
            {
                _logger.LogWarning("Parâmetros inválidos: número do pedido ou etapa estão ausentes.");
                return BadRequest("Número do pedido e etapa são obrigatórios.");
            }

            try
            {
                await _pedidoService.EnviarEtapaAsync(numeroPedido, etapa);
                return Ok("Informações enviadas com sucesso.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro de comunicação com a API: {ex.Message}");
                return StatusCode(500, "Erro ao enviar a etapa para o pedido.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro inesperado.");
            }
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("PedidoController está ativo.");
        }
    }
}
