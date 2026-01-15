using MediatR;
using OrderHub.Customer.Repositories;

namespace OrderHub.Core.Customer.Commands
{
    public static class DeleteCustomerCommand
    {
        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly ICustomerRepository CustomerRepository;
            public Handler(ICustomerRepository customerRepository)
            {
                CustomerRepository = customerRepository;
            }
            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
               bool result = await CustomerRepository.Delete(request.Id, cancellationToken);

               return result;
            }
        }

        public class Request : IRequest<bool>
        {
            public int Id { get; set; }
        }
    }
}
