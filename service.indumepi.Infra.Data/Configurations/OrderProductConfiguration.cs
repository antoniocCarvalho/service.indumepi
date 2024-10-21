using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using service.indumepi.Domain.Aggregates.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.indumepi.Infra.Data.Configurations
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProduct");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .IsRequired();

            builder.Property(x => x.NumeroPedido);
                

            builder.Property(x => x.CodigoCliente);

            builder.Property(x => x.CodigoEmpresa);

            builder.Property(x => x.Etapa);

            builder.Property(x => x.Cfop);

            builder.Property(x => x.CnpjFabricante);

            builder.Property(x => x.CodigoProduto);

            builder.Property(x => x.CodigoTabelaPreco);

            builder.Property(x => x.Codigo);

            builder.Property(x => x.Ean);

            builder.Property(x => x.IndicadorEscala);

            builder.Property(x => x.MotivoIcmsDesonerado);

            builder.Property(x => x.Ncm);

            builder.Property(x => x.PercentualDesconto);

            builder.Property(x => x.Quantidade);

            builder.Property(x => x.Reservado);

            builder.Property(x => x.TipoDesconto);

            builder.Property(x => x.Unidade);

            builder.Property(x => x.ValorDeducao);

            builder.Property(x => x.ValorDesconto);

            builder.Property(x => x.ValorIcmsDesonerado);

            builder.Property(x => x.ValorMercadoria);

            builder.Property(x => x.ValorTotal);

            builder.Property(x => x.ValorUnitario)
             .HasColumnType("decimal(18,2)");


            builder.Property(x => x.PrimeiraSeparacao);

            builder.Property(x => x.SegundaSeparacao);

            builder.Property(x => x.Conferido);

            builder.Property(x => x.Enviado);

            builder.Property(x => x.Conferencia);

            builder.Property(x => x.UserName);

            builder.Property(x => x.Editado)
                  .IsRequired()
                  .HasDefaultValue(false);
        }
    }
}
