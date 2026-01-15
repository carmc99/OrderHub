using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Models;
using OrderHub.Core.Customer.Repositories.EF.Queries;
using OrderHub.Core.Customer.Specifications;

namespace OrderHub.Test.Customer
{
    public class SearchCustomerOrdersHistoryTest
    {
        [Fact]
        public async Task GivenCustomerWithFiveOrders_WhenGettingHistory_ThenShouldReturnFiveOrdersOrderedByDate()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithFiveOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 1
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldReturnFiveOrdersOrderedByDate();
        }

        [Fact]
        public async Task GivenCustomerWithCompletedOrders_WhenGettingHistory_ThenShouldShowTotalCompletedAmount()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithFiveOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 1
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldShowTotalCompletedAmount();
        }

        [Fact]
        public async Task GivenCustomerWithoutOrders_WhenGettingHistory_ThenShouldShowZeroOrdersWithCustomerInformation()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithoutOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 2
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldShowZeroOrdersWithCustomerInformation();
        }

        [Fact]
        public async Task GivenCustomerWithoutOrders_WhenGettingHistory_ThenShouldShowCustomerBasicInformation()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithoutOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 2
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldShowCustomerBasicInformation();
        }

        [Fact]
        public async Task GivenCustomerWithOrders_WhenGettingHistory_ThenShouldIncludeAllOrdersInformation()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithCompletedOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 3
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldIncludeAllOrdersInformation();
        }

        [Fact]
        public async Task GivenCustomerWithMixedOrders_WhenGettingHistory_ThenShouldCalculateCorrectTotalCompletedAmount()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenCustomerWithCompletedOrdersInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 3
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldCalculateCorrectTotalCompletedAmount();
        }

        [Fact]
        public async Task GivenNonExistentCustomer_WhenGettingHistory_ThenShouldReturnNull()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchCustomerOrdersHistoryQuery.Handler>>();
                    services.GivenNonExistentCustomerInDatabase();
                });

            SearchCustomerOrdersHistorySpecification request = new()
            {
                CustomerId = 999
            };

            // When
            CustomerHistoryModel? result = await serviceProvider.WhenGetCustomerHistory(request);

            // Then
            result.ThenShouldReturnNullForNonExistentCustomer();
        }
    }
}
