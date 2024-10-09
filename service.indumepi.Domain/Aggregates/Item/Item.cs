using System.Numerics;

namespace service.indumepi.Domain.Aggregates.Item
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public long CodigoProduto { get; set; }
        public string Descricao { get; set; }
        public decimal ValorUnitario { get; set; }
        public long CodigoFamilia { get; set; }

        public Item() { }

        public Item(string codigo, long codigoProduto, string descricao, decimal valorUnitario, long codigoFamilia)
        {
            Codigo = codigo;
            CodigoProduto = codigoProduto;
            Descricao = descricao;
            ValorUnitario = valorUnitario;
            CodigoFamilia = codigoFamilia;
        }
    }



}
