using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderHub.Core.Customer.Commands;
using OrderHub.Core.Customer.Specifications;
using OrderHub.Customer.Models;
using OrderHub.Customer.Repositories;
using OrderHub.Customer.Repositories.EF.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderHub.Test.Customer
{
    public static class UpdateCustomerSteps
    {
        public static IServiceCollection GivenMediatorWithoutSetup(this IServiceCollection services)
        {
            services.MockClass<IMediator>();
            return services;
        }

        public static IServiceCollection GivenCustomerExists(
            this IServiceCollection services,
            int customerId)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.Is<SearchCustomerByIdSpecification>(s => s.Id == customerId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CustomerModel
                {
                    Id = customerId,
                    Name = "Existing Customer",
                    Email = "existing@example.com",
                    PhoneNumber = "3001234567",
                    Address = "Existing Address"
                });

            return services;
        }

        public static IServiceCollection GivenCustomerDoesNotExist(
            this IServiceCollection services,
            int customerId)
        {
            Mock<IMediator> mediatorMock = services.MockClass<IMediator>();

            mediatorMock
                .Setup(x => x.Send(It.Is<SearchCustomerByIdSpecification>(s => s.Id == customerId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerModel?)null);

            return services;
        }

        public static IServiceCollection GivenCustomerRepositoryReturnsUpdatedCustomer(
            this IServiceCollection services,
            CustomerModel expectedReturn)
        {
            Mock<ICustomerRepository> repositoryMock = services.MockClass<ICustomerRepository>();

            repositoryMock
                .Setup(x => x.Store(It.IsAny<CustomerModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CustomerModel model, CancellationToken ct) => new CustomerEntity
                {
                    Id = expectedReturn.Id,
                    Name = expectedReturn.Name,
                    Email = expectedReturn.Email,
                    PhoneNumber = expectedReturn.PhoneNumber,
                    Address = expectedReturn.Address
                });

            return services;
        }

        public static Task<CustomerModel?> WhenUpdateCustomer(this IServiceProvider services, UpdateCustomerCommand.Request request)
        {
            IRequestHandler<UpdateCustomerCommand.Request, CustomerModel?> handler = services
                .GetRequiredService<IRequestHandler<UpdateCustomerCommand.Request, CustomerModel?>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldUpdateSuccessfully(this CustomerModel? result)
        {
            Assert.NotNull(result);
            Assert.True(result!.Id > 0);
        }

        public static Task ThenShouldThrowInvalidOperationException(this Task task)
        {
            return Assert.ThrowsAsync<InvalidOperationException>(() => task);
        }
    }
}

