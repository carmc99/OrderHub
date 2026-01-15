using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Repositories.EF;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Repositories.EF
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }

        public CustomerDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerMapper());
        }
    }
}
