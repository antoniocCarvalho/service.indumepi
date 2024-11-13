using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using service.indumepi.Domain.Aggregates.Estoque;

namespace service.indumepi.Infra.Data.Configurations
{
    public class EstoqueConfiguration : IEntityTypeConfiguration<Estoque>
    {
        public void Configure(EntityTypeBuilder<Estoque> builder)
        {
            builder.ToTable("Estoque");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(e => e.CCodigo)
                   .IsRequired()
                   .HasMaxLength(50); // Ajuste o tamanho conforme necessário

            builder.Property(e => e.CDescricao)
                   .IsRequired()
                   .HasMaxLength(200); // Ajuste conforme necessário

            builder.Property(e => e.CodigoLocalEstoque)
                   .IsRequired();

            builder.Property(e => e.EstoqueMinimo)
                   .IsRequired();

            builder.Property(e => e.Fisico)
                   .IsRequired();

            builder.Property(e => e.NCMC)
                   .IsRequired()
                   .HasColumnType("float");

            builder.Property(e => e.NCodProd)
                   .IsRequired();

            builder.Property(e => e.NPendente)
                   .IsRequired();

            builder.Property(e => e.NPrecoUnitario)
                   .IsRequired()
                   .HasColumnType("decimal(18, 2)"); // Define o tipo decimal(18, 2)

            builder.Property(e => e.NSaldo)
                   .IsRequired();

            builder.Property(e => e.Reservado)
                   .IsRequired();
        }
    }
}
