using Microsoft.Extensions.Logging;
using OrderHub.Core.Order.Models;
using OrderHub.Core.Order.Repositories.EF.Queries;
using OrderHub.Core.Order.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHub.Test.Order
{
    public class SearchOrderTest
    {
        [Fact]
        public async Task GivenTenOrdersInDatabase_WhenSearchingAllOrders_ThenShouldReturnTenOrders()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchOrdersQuery.Handler>>();
                    services.GivenTenOrdersInDatabase();
                });

            SearchOrdersSpecification request = new();

            // When
            List<OrderModel> result = await serviceProvider.WhenSearchOrders(request);

            // Then
            result.ThenShouldReturnOrderCount(10);
        }

        [Fact]
        public async Task GivenNoOrdersInDatabase_WhenSearchingById_ThenShouldReturnNull()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchOrderByIdQuery.Handler>>();
                    services.GivenNoOrdersInDatabase();
                });

            SearchOrderByIdSpecification request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenSearchOrderById(request);

            // Then
            result.ThenShouldReturnNull();
        }

        [Fact]
        public async Task GivenExistingOrderWithId_WhenSearchingById_ThenShouldReturnOrder()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchOrderByIdQuery.Handler>>();
                    services.GivenTenOrdersInDatabase();
                });

            SearchOrderByIdSpecification request = new()
            {
                Id = 5
            };

            // When
            OrderModel? result = await serviceProvider.WhenSearchOrderById(request);

            // Then
            result.ThenShouldReturnOrderWithId(5);
        }

        [Fact]
        public async Task GivenExistingOrder_WhenSearchingById_ThenShouldReturnCompleteInformation()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchOrderByIdQuery.Handler>>();
                    services.GivenTenOrdersInDatabase();
                });

            SearchOrderByIdSpecification request = new()
            {
                Id = 1
            };

            // When
            OrderModel? result = await serviceProvider.WhenSearchOrderById(request);

            // Then
            result.ThenShouldContainCompleteInformation();
        }

        [Fact]
        public async Task GivenNoOrdersInDatabase_WhenSearchingOrders_ThenShouldReturnEmptyList()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<SearchOrdersQuery.Handler>>();
                    services.GivenNoOrdersInDatabase();
                });

            SearchOrdersSpecification request = new();

            // When
            List<OrderModel> result = await serviceProvider.WhenSearchOrders(request);

            // Then
            result.ThenShouldReturnEmptyList();
        }
    }
}
