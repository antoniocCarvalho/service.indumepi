using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Domain.Aggregates.Order
{
    public class OrderList
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; }
        public long? CodigoCliente { get; set; } 
        public long? CodigoEmpresa { get; set; } 
        public string Etapa { get; set; }
        public long? CodigoPedido { get; set; } 
        public string Cancelada { get; set; }
        public string? Encerrado { get; set; }
        public DateTime? DtPrevisao { get; set; }

        public OrderList()
        {
        }

        public OrderList(string numeroPedido, long? codigoCliente, long? codigoEmpresa, string etapa, long? codigoPedido, string cancelada, string encerrado, DateTime? dtPrevisao)
        {
            NumeroPedido = numeroPedido;
            CodigoCliente = codigoCliente;
            CodigoEmpresa = codigoEmpresa;
            Etapa = etapa;
            CodigoPedido = codigoPedido;
            Cancelada = cancelada;
            Encerrado = encerrado;
            DtPrevisao = dtPrevisao;
        }
    }
}
