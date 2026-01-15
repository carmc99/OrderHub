using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Order.Specifications;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core.Order.Repositories.EF.Queries
{
    public static class SearchOrdersQuery
    {
        public class Handler : IRequestHandler<SearchOrdersSpecification, List<OrderModel>>
        {
            private readonly ReadDbContext Context;

            public Handler(ReadDbContext context)
            {
                Context = context;
            }

            public async Task<List<OrderModel>> Handle(SearchOrdersSpecification request, CancellationToken cancellationToken)
            {
                List<OrderModel> result = new();

                List<OrderEntity> entities = await Context.Orders
                    .ToListAsync(cancellationToken);

                if (entities != null && entities.Count > 0)
                {
                    result = entities
                        .Select(entity => OrderModel.FromEntity(entity))
                        .ToList();
                }

                return result;
            }
        }
    }
}