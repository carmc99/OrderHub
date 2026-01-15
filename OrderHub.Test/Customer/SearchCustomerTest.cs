using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Repositories.EF.Queries;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;

namespace OrderHub.Test.Customer
{
    public class SearchCustomerTest
    {
        [Fact]
        public async Task GivenFiveCustomersRegistered_WhenSearchingAllCustomers_ThenShouldReturnFiveCustomers()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomersQuery.Handler>>();
                    services.GivenFiveCustomersInDatabase();
                });

            SearchCustomersSpecification request = new();

            // When
            List<CustomerModel> result = await serviceProvider.WhenSearchCustomers(request);

            // Then
            result.ThenShouldReturnCustomerCount(5);
        }

        [Fact]
        public async Task GivenNoCustomersInDatabase_WhenSearchingById_ThenShouldReturnNull()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerByIdQuery.Handler>>();
                    services.GivenNoCustomersInDatabase();
                });

            SearchCustomerByIdSpecification request = new()
            {
                Id = 1
            };

            // When
            CustomerModel? result = await serviceProvider.WhenSearchCustomerById(request);

            // Then
            result.ThenShouldReturnNull();
        }

        [Fact]
        public async Task GivenExistingCustomerWithId_WhenSearchingById_ThenShouldReturnCustomer()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerByIdQuery.Handler>>();
                    services.GivenFiveCustomersInDatabase();
                });

            SearchCustomerByIdSpecification request = new()
            {
                Id = 3
            };

            // When
            CustomerModel? result = await serviceProvider.WhenSearchCustomerById(request);

            // Then
            result.ThenShouldReturnCustomerWithId(3);
        }

        [Fact]
        public async Task GivenExistingCustomer_WhenSearchingById_ThenShouldReturnCompleteInformation()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerByIdQuery.Handler>>();
                    services.GivenFiveCustomersInDatabase();
                });

            SearchCustomerByIdSpecification request = new()
            {
                Id = 1
            };

            // When
            CustomerModel? result = await serviceProvider.WhenSearchCustomerById(request);

            // Then
            result.ThenShouldContainCompleteInformation();
        }

        [Fact]
        public async Task GivenNoCustomersRegistered_WhenSearchingCustomers_ThenShouldReturnEmptyList()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomersQuery.Handler>>();
                    services.GivenNoCustomersInDatabase();
                });

            SearchCustomersSpecification request = new();

            // When
            List<CustomerModel> result = await serviceProvider.WhenSearchCustomers(request);

            // Then
            result.ThenShouldReturnEmptyList();
        }
    }
}
