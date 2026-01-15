using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Repositories.EF.Queries
{
    public static class SearchCustomerByIdQuery
    {
        public class Handler : IRequestHandler<SearchCustomerByIdSpecification, CustomerModel?>
        {
            private readonly CustomerDbContext Context;

            public Handler(CustomerDbContext context)
            {
                Context = context;
            }

            public async Task<CustomerModel?> Handle(SearchCustomerByIdSpecification request, CancellationToken cancellationToken)
            {
                CustomerModel? result = null;

                CustomerEntity? entity = await Context.Set<CustomerEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity != null)
                {
                    result = CustomerModel.FromEntity(entity);
                }

                return result;
            }
        }
    }
}
