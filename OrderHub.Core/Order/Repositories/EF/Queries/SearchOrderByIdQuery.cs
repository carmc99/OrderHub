using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Order.Specifications;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core.Order.Repositories.EF.Queries
{
    public static class SearchOrderByIdQuery
    {
        public class Handler : IRequestHandler<SearchOrderByIdSpecification, OrderModel?>
        {
            private readonly ReadDbContext Context;

            public Handler(ReadDbContext context)
            {
                Context = context;
            }

            public async Task<OrderModel?> Handle(SearchOrderByIdSpecification request, CancellationToken cancellationToken)
            {
                OrderModel? result = null;

                OrderEntity? entity = await Context.Set<OrderEntity>()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity != null)
                {
                    result = OrderModel.FromEntity(entity);
                }

                return result;
            }
        }
    }
}
