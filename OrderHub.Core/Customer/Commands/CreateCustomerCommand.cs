using FluentValidation;

using MediatR;

using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories;

namespace OrderHub.Customer.Commands
{
    public static class CreateCustomerCommand
    {
        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly ICustomerRepository CustomerRepository;
            private readonly IValidator<Request> RequestValidation;

            public Handler(
                ICustomerRepository customerRepository,
                IValidator<Request> requestValidation)
            {
                CustomerRepository = customerRepository;
                RequestValidation = requestValidation;
            }

            public Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        public class Request : CustomerModel, IRequest<bool> { }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}
