using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Commands;

namespace OrderHub.Test.Customer
{
    public class DeleteCustomerTest
    {
        [Fact]
        public async Task GivenExistingCustomer_WhenDeletingCustomer_ThenShouldDeleteSuccessfully()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<DeleteCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryDeleteReturns(true);
                });

            DeleteCustomerCommand.Request request = new()
            {
                Id = 1
            };

            // When
            bool result = await serviceProvider.WhenDeleteCustomer(request);

            // Then
            result.ThenShouldDeleteSuccessfully();
        }

        [Fact]
        public async Task GivenNonExistingCustomer_WhenDeletingCustomer_ThenShouldReturnFalse()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<DeleteCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryDeleteReturns(false);
                });

            DeleteCustomerCommand.Request request = new()
            {
                Id = 999
            };

            // When
            bool result = await serviceProvider.WhenDeleteCustomer(request);

            // Then
            result.ThenShouldNotDelete();
        }

        [Fact]
        public async Task GivenDatabaseError_WhenDeletingCustomer_ThenShouldThrowException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<DeleteCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryDeleteThrowsException();
                });

            DeleteCustomerCommand.Request request = new()
            {
                Id = 1
            };

            // When
            Task task = serviceProvider.WhenDeleteCustomer(request);

            // Then
            await task.ThenShouldThrowException<Exception>();
        }
    }
}
