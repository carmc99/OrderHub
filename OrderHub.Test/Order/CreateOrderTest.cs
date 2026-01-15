using Microsoft.Extensions.Logging;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Test.Order
{
    public class CreateOrderTest
    {
        [Fact]
        public async Task GivenValidOrder_WhenCreatingOrder_ThenOrderIsCreated()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsSuccess();
                    services.GivenCustomerExists(5);
                });

            CreateOrderCommand.Request request = new()
            {
                CustomerId = 5,
                Total = 1500.00,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // When
            OrderModel? result = await serviceProvider.WhenCreateOrder(request);

            // Then
            result.ThenShouldCompleteSuccessfully();
        }

        [Fact]
        public async Task GivenOrderWithoutCustomer_WhenCreatingOrder_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsSuccess();
                    services.GivenMediatorWithoutSetup();
                });

            CreateOrderCommand.Request request = new()
            {
                CustomerId = 0,
                Total = 1500.00,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // When
            Task task = serviceProvider.WhenCreateOrder(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenOrderWithoutProducts_WhenCreatingOrder_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsSuccess();
                    services.GivenCustomerExists(5);
                });

            CreateOrderCommand.Request request = new()
            {
                CustomerId = 5,
                Total = 0.00,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // When
            Task task = serviceProvider.WhenCreateOrder(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenOrderWithNegativeTotal_WhenCreatingOrder_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsSuccess();
                    services.GivenCustomerExists(5);
                });

            CreateOrderCommand.Request request = new()
            {
                CustomerId = 5,
                Total = -100.00,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // When
            Task task = serviceProvider.WhenCreateOrder(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenNonExistentCustomer_WhenCreatingOrder_ThenShouldThrowCustomerNotFoundException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsSuccess();
                    services.GivenCustomerDoesNotExist();
                });

            CreateOrderCommand.Request request = new()
            {
                CustomerId = 999,
                Total = 1500.00,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            // When
            Task task = serviceProvider.WhenCreateOrder(request);

            // Then
            await task.ThenShouldThrowCustomerNotFoundException();
        }
    }
}
