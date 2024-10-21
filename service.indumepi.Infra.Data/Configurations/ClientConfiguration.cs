using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using service.indumepi.Domain.Aggregates.Client;

namespace service.indumepi.Infra.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired(); 

            builder.Property(u => u.CnpjCpf)
                   .IsRequired();

            builder.Property(u => u.CodigoCliente)
                   .IsRequired();

            builder.Property(u => u.NomeFantasia)
                   .IsRequired();

            builder.Property(u => u.RazaoSocial)
                   .IsRequired();
        }
    }
}
