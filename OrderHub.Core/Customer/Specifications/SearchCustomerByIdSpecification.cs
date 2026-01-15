using MediatR;
using OrderHub.Core.Customer.Models;

namespace OrderHub.Core.Customer.Specifications
{
    public class SearchCustomerByIdSpecification : IRequest<CustomerModel?>
    {
        public int Id { get; set; }
    }
}
