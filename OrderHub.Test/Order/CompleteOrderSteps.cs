using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Order.Specifications;

namespace OrderHub.Test.Order
{
    public static class CompleteOrderSteps
    {
        public static IServiceCollection GivenOrderRepositoryReturnsCompletedOrder(
            this IServiceCollection services)
        {
            Mock<IOrderRepository> repositoryMock = services.MockClass<IOrderRepository>();

            repositoryMock
                .Setup(x => x.Store(It.IsAny<OrderModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderModel model, CancellationToken ct) => new OrderEntity
                {
                    Id = model.Id,
                    CustomerId = model.CustomerId,
                    OrderDate = model.OrderDate,
                    Total = model.Total,
                    Status = model.Status,
                    CompletedDate = model.CompletedDate,
                    CancelledDate = model.CancelledDate
                });

            return services;
        }

        public static IServiceCollection GivenOrderExistsWithStatus(
            this IServiceCollection services,
            int orderId,
            OrderStatus status)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            OrderModel existingOrder = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                Total = 1500.00,
                Status = status
            };

            if (status == OrderStatus.Completed)
            {
                existingOrder.CompletedDate = DateTime.UtcNow.AddHours(-1);
            }
            else if (status == OrderStatus.Cancelled)
            {
                existingOrder.CancelledDate = DateTime.UtcNow.AddHours(-1);
            }

            mediatorMock
                .Setup(x => x.Send(
                    It.Is<SearchOrderByIdSpecification>(s => s.Id == orderId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOrder);

            return services;
        }

        public static IServiceCollection GivenOrderDoesNotExist(
            this IServiceCollection services)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<SearchOrderByIdSpecification>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderModel?)null);

            return services;
        }

        public static Task<OrderModel?> WhenCompleteOrder(
            this IServiceProvider services,
            CompleteOrderCommand.Request request)
        {
            IRequestHandler<CompleteOrderCommand.Request, OrderModel?> handler = services
                .GetRequiredService<IRequestHandler<CompleteOrderCommand.Request, OrderModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldCompleteOrderSuccessfully(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Completed, result!.Status);
        }

        public static async Task ThenShouldThrowInvalidOperationExceptionWithMessage(
            this Task task,
            string expectedMessage)
        {
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(() => task);
            Assert.Equal(expectedMessage, exception.Message);
        }

        public static void ThenShouldHaveCompletedDate(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.NotNull(result!.CompletedDate);
            Assert.True(result.CompletedDate <= DateTime.UtcNow);
        }
    }
}
