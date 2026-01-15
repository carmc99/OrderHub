
using Microsoft.EntityFrameworkCore;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Repositories.EF
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

            CustomerEntity? entity = await Context.Set<CustomerEntity>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (entity != null)
            {
                Context.Set<CustomerEntity>().Remove(entity);
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
                bool emailExists = await Context.Set<CustomerEntity>()
                    .AnyAsync(x => x.Email == model.Email && x.Id != model.Id, cancellationToken);

                if (emailExists)
                {
                    throw new InvalidOperationException("A customer with this email already exists");
                }
            }

            if (model.Id == 0)
            {
                CustomerEntity newEntity = CustomerEntity.FromModel(model);
                Context.Set<CustomerEntity>().Add(newEntity);
                await Context.SaveChangesAsync(cancellationToken);

                result = newEntity;
            }
            else
            {
                CustomerEntity? existingEntity = await Context.Set<CustomerEntity>()
                    .FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);

                if (existingEntity != null)
                {
                    existingEntity.Name = model.Name;
                    existingEntity.Email = model.Email;
                    existingEntity.PhoneNumber = model.PhoneNumber;
                    existingEntity.Address = model.Address;

                    Context.Set<CustomerEntity>().Update(existingEntity);
                    await Context.SaveChangesAsync(cancellationToken);
                    result = existingEntity;
                }
            }

            return result;
        }
    }
}
