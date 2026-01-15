using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Repositories.EF.Queries
{
    public static class SearchCustomersQuery
    {
        public class Handler : IRequestHandler<SearchCustomersSpecification, List<CustomerModel>>
        {
            private readonly CustomerDbContext Context;

            public Handler(CustomerDbContext context)
            {
                Context = context;
            }

            public async Task<List<CustomerModel>> Handle(SearchCustomersSpecification request, CancellationToken cancellationToken)
            {
                List<CustomerModel> result = new();

                List<CustomerEntity> entities = await Context.Set<CustomerEntity>()
                    .AsNoTracking()
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
