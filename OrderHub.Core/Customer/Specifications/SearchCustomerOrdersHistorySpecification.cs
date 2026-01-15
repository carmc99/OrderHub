using MediatR;
using OrderHub.Core.Customer.Models;

namespace OrderHub.Core.Customer.Specifications
{
    public class SearchCustomerOrdersHistorySpecification : IRequest<CustomerHistoryModel?>
    {
        public int CustomerId { get; set; }
    }
}
