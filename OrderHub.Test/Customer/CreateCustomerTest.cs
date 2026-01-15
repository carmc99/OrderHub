using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Commands;
using OrderHub.Core.Customer.Models;

namespace OrderHub.Test.Customer
{
    public class CreateCustomerTest
    {
        [Fact]
        public async Task GivenValidCustomer_WhenCreatingCustomer_ThenCustomerIsCreated()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsSuccess();
                });

            CreateCustomerCommand.Request request = new()
            {
                Name = "Test Customer",
                Email = "test@example.com",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            CustomerModel? result = await serviceProvider.WhenCreateCustomer(request);

            // Then
            result.ThenShouldCompleteSuccessfully();
        }

        [Fact]
        public async Task GivenCustomerWithoutName_WhenCreatingCustomer_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsSuccess();
                });

            CreateCustomerCommand.Request request = new()
            {
                Name = string.Empty,
                Email = "test@example.com",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenCreateCustomer(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenCustomerWithInvalidEmail_WhenCreatingCustomer_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<CreateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsSuccess();
                });

            CreateCustomerCommand.Request request = new()
            {
                Name = string.Empty,
                Email = "invalid email",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenCreateCustomer(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }
    }
}
