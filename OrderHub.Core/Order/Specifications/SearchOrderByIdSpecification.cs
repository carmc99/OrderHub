using MediatR;
using OrderHub.Core.Order.Models;

namespace OrderHub.Core.Order.Specifications
{
    public class SearchOrderByIdSpecification : IRequest<OrderModel?>
    {
        public int Id { get; set; }
    }
}
