using Microsoft.Extensions.Logging;
using OrderHub.Core.Order.Commands;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHub.Test.Order
{
    public class CancelOrderTest
    {
        [Fact]
        public async Task GivenPendingOrder_WhenCancellingOrder_ThenShouldCancelSuccessfully()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CancelOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCancelledOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Pending);
                });

            CancelOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenCancelOrder(request);

            // Then
            result.ThenShouldCancelOrderSuccessfully();
        }

        [Fact]
        public async Task GivenCompletedOrder_WhenCancellingOrder_ThenShouldThrowInvalidOperationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CancelOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCancelledOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Completed);
                });

            CancelOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            Task task = serviceProvider.WhenCancelOrder(request);

            // Then
            await task.ThenShouldThrowInvalidOperationExceptionWithMessage("No se puede cancelar una orden completada");
        }

        [Fact]
        public async Task GivenOrderIdZero_WhenCancellingOrder_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CancelOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCancelledOrder();
                    services.GivenMediatorWithoutSetup();
                });

            CancelOrderCommand.Request request = new()
            {
                Id = 0
            };

            // When
            Task task = serviceProvider.WhenCancelOrder(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenNonExistentOrder_WhenCancellingOrder_ThenShouldReturnNull()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CancelOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCancelledOrder();
                    services.GivenOrderDoesNotExist();
                });

            CancelOrderCommand.Request request = new()
            {
                Id = 999
            };

            // When
            OrderModel? result = await serviceProvider.WhenCancelOrder(request);

            // Then
            result.ThenShouldReturnNull();
        }

        [Fact]
        public async Task GivenPendingOrder_WhenCancellingOrder_ThenShouldRegisterCancelledDate()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CancelOrderCommand.Handler>>();
                    services.GivenOrderRepositoryReturnsCancelledOrder();
                    services.GivenOrderExistsWithStatus(1, OrderStatus.Pending);
                });

            CancelOrderCommand.Request request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenCancelOrder(request);

            // Then
            result.ThenShouldHaveCancelledDate();
        }
    }
}
