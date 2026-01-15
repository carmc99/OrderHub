using Castle.Core.Resource;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Order.Specifications;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Test.Order
{
    internal static class SearchOrderSteps
    {
        public static IServiceCollection GivenTenOrdersInDatabase(
           this IServiceCollection services)
        {
            ReadDbContext context = services.GetInMemoryContext();

            List<OrderEntity> orders = new()
            {
                new OrderEntity
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-10),
                    Total = 1500.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-9)
                },
                new OrderEntity
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-8),
                    Total = 2000.00,
                    Status = OrderStatus.Pending
                },
                new OrderEntity
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.UtcNow.AddDays(-7),
                    Total = 1750.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-6)
                },
                new OrderEntity
                {
                    Id = 4,
                    CustomerId = 2,
                    OrderDate = DateTime.UtcNow.AddDays(-6),
                    Total = 3000.00,
                    Status = OrderStatus.Cancelled,
                    CancelledDate = DateTime.UtcNow.AddDays(-5)
                },
                new OrderEntity
                {
                    Id = 5,
                    CustomerId = 3,
                    OrderDate = DateTime.UtcNow.AddDays(-5),
                    Total = 1250.00,
                    Status = OrderStatus.Pending
                },
                new OrderEntity
                {
                    Id = 6,
                    CustomerId = 3,
                    OrderDate = DateTime.UtcNow.AddDays(-4),
                    Total = 4500.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-3)
                },
                new OrderEntity
                {
                    Id = 7,
                    CustomerId = 4,
                    OrderDate = DateTime.UtcNow.AddDays(-3),
                    Total = 800.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-2)
                },
                new OrderEntity
                {
                    Id = 8,
                    CustomerId = 4,
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    Total = 1900.00,
                    Status = OrderStatus.Pending
                },
                new OrderEntity
                {
                    Id = 9,
                    CustomerId = 5,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    Total = 2200.00,
                    Status = OrderStatus.Cancelled,
                    CancelledDate = DateTime.UtcNow
                },
                new OrderEntity
                {
                    Id = 10,
                    CustomerId = 5,
                    OrderDate = DateTime.UtcNow,
                    Total = 3500.00,
                    Status = OrderStatus.Pending
                }
            };

            context.LoadData(orders);

            return services;
        }

        public static IServiceCollection GivenNoOrdersInDatabase(
            this IServiceCollection services)
        {
            services.GetInMemoryContext();

            return services;
        }

        public static Task<List<OrderModel>> WhenSearchOrders(
            this IServiceProvider services,
            SearchOrdersSpecification request)
        {
            IRequestHandler<SearchOrdersSpecification, List<OrderModel>> handler = services
                .GetRequiredService<IRequestHandler<SearchOrdersSpecification, List<OrderModel>>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static Task<OrderModel?> WhenSearchOrderById(
            this IServiceProvider services,
            SearchOrderByIdSpecification request)
        {
            IRequestHandler<SearchOrderByIdSpecification, OrderModel?> handler = services
                .GetRequiredService<IRequestHandler<SearchOrderByIdSpecification, OrderModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldReturnOrderCount(
            this List<OrderModel> result,
            int expectedCount)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

        public static void ThenShouldReturnNull(this OrderModel? result)
        {
            Assert.Null(result);
        }

        public static void ThenShouldReturnOrderWithId(
            this OrderModel? result,
            int expectedId)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedId, result!.Id);
        }

        public static void ThenShouldContainCompleteInformation(this OrderModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.Id > 0);
            Assert.True(result.CustomerId > 0);
            Assert.True(result.Total > 0);
            Assert.NotEqual(default, result.OrderDate);
        }

        public static void ThenShouldReturnEmptyList(this List<OrderModel> result)
        {
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
