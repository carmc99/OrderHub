using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Core.Repositories.EF;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories.EF;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Test.Customer
{
    internal static class SearchCustomerSteps
    {
        public static IServiceCollection GivenFiveCustomersInDatabase(
            this IServiceCollection services)
        {
            ReadDbContext context = services.GetInMemoryContext();

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

            context.LoadData(customers);

            return services;
        }

        public static IServiceCollection GivenNoCustomersInDatabase(
            this IServiceCollection services)
        {
            services.GetInMemoryContext();

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

        public static Task<CustomerModel?> WhenSearchCustomerById(
           this IServiceProvider services,
           SearchCustomerByIdSpecification request)
        {
            IRequestHandler<SearchCustomerByIdSpecification, CustomerModel?> handler = services
                .GetRequiredService<IRequestHandler<SearchCustomerByIdSpecification, CustomerModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldReturnNull(this CustomerModel? result)
        {
            Assert.Null(result);
        }

        public static void ThenShouldContainCompleteInformation(this CustomerModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.Id > 0);
            Assert.NotNull(result.Name);
            Assert.False(string.IsNullOrEmpty(result.Name));
            Assert.NotNull(result.Email);
            Assert.False(string.IsNullOrEmpty(result.Email));
        }

        public static void ThenShouldReturnCustomerWithId(
            this CustomerModel? result,
            int expectedId)
        {
            Assert.NotNull(result);
            Assert.Equal(expectedId, result!.Id);
        }

        public static void ThenShouldReturnEmptyList(this List<CustomerModel> result)
        {
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        private static ReadDbContext GetInMemoryContext()
        {
            DbContextOptions<ReadDbContext> options = new DbContextOptionsBuilder<ReadDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            ReadDbContext context = new(options);

            return context;
        }
    }
}
