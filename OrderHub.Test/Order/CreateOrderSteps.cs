using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Customer.Models;

namespace OrderHub.Test.Order
{
    public static class CreateOrderSteps
    {
        public static IServiceCollection GivenOrderRepositoryReturnsSuccess(
            this IServiceCollection services)
        {
            Mock<IOrderRepository> repositoryMock = services.MockClass<IOrderRepository>();

            repositoryMock
                .Setup(x => x.Store(It.IsAny<OrderModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((OrderModel model, CancellationToken ct) => new OrderEntity
                {
                    Id = 1,
                    CustomerId = model.CustomerId,
                    OrderDate = model.OrderDate,
                    Total = model.Total,
                    Status = model.Status,
                    CompletedDate = model.CompletedDate,
                    CancelledDate = model.CancelledDate
                });

            return services;
        }

        public static IServiceCollection GivenMediatorWithoutSetup(this IServiceCollection services)
        {
            services.MockClass<IMediator>();
            return services;
        }

        public static IServiceCollection GivenCustomerExists(
            this IServiceCollection services,
            int customerId)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            mediatorMock
                .Setup(x => x.Send(
                    It.Is<SearchCustomerByIdSpecification>(s => s.Id == customerId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CustomerModel
                {
                    Id = customerId,
                    Name = "Test Customer",
                    Email = "test@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Test Address"
                });

            return services;
        }

        public static IServiceCollection GivenCustomerDoesNotExist(
            this IServiceCollection services)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            mediatorMock
                .Setup(x => x.Send(
                    It.IsAny<SearchCustomerByIdSpecification>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerModel?)null);

            return services;
        }

        public static Task<OrderModel?> WhenCreateOrder(
            this IServiceProvider services,
            CreateOrderCommand.Request request)
        {
            IRequestHandler<CreateOrderCommand.Request, OrderModel?> handler = services
                .GetRequiredService<IRequestHandler<CreateOrderCommand.Request, OrderModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldCompleteSuccessfully(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.Id > 0);
            Assert.Equal(OrderStatus.Pending, result.Status);
        }

        public static Task ThenShouldThrowValidationException(this Task task)
        {
            return Assert.ThrowsAsync<ValidationException>(() => task);
        }

        public static Task ThenShouldThrowCustomerNotFoundException(this Task task)
        {
            return Assert.ThrowsAsync<InvalidOperationException>(() => task);
        }
    }
}
