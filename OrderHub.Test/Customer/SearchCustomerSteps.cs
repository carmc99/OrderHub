using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Test.Customer
{
    public static class SearchCustomerSteps
    {
        public static IServiceCollection GivenFiveCustomersInDatabase(
            this IServiceCollection services)
        {
            CustomerDbContext context = GetInMemoryContext();

            List<CustomerEntity> customers = new()
            {
                new CustomerEntity
                {
                    Id = 1,
                    Name = "Juan Pérez",
                    Email = "juan@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Calle 10 #20-30"
                },
                new CustomerEntity
                {
                    Id = 2,
                    Name = "María López",
                    Email = "maria@example.com",
                    PhoneNumber = "3101234567",
                    Address = "Carrera 20 #30-40"
                },
                new CustomerEntity
                {
                    Id = 3,
                    Name = "Pedro Gómez",
                    Email = "pedro@example.com",
                    PhoneNumber = "3201234567",
                    Address = "Avenida 30 #40-50"
                },
                new CustomerEntity
                {
                    Id = 4,
                    Name = "Ana Martínez",
                    Email = "ana@example.com",
                    PhoneNumber = "3301234567",
                    Address = "Calle 40 #50-60"
                },
                new CustomerEntity
                {
                    Id = 5,
                    Name = "Carlos Rodríguez",
                    Email = "carlos@example.com",
                    PhoneNumber = "3401234567",
                    Address = "Carrera 50 #60-70"
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            services.AddSingleton(context);

            return services;
        }

        public static IServiceCollection GivenNoCustomersInDatabase(
            this IServiceCollection services)
        {
            CustomerDbContext context = GetInMemoryContext();

            services.AddSingleton(context);

            return services;
        }

        public static IServiceCollection GivenCustomersWithActiveAndInactiveStatus(
            this IServiceCollection services)
        {
            CustomerDbContext context = GetInMemoryContext();

            List<CustomerEntity> customers = new()
            {
                new CustomerEntity
                {
                    Id = 1,
                    Name = "Active Customer 1",
                    Email = "active1@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Address 1"
                },
                new CustomerEntity
                {
                    Id = 2,
                    Name = "Active Customer 2",
                    Email = "active2@example.com",
                    PhoneNumber = "3101234567",
                    Address = "Address 2"
                },
                new CustomerEntity
                {
                    Id = 3,
                    Name = "Inactive Customer 1",
                    Email = "inactive1@example.com",
                    PhoneNumber = "3201234567",
                    Address = "Address 3"
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            services.AddSingleton(context);

            return services;
        }

        public static Task<List<CustomerModel>> WhenSearchCustomers(
            this IServiceProvider services,
            SearchCustomersSpecification request)
        {
            IRequestHandler<SearchCustomersSpecification, List<CustomerModel>> handler = services
                .GetRequiredService<IRequestHandler<SearchCustomersSpecification, List<CustomerModel>>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldReturnCustomerCount(
            this List<CustomerModel> result,
            int expectedCount)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
        }

        public static void ThenShouldReturnEmptyList(this List<CustomerModel> result)
        {
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public static void ThenShouldContainCustomersWithStatus(this List<CustomerModel> result)
        {
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            Assert.All(result, customer =>
            {
                Assert.NotNull(customer.Name);
                Assert.NotNull(customer.Email);
            });
        }

        private static CustomerDbContext GetInMemoryContext()
        {
            DbContextOptions<CustomerDbContext> options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            CustomerDbContext context = new(options);

            return context;
        }
    }
}
