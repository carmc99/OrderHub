using FluentValidation;
using MediatR;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Order.Specifications;

namespace OrderHub.Core.Order.Commands
{
    public static class CancelOrderCommand
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

                SearchOrderByIdSpecification searchSpec = new()
                {
                    Id = request.Id
                };

                OrderModel? existingOrder = await Mediator.Send(searchSpec, cancellationToken);

                if (existingOrder != null)
                {
                    if (existingOrder.Status == OrderStatus.Completed)
                    {
                        throw new InvalidOperationException("Completed order cannot be cancelled");
                    }

                    existingOrder.Status = OrderStatus.Cancelled;
                    existingOrder.CancelledDate = DateTime.UtcNow;

                    OrderEntity? entity = await OrderRepository.Store(existingOrder, cancellationToken);

                    if (entity != null)
                    {
                        result = OrderModel.FromEntity(entity);
                    }
                }

                return result;
            }
        }

        public class Request : IRequest<OrderModel?>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Order Id must be greater than 0");
            }
        }
    }
}