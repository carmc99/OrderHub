using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Commands;
using OrderHub.Core.Customer.Models;

namespace OrderHub.Test.Customer
{
    public class UpdateCustomerTest
    {
        [Fact]
        public async Task GivenValidCustomerData_WhenUpdatingCustomer_ThenShouldUpdateSuccessfully()
        {
            // Given
            TestApplicationBuilder builder = new();

            CustomerModel expectedResult = new()
            {
                Id = 1,
                Name = "Updated Customer",
                Email = "updated@example.com",
                PhoneNumber = "3107654321",
                Address = "Updated Address"
            };

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.GivenMediatorWithoutSetup();
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer(expectedResult);
                    services.GivenCustomerExists(expectedResult.Id);
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 1,
                Name = "Customer",
                Email = "test@example.com",
                PhoneNumber = "3107654321",
                Address = "test Address"
            };

            // When
            CustomerModel? result = await serviceProvider.WhenUpdateCustomer(request);

            // Then
            result.ThenShouldUpdateSuccessfully();
        }

        [Fact]
        public async Task GivenDuplicateEmail_WhenUpdatingCustomer_ThenShouldThrowInvalidOperationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerExists(1);
                    services.GivenCustomerRepositoryThrowsDuplicateEmailException();
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 1,
                Name = "Test Customer",
                Email = "duplicate@example.com",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenUpdateCustomer(request);

            // Then
            await task.ThenShouldThrowInvalidOperationException();
        }

        [Fact]
        public async Task GivenCustomerIdZero_WhenUpdatingCustomer_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            CustomerModel expectedResult = new()
            {
                Id = 1,
                Name = "Updated Customer",
                Email = "test@example.com",
                PhoneNumber = "3001234567",
                Address = "Updated Address"
            };

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.GivenMediatorWithoutSetup();
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer(expectedResult);
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 0,
                Name = "Test Customer",
                Email = "test@example.com",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenUpdateCustomer(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenEmptyName_WhenUpdatingCustomer_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            CustomerModel expectedResult = new()
            {
                Id = 1,
            };

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.GivenMediatorWithoutSetup();
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer(expectedResult);
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 1,
                Name = string.Empty,
                Email = "test@example.com",
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenUpdateCustomer(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }

        [Fact]
        public async Task GivenEmptyEmail_WhenUpdatingCustomer_ThenShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            CustomerModel expectedResult = new()
            {
                Id = 1,
            };

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.GivenMediatorWithoutSetup();
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer(expectedResult);
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 1,
                Name = "Test Customer",
                Email = string.Empty,
                PhoneNumber = "3001234567",
                Address = "Test Address"
            };

            // When
            Task task = serviceProvider.WhenUpdateCustomer(request);

            // Then
            await task.ThenShouldThrowValidationException();
        }
    }
}
