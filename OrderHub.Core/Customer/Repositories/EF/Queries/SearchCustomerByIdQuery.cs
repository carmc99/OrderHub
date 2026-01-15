using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core.Customer.Repositories.EF.Queries
{
    public static class SearchCustomerByIdQuery
    {
        public class Handler : IRequestHandler<SearchCustomerByIdSpecification, CustomerModel?>
        {
            private readonly ReadDbContext Context;

            public Handler(ReadDbContext context)
            {
                Context = context;
            }

            public async Task<CustomerModel?> Handle(SearchCustomerByIdSpecification request, CancellationToken cancellationToken)
            {
                CustomerModel? result = null;

                CustomerEntity? entity = await Context.Customers
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