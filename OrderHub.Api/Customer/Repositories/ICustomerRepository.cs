using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerEntity> Store(CustomerEntity customerEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}
