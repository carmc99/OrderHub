using FluentValidation;
using MediatR;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Customer.Models;

namespace OrderHub.Core.Order.Commands
{
    public static class CreateOrderCommand
    {
        public class Handler : IRequestHandler<Request, OrderModel?>
        {
            private readonly IOrderRepository OrderRepository;
            private readonly IValidator<Request> RequestValidation;
            private readonly IMediator Mediator;

            public Handler(
                IOrderRepository orderRepository,
                IValidator<Request> requestValidation,
                IMediator mediator)
            {
                OrderRepository = orderRepository;
                RequestValidation = requestValidation;
                Mediator = mediator;
            }

            public async Task<OrderModel?> Handle(Request request, CancellationToken cancellationToken)
            {
                OrderModel? result = null;

                await RequestValidation.ValidateAndThrowAsync(request, cancellationToken);

                _ = await Mediator.Send(new SearchCustomerByIdSpecification { 
                        Id = request.CustomerId 
                    }, cancellationToken) ??
                        throw new InvalidOperationException("Customer not found");

                OrderEntity? entity = await OrderRepository.Store(request, cancellationToken);

                if (entity != null)
                {
                    result = OrderModel.FromEntity(entity);
                }

                return result;
            }
        }

        public class Request : OrderModel, IRequest<OrderModel?> { }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.CustomerId)
                    .GreaterThan(0)
                    .WithMessage("Customer is required");

                RuleFor(x => x.Total)
                    .GreaterThan(0)
                    .WithMessage("Order must have at least one product");

                RuleFor(x => x.Total)
                    .Must(total => total >= 0)
                    .WithMessage("Order total cannot be negative");
            }
        }
    }
}
