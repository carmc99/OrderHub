using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Repositories.EF
{
    internal class CustomerMapper : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(p => p.Email);
            builder.Property(p => p.Address);
            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(16);
        }
    }
}
