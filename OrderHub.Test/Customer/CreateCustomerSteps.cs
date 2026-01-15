using FluentValidation;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using OrderHub.Customer.Commands;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories;
using OrderHub.Customer.Repositories.EF.Entities;

namespace OrderHub.Test.Customer
{
    public static class CreateCustomerSteps
    {
        public static IServiceCollection GivenCustomerRepositoryReturnsSuccess(
            this IServiceCollection services)
        {
            Mock<ICustomerRepository> repositoryMock = services.MockClass<ICustomerRepository>();

            repositoryMock
                .Setup(x => x.Store(It.IsAny<CustomerModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CustomerEntity
                {
                    Id = 1,
                    Name = "Test Customer",
                    Email = "test@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Test Address"
                });

            return services;
        }

        public static IServiceCollection GivenCustomerRepositoryThrowsDuplicateEmailException(
            this IServiceCollection services)
        {
            Mock<ICustomerRepository> repositoryMock = services.MockClass<ICustomerRepository>();

            repositoryMock
                .Setup(x => x.Store(It.IsAny<CustomerModel>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Ya existe un cliente con este email"));

            return services;
        }

        public static Task<CustomerModel?> WhenCreateCustomer(this IServiceProvider services, CreateCustomerCommand.Request request)
        {
            IRequestHandler<CreateCustomerCommand.Request, CustomerModel?> handler = services
                .GetRequiredService<IRequestHandler<CreateCustomerCommand.Request, CustomerModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldCompleteSuccessfully(this CustomerModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.Id > 0);
        }

        public static Task ThenShouldThrowValidationException(this Task task)
        {
            return Assert.ThrowsAsync<ValidationException>(() => task);
        }
    }
}
