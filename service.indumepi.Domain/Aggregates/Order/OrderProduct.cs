using System.ComponentModel.DataAnnotations.Schema;

namespace service.indumepi.Domain.Aggregates.Order
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public string NumeroPedido { get; set; }
        public long CodigoCliente { get; set; } // Agora é nullable
        public long CodigoEmpresa { get; set; } // Agora é nullable
        public string? Etapa { get; set; } // Agora é nullable
        public string? Cfop { get; set; } // Agora é nullable
        public string? CnpjFabricante { get; set; } // Agora é nullable
        public long CodigoProduto { get; set; } // Agora é nullable
        public long CodigoTabelaPreco { get; set; } // Agora é nullable
        public string? Codigo { get; set; } // Agora é nullable
        public string? Ean { get; set; } // Agora é nullable
        public string? IndicadorEscala { get; set; } // Agora é nullable
        public string? MotivoIcmsDesonerado { get; set; } // Agora é nullable
        public string Ncm { get; set; } // Agora é nullable
        public long? PercentualDesconto { get; set; } // Agora é nullable
        public long? Quantidade { get; set; } // Agora é nullable
        public string? Reservado { get; set; } // Agora é nullable
        public string? TipoDesconto { get; set; } // Agora é nullable
        public string? Unidade { get; set; } // Agora é nullable
        public long? ValorDeducao { get; set; } // Agora é nullable
        public long? ValorDesconto { get; set; } // Agora é nullable
        public long? ValorIcmsDesonerado { get; set; } // Agora é nullable
        public long? ValorMercadoria { get; set; } // Agora é nullable
        public long? ValorTotal { get; set; } // Agora é nullable
        public decimal? ValorUnitario { get; set; } // Agora é nullable
        public long PrimeiraSeparacao { get; set; } // Agora é nullable
        public long SegundaSeparacao { get; set; } // Agora é nullable
        public long Conferido { get; set; } // Agora é nullable
        public long? Enviado { get; set; } // Agora é nullable
        public long? Conferencia { get; set; } // Agora é nullable
        public string? UserName { get; set; } // Agora é nullable

        [NotMapped]
        public string? NomeProduto { get; set; } // Agora é nullable

        public bool Editado { get; set; }

        public OrderProduct()
        {
        }

        public OrderProduct(string numeroPedido, long codigoCliente, long codigoEmpresa, string? etapa, string? cfop, string? cnpjFabricante, long codigoProduto, long codigoTabelaPreco, string? codigo, string? ean, string? indicadorEscala, string? motivoIcmsDesonerado, string? ncm, long? percentualDesconto, long? quantidade, string? reservado, string? tipoDesconto, string? unidade, long? valorDeducao, long? valorDesconto, long? valorIcmsDesonerado, long? valorMercadoria, long? valorTotal, decimal? valorUnitario, long primeiraSeparacao, long segundaSeparacao, long conferido, long? enviado, long? conferencia, string? userName, bool editado)
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
            Editado = editado;
        }
    }
}
