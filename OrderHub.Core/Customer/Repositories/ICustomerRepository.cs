using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> Delete(int id, CancellationToken cancellationToken);

        Task<CustomerEntity?> Store(CustomerModel model, CancellationToken cancellationToken);
    }
}
