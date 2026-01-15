using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Core.Order.Repositories.EF
{
    public class OrderDbContext : DbContext
    {
        public DbSet<OrderEntity> Orders { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderMapper());
        }
    }
}
