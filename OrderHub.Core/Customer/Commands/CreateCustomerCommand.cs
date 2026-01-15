using FluentValidation;
using MediatR;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Customer.Commands
{
    public static class CreateCustomerCommand
    {
        public class Handler : IRequestHandler<Request, CustomerModel?>
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

            public async Task<CustomerModel?> Handle(Request request, CancellationToken cancellationToken)
            {
                CustomerModel? result = null;

                await RequestValidation.ValidateAndThrowAsync(request, cancellationToken);

                CustomerModel customerModel = new()
                {
                    Address = request.Address,
                    Email = request.Email,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber
                };

                CustomerEntity? entity = await CustomerRepository.Store(customerModel, cancellationToken);

                if (entity != null)
                {
                    result = CustomerModel.FromEntity(entity);
                }

                return result;
            }
        }

        public class Request : IRequest<CustomerModel?> 
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
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
