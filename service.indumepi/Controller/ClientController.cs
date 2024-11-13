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
        public async Task<IActionResult> ListarClientes()
        {
            try
            {
                var clientes = await _clientService.ListarTodosOsClientesAsync();

                // Verifica se o resultado é nulo ou a lista está vazia
                if (clientes == null || !clientes.Any())
                {
                    return NotFound(new { message = "Nenhum Cliente encontrado na API Omie." });
                }

                // Operações de limpeza e salvamento no repositório
                _clientRepository.DeleteAll();
                _clientRepository.SaveCustomers(clientes);

                return Ok(new { message = "Clientes listados e salvos com sucesso!", clienteSalvo = clientes.Count });
            }
            catch (Exception ex)
            {
                // Loga o erro para análise

                // Retorna uma resposta de erro detalhada
                return StatusCode(500, new { message = "Erro interno ao listar clientes.", detalhes = ex.Message });
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

