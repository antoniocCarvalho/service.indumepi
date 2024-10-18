

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using service.indumepi.Domain.Aggregates.Item;
using service.indumepi.Domain.Aggregates.Order;

namespace service.indumepi.Infra.Data.Configurations
{
    public class OrderListConfiguration : IEntityTypeConfiguration<OrderList>
    {
        public void Configure(EntityTypeBuilder<OrderList> builder)
        {
            builder.ToTable("OrderList");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .IsRequired();

            builder.Property(o => o.NumeroPedido);
            builder.Property(o => o.CodigoCliente);
            builder.Property(o => o.CodigoEmpresa);
            builder.Property(o => o.Etapa);
            builder.Property(o => o.CodigoPedido);
            builder.Property(o => o.Cancelada);
            builder.Property(o => o.Encerrado);
            builder.Property(o => o.DtPrevisao);
        }
    }
}
