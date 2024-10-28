using System;

namespace service.indumepi.Domain.Aggregates.Estoque
{
    public class Estoque
    {
        public Guid Id { get; set; }
        public string CCodigo { get; set; }
        public string CDescricao { get; set; }
        public long CodigoLocalEstoque { get; set; }
        public int EstoqueMinimo { get; set; }
        public int Fisico { get; set; }
        public double NCMC { get; set; }
        public long NCodProd { get; set; }
        public int NPendente { get; set; }
        public decimal NPrecoUnitario { get; set; }
        public int NSaldo { get; set; }
        public int Reservado { get; set; }

        public Estoque() { }

        public Estoque(
            string cCodigo,
            string cDescricao,
            long codigoLocalEstoque,
            int estoqueMinimo,
            int fisico,
            double nCMC,
            long nCodProd,
            int nPendente,
            decimal nPrecoUnitario,
            int nSaldo,
            int reservado)
        {
            CCodigo = cCodigo;
            CDescricao = cDescricao;
            CodigoLocalEstoque = codigoLocalEstoque;
            EstoqueMinimo = estoqueMinimo;
            Fisico = fisico;
            NCMC = nCMC;
            NCodProd = nCodProd;
            NPendente = nPendente;
            NPrecoUnitario = nPrecoUnitario;
            NSaldo = nSaldo;
            Reservado = reservado;
        }
    }
}

