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
                    existingCustomer.NomeFantasia = customer.NomeFantasia;
                    existingCustomer.RazaoSocial = customer.RazaoSocial;
                }
                else
                {
                    _context.Client.Add(customer);
                }
            }
            _context.SaveChanges();
        }



        public List<Client> GetCustomers() {
            return _context.Client
                .Select(client => new Client
                {
                    CodigoCliente = client.CodigoCliente,
                    RazaoSocial = client.RazaoSocial
                })
                .ToList();
        }
    }
}
