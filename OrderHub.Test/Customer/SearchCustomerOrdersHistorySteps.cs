using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Entities;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Entities;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Test.Customer
{
    internal static class SearchCustomerOrdersHistorySteps
    {
        public static IServiceCollection GivenCustomerWithFiveOrdersInDatabase(
            this IServiceCollection services)
        {
            ReadDbContext context = services.GetInMemoryContext();

            List<CustomerEntity> customers = new()
            {
                new CustomerEntity
                {
                    Id = 1,
                    Name = "María López",
                    Email = "maria.lopez@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Calle 10 #20-30"
                }
            };

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
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-7)
                },
                new OrderEntity
                {
                    Id = 3,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-5),
                    Total = 1750.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-4)
                },
                new OrderEntity
                {
                    Id = 4,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    Total = 1000.00,
                    Status = OrderStatus.Pending
                },
                new OrderEntity
                {
                    Id = 5,
                    CustomerId = 1,
                    OrderDate = DateTime.UtcNow.AddDays(-6),
                    Total = 2500.00,
                    Status = OrderStatus.Cancelled,
                    CancelledDate = DateTime.UtcNow.AddDays(-5)
                }
            };

            context.LoadData(customers);
            context.LoadData(orders);

            return services;
        }

        public static IServiceCollection GivenCustomerWithoutOrdersInDatabase(
            this IServiceCollection services)
        {
            ReadDbContext context = services.GetInMemoryContext();

            List<CustomerEntity> customers = new()
            {
                new CustomerEntity
                {
                    Id = 2,
                    Name = "Pedro Gómez",
                    Email = "pedro.gomez@example.com",
                    PhoneNumber = "3107654321",
                    Address = "Carrera 20 #30-40"
                }
            };

            context.LoadData(customers);

            return services;
        }

        public static IServiceCollection GivenCustomerWithCompletedOrdersInDatabase(
            this IServiceCollection services)
        {
            ReadDbContext context = services.GetInMemoryContext();

            List<CustomerEntity> customers = new()
            {
                new CustomerEntity
                {
                    Id = 3,
                    Name = "Ana Martínez",
                    Email = "ana.martinez@example.com",
                    PhoneNumber = "3201234567",
                    Address = "Avenida 30 #40-50"
                }
            };

            List<OrderEntity> orders = new()
            {
                new OrderEntity
                {
                    Id = 10,
                    CustomerId = 3,
                    OrderDate = DateTime.UtcNow.AddDays(-30),
                    Total = 1200.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-29)
                },
                new OrderEntity
                {
                    Id = 11,
                    CustomerId = 3,
                    OrderDate = DateTime.UtcNow.AddDays(-20),
                    Total = 3500.00,
                    Status = OrderStatus.Completed,
                    CompletedDate = DateTime.UtcNow.AddDays(-19)
                },
                new OrderEntity
                {
                    Id = 12,
                    CustomerId = 3,
                    OrderDate = DateTime.UtcNow.AddDays(-1),
                    Total = 800.00,
                    Status = OrderStatus.Pending
                }
            };

            context.LoadData(customers);
            context.LoadData(orders);

            return services;
        }

        public static IServiceCollection GivenNonExistentCustomerInDatabase(
            this IServiceCollection services)
        {
            services.GetInMemoryContext();

            return services;
        }

        public static Task<CustomerHistoryModel?> WhenGetCustomerHistory(
            this IServiceProvider services,
            SearchCustomerOrdersHistorySpecification request)
        {
            IRequestHandler<SearchCustomerOrdersHistorySpecification, CustomerHistoryModel?> handler = services
                .GetRequiredService<IRequestHandler<SearchCustomerOrdersHistorySpecification, CustomerHistoryModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldReturnFiveOrdersOrderedByDate(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);
            Assert.Equal(5, result!.TotalOrders);
            Assert.Equal(5, result.Orders.Count);

            for (int i = 0; i < result.Orders.Count - 1; i++)
            {
                Assert.True(result.Orders[i].OrderDate >= result.Orders[i + 1].OrderDate);
            }
        }

        public static void ThenShouldShowTotalCompletedAmount(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);
            Assert.Equal(5250.00, result!.TotalCompletedAmount);
        }

        public static void ThenShouldReturnNullForNonExistentCustomer(
            this CustomerHistoryModel? result)
        {
            Assert.Null(result);
        }

        public static void ThenShouldShowZeroOrdersWithCustomerInformation(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);
            Assert.Equal(0, result!.TotalOrders);
            Assert.Empty(result.Orders);
            Assert.Equal("Pedro Gómez", result.CustomerName);
            Assert.Equal("pedro.gomez@example.com", result.CustomerEmail);
        }

        public static void ThenShouldShowCustomerBasicInformation(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.CustomerId > 0);
            Assert.False(string.IsNullOrEmpty(result.CustomerName));
            Assert.False(string.IsNullOrEmpty(result.CustomerEmail));
        }

        public static void ThenShouldIncludeAllOrdersInformation(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.TotalOrders > 0);
            Assert.NotEmpty(result.Orders);

            foreach (OrderHistoryItemModel order in result.Orders)
            {
                Assert.True(order.OrderId > 0);
                Assert.True(order.Total >= 0);
                Assert.NotEqual(default, order.OrderDate);
                Assert.False(string.IsNullOrEmpty(order.Status));
            }
        }

        public static void ThenShouldCalculateCorrectTotalCompletedAmount(
            this CustomerHistoryModel? result)
        {
            Assert.NotNull(result);

            double expectedTotal = result!.Orders
                .Where(x => x.Status == "Completed")
                .Sum(x => x.Total);

            Assert.Equal(expectedTotal, result.TotalCompletedAmount);
        }
    }
}
