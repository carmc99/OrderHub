using Microsoft.Extensions.Logging;
using OrderHub.Core.Customer.Commands;
using OrderHub.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHub.Test.Customer
{
    public class UpdateCustomerTest
    {
        [Fact]
        public async Task Given_ValidCustomerData_When_UpdatingCustomer_Then_ShouldUpdateSuccessfully()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer();
                });

            UpdateCustomerCommand.Request request = new()
            {
                Id = 1,
                Name = "Updated Customer",
                Email = "updated@example.com",
                PhoneNumber = "3107654321",
                Address = "Updated Address"
            };

            // When
            CustomerModel? result = await serviceProvider.WhenUpdateCustomer(request);

            // Then
            result.ThenShouldUpdateSuccessfully();
        }

        [Fact]
        public async Task Given_DuplicateEmail_When_UpdatingCustomer_Then_ShouldThrowInvalidOperationException()
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
        public async Task Given_CustomerIdZero_When_UpdatingCustomer_Then_ShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer();
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
        public async Task Given_EmptyName_When_UpdatingCustomer_Then_ShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer();
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
        public async Task Given_EmptyEmail_When_UpdatingCustomer_Then_ShouldThrowValidationException()
        {
            // Given
            TestApplicationBuilder builder = new();

            IServiceProvider serviceProvider = builder
                .Setup(services =>
                {
                    services.MockClass<ILogger<UpdateCustomerCommand.Handler>>();
                    services.GivenCustomerRepositoryReturnsUpdatedCustomer();
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
