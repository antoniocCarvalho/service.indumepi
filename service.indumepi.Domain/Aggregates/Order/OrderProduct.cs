

namespace service.indumepi.Domain.Aggregates.Order
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; }
        public long CodigoCliente { get; set; }
        public long CodigoEmpresa { get; set; }
        public string Etapa { get; set; }
        public string Cfop { get; set; }
        public string CnpjFabricante { get; set; }
        public long CodigoProduto { get; set; }
        public long CodigoTabelaPreco { get; set; }
        public string Codigo { get; set; }
        public string Ean { get; set; }
        public string IndicadorEscala { get; set; }
        public string MotivoIcmsDesonerado { get; set; }
        public string Ncm { get; set; }
        public long PercentualDesconto { get; set; }
        public long Quantidade { get; set; }
        public string Reservado { get; set; }
        public string TipoDesconto { get; set; }
        public string Unidade { get; set; }
        public long ValorDeducao { get; set; }
        public long ValorDesconto { get; set; }
        public long ValorIcmsDesonerado { get; set; }
        public long ValorMercadoria { get; set; }
        public long ValorTotal { get; set; }
        public decimal ValorUnitario { get; set; }
        public long PrimeiraSeparacao { get; set; }
        public long SegundaSeparacao { get; set; }
        public long Conferido { get; set; }
        public long Enviado { get; set; }
        public long Conferencia { get; set; }
        public string UserName { get; set; }

        public OrderProduct()
        {
        }

        public OrderProduct(string numeroPedido, long codigoCliente, long codigoEmpresa, string etapa, string cfop, string cnpjFabricante, long codigoProduto, long codigoTabelaPreco, string codigo, string ean, string indicadorEscala, string motivoIcmsDesonerado, string ncm, long percentualDesconto, long quantidade, string reservado, string tipoDesconto, string unidade, long valorDeducao, long valorDesconto, long valorIcmsDesonerado, long valorMercadoria, long valorTotal, decimal valorUnitario, long primeiraSeparacao, long segundaSeparacao, long conferido, long enviado, long conferencia, string userName)
        {
            NumeroPedido = numeroPedido;
            CodigoCliente = codigoCliente;
            CodigoEmpresa = codigoEmpresa;
            Etapa = etapa;
            Cfop = cfop;
            CnpjFabricante = cnpjFabricante;
            CodigoProduto = codigoProduto;
            CodigoTabelaPreco = codigoTabelaPreco;
            Codigo = codigo;
            Ean = ean;
            IndicadorEscala = indicadorEscala;
            MotivoIcmsDesonerado = motivoIcmsDesonerado;
            Ncm = ncm;
            PercentualDesconto = percentualDesconto;
            Quantidade = quantidade;
            Reservado = reservado;
            TipoDesconto = tipoDesconto;
            Unidade = unidade;
            ValorDeducao = valorDeducao;
            ValorDesconto = valorDesconto;
            ValorIcmsDesonerado = valorIcmsDesonerado;
            ValorMercadoria = valorMercadoria;
            ValorTotal = valorTotal;
            ValorUnitario = valorUnitario;
            PrimeiraSeparacao = primeiraSeparacao;
            SegundaSeparacao = segundaSeparacao;
            Conferido = conferido;
            Enviado = enviado;
            Conferencia = conferencia;
            UserName = userName;
        }
    }
}
