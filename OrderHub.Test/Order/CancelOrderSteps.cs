using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Test.Order
{
    public static class CancelOrderSteps
    {
        public static IServiceCollection GivenOrderRepositoryReturnsCancelledOrder(
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

        public static Task<OrderModel?> WhenCancelOrder(
            this IServiceProvider services,
            CancelOrderCommand.Request request)
        {
            IRequestHandler<CancelOrderCommand.Request, OrderModel?> handler = services
                .GetRequiredService<IRequestHandler<CancelOrderCommand.Request, OrderModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldCancelOrderSuccessfully(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.Equal(OrderStatus.Cancelled, result!.Status);
        }

        public static void ThenShouldHaveCancelledDate(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.NotNull(result!.CancelledDate);
            Assert.True(result.CancelledDate <= DateTime.UtcNow);
        }
    }
}
