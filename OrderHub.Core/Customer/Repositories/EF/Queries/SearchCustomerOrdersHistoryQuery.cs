using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core.Customer.Repositories.EF.Queries
{
    public static class SearchCustomerOrdersHistoryQuery
    {
        public class Handler : IRequestHandler<SearchCustomerOrdersHistorySpecification, CustomerHistoryModel?>
        {
            private readonly ReadDbContext Context;

            public Handler(ReadDbContext context)
            {
                Context = context;
            }

            public async Task<CustomerHistoryModel?> Handle(SearchCustomerOrdersHistorySpecification request, CancellationToken cancellationToken)
            {
                CustomerHistoryModel? result = null;

                CustomerEntity? customer = await Context.Customers
                    .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

                if (customer != null)
                {
                    List<OrderEntity> orders = await Context.Orders
                        .Where(x => x.CustomerId == request.CustomerId)
                        .OrderByDescending(x => x.OrderDate)
                        .ToListAsync(cancellationToken);

                    double totalCompletedAmount = orders
                        .Where(x => x.Status == OrderStatus.Completed)
                        .Sum(x => x.Total);

                    result = new CustomerHistoryModel
                    {
                        CustomerId = customer.Id,
                        CustomerName = customer.Name,
                        CustomerEmail = customer.Email,
                        CustomerPhoneNumber = customer.PhoneNumber,
                        CustomerAddress = customer.Address,
                        TotalOrders = orders.Count,
                        TotalCompletedAmount = totalCompletedAmount,
                        Orders = orders.Select(o => new OrderHistoryItemModel
                        {
                            OrderId = o.Id,
                            OrderDate = o.OrderDate,
                            Total = o.Total,
                            Status = o.Status.ToString(),
                            CompletedDate = o.CompletedDate,
                            CancelledDate = o.CancelledDate
                        }).ToList()
                    };
                }

                return result;
            }
        }
    }
}
