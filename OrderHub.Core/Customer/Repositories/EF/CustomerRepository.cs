using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Repositories.EF
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext Context;

        public CustomerRepository(CustomerDbContext context)
        {
            Context = context;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = false;

            CustomerEntity? entity = await Context.Customers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity != null)
            {
                Context.Customers.Remove(entity);

                int affectedRows = await Context.SaveChangesAsync(cancellationToken);
                result = affectedRows > 0;
            }

            return result;
        }

        public async Task<CustomerEntity?> Store(CustomerModel model, CancellationToken cancellationToken)
        {
            CustomerEntity? result = null;

            if (!string.IsNullOrEmpty(model.Email))
            {
                bool emailExists = await Context.Customers
                    .AnyAsync(x => x.Email == model.Email && x.Id != model.Id, cancellationToken);

                if (emailExists)
                {
                    throw new InvalidOperationException("A customer with this email already exists");
                }
            }

            CustomerEntity? currentEntity = await Context.Customers
                .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

            bool isNewEntity = currentEntity == null;

            if (isNewEntity)
            {
                CustomerEntity newEntity = MapToEntity(model);
                Context.Customers.Add(newEntity);
                await Context.SaveChangesAsync(cancellationToken);
                result = newEntity;
            }
            else
            {
                MapToExistingEntity(currentEntity, model);
                await Context.SaveChangesAsync(cancellationToken);
                result = currentEntity;
            }

            return result;
        }

        private static CustomerEntity MapToEntity(CustomerModel model)
        {
            CustomerEntity entity = new()
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            return entity;
        }

        private static void MapToExistingEntity(CustomerEntity entity, CustomerModel model)
        {
            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Address = model.Address;
        }
    }
}