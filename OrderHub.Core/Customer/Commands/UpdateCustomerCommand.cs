using FluentValidation;
using MediatR;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Core.Customer.Commands
{
    public static class UpdateCustomerCommand
    {
        public class Handler : IRequestHandler<Request, CustomerModel?>
        {
            private readonly ICustomerRepository CustomerRepository;
            private readonly IValidator<Request> RequestValidation;
            private readonly IMediator Mediator;

            public Handler(
                ICustomerRepository customerRepository,
                IValidator<Request> requestValidation,
                IMediator mediator)
            {
                CustomerRepository = customerRepository;
                RequestValidation = requestValidation;
                Mediator = mediator;
            }

            public async Task<CustomerModel?> Handle(Request request, CancellationToken cancellationToken)
            {
                CustomerModel? result = null;

                await RequestValidation.ValidateAndThrowAsync(request, cancellationToken);

                SearchCustomerByIdSpecification searchSpec = new()
                {
                    Id = request.Id
                };

                CustomerModel? existingCustomer = await Mediator.Send(searchSpec, cancellationToken);

                if(existingCustomer != null)
                {
                    CustomerEntity? entity = await CustomerRepository.Store(request, cancellationToken);
                    
                    if (entity != null)
                    {
                        result = CustomerModel.FromEntity(entity);
                    }
                }

                return result;
            }
        }
        public class Request : CustomerModel, IRequest<CustomerModel?> { }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Customer Id must be greater than 0");

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name is required");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("Email is required")
                    .EmailAddress()
                    .WithMessage("Email format is invalid");
            }
        }
    }
}

