using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Repositories.EF;
using OrderHub.Core.Order.Repositories.EF;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Repositories.EF
{
    public class ReadDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }

        public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerMapper());
            modelBuilder.ApplyConfiguration(new OrderMapper());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
