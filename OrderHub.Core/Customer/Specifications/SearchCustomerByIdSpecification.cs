using MediatR;
using OrderHub.Customer.Models;

namespace OrderHub.Core.Customer.Specifications
{
    public class SearchCustomerByIdSpecification : IRequest<CustomerModel?>
    {
        public int Id { get; set; }
    }
}
