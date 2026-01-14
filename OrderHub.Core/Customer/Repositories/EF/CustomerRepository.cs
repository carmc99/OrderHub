
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Repositories.EF
{
    internal class CustomerRepository : ICustomerRepository
    {
        public Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerEntity?> Store(CustomerModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
