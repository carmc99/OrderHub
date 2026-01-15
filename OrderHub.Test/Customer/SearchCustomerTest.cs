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

        [Fact]
        public async Task GivenCustomersWithDifferentStatus_WhenSearchingCustomers_ThenShouldReturnAllCustomersWithStatus()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomersQuery.Handler>>();
                    services.GivenCustomersWithActiveAndInactiveStatus();
                });

            SearchCustomersSpecification request = new();

            // When
            List<CustomerModel> result = await serviceProvider.WhenSearchCustomers(request);

            // Then
            result.ThenShouldContainCustomersWithStatus();
        }
    }
}
