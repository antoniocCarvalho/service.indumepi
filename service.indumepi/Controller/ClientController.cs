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
            var produtos = await _clientService.ListarTodosOsClientesAsync();
            if (produtos.Any())
            {
                _clientRepository.DeleteAll();
                _clientRepository.SaveCustomers(produtos);
                return Ok(new { message = "Clientes listados e salvos com sucesso!", clienteSalvo = produtos.Count });
            }
            else
            {
                return NotFound("Nenhum Cliente encontrado na API Omie.");
            }
        }



        [HttpGet("Clientesfantasia")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = _clientRepository.GetCustomers();
            if (clientes.Any())
            {
                return Ok(clientes);
            }
            else
            {
                return NotFound("Nenhum Cliente encontrado no banco de dados.");
            }
        }
    }
}

