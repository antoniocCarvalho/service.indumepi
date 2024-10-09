using Microsoft.Extensions.Logging;
using service.indumepi.Application.Service;
using service.indumepi.Application.Service.ClientRequest;
using service.indumepi.Application.Service.ItemRequest;
using service.indumepi.Domain.Aggregates;
using service.indumepi.Domain.Aggregates.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.indumepi.Infra.Data.Features
{
    public class ClientRepository
    {
        private readonly Context _context;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(Context context, ILogger<ClientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void DeleteAll()
        {
            _context.Client.RemoveRange(_context.Client.ToList());
            _context.SaveChanges();
        }

        public void SaveCustomers(List<Client> customers)
        {
            foreach (var customer in customers)
            {
                var existingCustomer = _context.Client
                    .FirstOrDefault(p => p.CnpjCpf == customer.CnpjCpf || p.CodigoCliente == customer.CodigoCliente);

                if (existingCustomer != null)
                {
                    // Atualiza apenas os campos que podem mudar
                    existingCustomer.NomeFantasia = customer.NomeFantasia;
                    existingCustomer.RazaoSocial = customer.RazaoSocial;
                }
                else
                {
                    // Adiciona um novo cliente no banco de dados
                    _context.Client.Add(customer);
                }
            }
            _context.SaveChanges();
        }

        public async Task AtualizarClientesAsync(ClientService clientService)
        {
            try
            {
                DeleteAll();  // Remove todos os registros anteriores

                var clientes = await clientService.ListarClientesAsync();
                _logger.LogInformation($"Clientes recebidos: {clientes.Count}");

                if (clientes.Any())
                {
                    SaveCustomers(clientes);
                    _logger.LogInformation("Clientes inseridos no banco de dados com sucesso.");

                    // Supondo que a resposta contenha informação de total de páginas
                    var totalDePaginas = clientes.Count / 50 + 1;

                    // Itera pelas páginas restantes e atualiza o banco de dados
                    for (int pagina = 2; pagina <= totalDePaginas; pagina++)
                    {
                        var paginaClientes = await clientService.ListarClientesAsync(pagina);
                        SaveCustomers(paginaClientes);
                        _logger.LogInformation($"Inserção de clientes concluída - Página {pagina}");
                    }
                }
                else
                {
                    _logger.LogWarning("Nenhum cliente encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar clientes: {ex.Message}");
            }
        }
    }
}
