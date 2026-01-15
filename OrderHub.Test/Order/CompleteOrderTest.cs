using Microsoft.Extensions.Logging;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;

namespace OrderHub.Test.Order
{
    public class CompleteOrderTest
    {
        [Fact]
        public async Task GivenPendingOrder_WhenCompletingOrder_ThenShouldCompleteSuccessfully()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CompleteOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCompletedOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Pending);
                });

            CompleteOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenCompleteOrder(request);

            // Then
            result.ThenShouldCompleteOrderSuccessfully();
        }

        [Fact]
        public async Task GivenCancelledOrder_WhenCompletingOrder_ThenShouldThrowInvalidOperationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CompleteOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCompletedOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Cancelled);
                });

            CompleteOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            Task task = serviceProvider.WhenCompleteOrder(request);

            // Then
            await task.ThenShouldThrowInvalidOperationExceptionWithMessage("Cant completed cancelled order");
        }

        [Fact]
        public async Task GivenOrderIdZero_WhenCompletingOrder_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CompleteOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCompletedOrder();
                    services.GivenMediatorWithoutSetup();
                });

            CompleteOrderCommand.Request request = new()
            {
                Id = 0
            };

            // When
            Task task = serviceProvider.WhenCompleteOrder(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenNonExistentOrder_WhenCompletingOrder_ThenShouldReturnNull()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CompleteOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCompletedOrder();
                    services.GivenOrderDoesNotExist();
                });

            CompleteOrderCommand.Request request = new()
            {
                Id = 999
            };

            // When
            OrderModel? result = await serviceProvider.WhenCompleteOrder(request);

            // Then
            result.ThenShouldReturnNull();
        }

        [Fact]
        public async Task GivenPendingOrder_WhenCompletingOrder_ThenShouldRegisterCompletedDate()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CompleteOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCompletedOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Pending);
                });

            CompleteOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenCompleteOrder(request);

            // Then
            result.ThenShouldHaveCompletedDate();
        }
    }
}
