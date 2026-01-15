using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OrderHub.Core.Customer.Commands;
using OrderHub.Customer.Repositories;

namespace OrderHub.Test.Customer
{
    public static class DeleteCustomerSteps
    {
        public static IServiceCollection GivenCustomerRepositoryDeleteReturns(
            this IServiceCollection services,
            bool result)
        {
            Mock<ICustomerRepository> repositoryMock = services.MockClass<ICustomerRepository>();

            repositoryMock
                .Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            return services;
        }

        public static IServiceCollection GivenCustomerRepositoryDeleteThrowsException(
            this IServiceCollection services)
        {
            Mock<ICustomerRepository> repositoryMock = services.MockClass<ICustomerRepository>();

            repositoryMock
                .Setup(x => x.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            return services;
        }

        public static Task<bool> WhenDeleteCustomer(
            this IServiceProvider services,
            DeleteCustomerCommand.Request request)
        {
            IRequestHandler<DeleteCustomerCommand.Request, bool> handler = services
                .GetRequiredService<IRequestHandler<DeleteCustomerCommand.Request, bool>>();

            return handler.Handle(request, CancellationToken.None);
        }

        public static void ThenShouldDeleteSuccessfully(this bool result)
        {
            Assert.True(result);
        }

        public static void ThenShouldNotDelete(this bool result)
        {
            Assert.False(result);
        }

        public static Task ThenShouldThrowException<TException>(this Task task)
            where TException : Exception
        {
            return Assert.ThrowsAsync<TException>(() => task);
        }
    }
}
