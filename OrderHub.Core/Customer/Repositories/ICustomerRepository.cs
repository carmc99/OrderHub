using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> Delete(int id, CancellationToken cancellationToken);

        Task<CustomerEntity?> Store(CustomerModel model, CancellationToken cancellationToken);
    }
}
