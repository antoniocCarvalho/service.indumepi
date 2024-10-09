using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using service.indumepi.Domain.Aggregates.Item;


namespace service.indumepi.Infra.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .IsRequired();

            builder.Property(u => u.Codigo).IsRequired();
            builder.Property(u => u.CodigoProduto).IsRequired();
            builder.Property(u => u.Descricao).IsRequired();
            builder.Property(u => u.ValorUnitario).IsRequired()
                    .HasColumnType("decimal(18, 2)"); // Define o tipo de coluna para decimal(18, 2)

            builder.Property(u => u.CodigoFamilia).IsRequired();
        }
    }
}


