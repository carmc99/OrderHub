using MediatR;
using OrderHub.Core.Customer.Models;

namespace OrderHub.Core.Customer.Specifications
{
    public class SearchCustomersSpecification : IRequest<List<CustomerModel>>{ }
}
