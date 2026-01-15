using FluentValidation;
using MediatR;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories;
using OrderHub.Core.Customer.Repositories.EF.Entities;
using OrderHub.Core.Customer.Specifications;
using System.Text.Json.Serialization;

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

                if (existingCustomer != null)
                {
                    existingCustomer.PhoneNumber = request.PhoneNumber;
                    existingCustomer.Address = request.Address;
                    existingCustomer.Name = request.Name;
                    existingCustomer.Email = request.Email;

                    CustomerEntity? entity = await CustomerRepository.Store(existingCustomer, cancellationToken);

                    if (entity != null)
                    {
                        result = CustomerModel.FromEntity(entity);
                    }
                }

                return result;
            }
        }

        public class Request : IRequest<CustomerModel?>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Address { get; set; }
        }

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