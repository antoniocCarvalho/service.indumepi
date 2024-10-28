using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using service.indumepi.Domain.Aggregates.Client;
using service.indumepi.Domain.Aggregates.Estoque;
using service.indumepi.Domain.Aggregates.Family;
using service.indumepi.Domain.Aggregates.Item;
using service.indumepi.Domain.Aggregates.Order;

namespace service.indumepi.Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Item> Item { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        public DbSet<Estoque> Estoque { get; set; }
    }

    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Indumepi;Trusted_Connection=True;TrustServerCertificate=True");

            return new Context(optionsBuilder.Options);
        }
    }
}
