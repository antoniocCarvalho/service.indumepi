using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using service.indumepi.Domain.Aggregates.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using service.indumepi.Domain.Aggregates.Family;

namespace service.indumepi.Infra.Data.Configurations
{
    public class FamiliesConfiguration : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.ToTable("Families");
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id)
                  .ValueGeneratedOnAdd()
                  .IsRequired();

            builder.Property(u => u.CodFamilia).IsRequired();
            builder.Property(u => u.CodInt).IsRequired();
            builder.Property(u => u.Codigo).IsRequired();
            builder.Property(u => u.Inativo).IsRequired();
            builder.Property(u => u.NomeFamilia).IsRequired();
        }
            
    }
}
