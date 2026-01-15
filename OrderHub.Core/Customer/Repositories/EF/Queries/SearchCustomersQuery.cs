using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core.Customer.Repositories.EF.Queries
{
    public static class SearchCustomersQuery
    {
        public class Handler : IRequestHandler<SearchCustomersSpecification, List<CustomerModel>>
        {
            private readonly ReadDbContext Context;

            public Handler(ReadDbContext context)
            {
                Context = context;
            }

            public async Task<List<CustomerModel>> Handle(SearchCustomersSpecification request, CancellationToken cancellationToken)
            {
                List<CustomerModel> result = new();

                List<CustomerEntity> entities = await Context.Customers
                    .ToListAsync(cancellationToken);

                if (entities != null && entities.Count > 0)
                {
                    result = entities
                        .Select(entity => CustomerModel.FromEntity(entity))
                        .ToList();
                }

                return result;
            }
        }
    }
}