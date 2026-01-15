using MediatR;
using OrderHub.Core.Order.Models;

namespace OrderHub.Core.Order.Specifications
{
    public class SearchOrdersSpecification : IRequest<List<OrderModel>>
    {
    }
}
