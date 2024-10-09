using Microsoft.AspNetCore.Mvc;
using service.indumepi.Application.Service.ClientRequest;
using service.indumepi.Infra.Data.Features;

namespace service.indumepi.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly ClientRepository _clientRepository;


        public ClientController(ClientService clientService, ClientRepository clientRepository)
        {
            _clientService = clientService;
            _clientRepository = clientRepository;
        }

        [HttpGet("Cliente")]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _clientService.ListarClientesAsync();
            if (produtos.Any())
            {
                _clientRepository.DeleteAll();
                _clientRepository.SaveCustomers(produtos);
                return Ok(new { message = "Produtos listados e salvos com sucesso!", produtosSalvos = produtos.Count });
            }
            else
            {
                return NotFound("Nenhum produto encontrado na API Omie.");
            }
        }
    }
}

