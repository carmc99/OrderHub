using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Core.Order.Repositories.EF
{
    internal class OrderMapper : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CustomerId)
                .IsRequired();

            builder.Property(p => p.OrderDate)
                .IsRequired();

            builder.Property(p => p.Total)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.CompletedDate);

            builder.Property(p => p.CancelledDate);
        }
    }
}
