using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Domain.Aggregates.Client
{
    public class Client
    {
        public Guid Id { get; set; }
        public string CnpjCpf { get; set; }
        public long CodigoCliente { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }


        public Client() { }


        public Client( string cnpjCpf, long codigoCliente, string nomeFantasia, string razaoSocial)
        {
            
            CnpjCpf = cnpjCpf;
            CodigoCliente = codigoCliente;
            NomeFantasia = nomeFantasia;
            RazaoSocial = razaoSocial;
        }
    }
}

