using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Customer.Repositories.EF;

namespace OrderHub.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderHubCore(this IServiceCollection services)
        {
            services.AddCustomerEFrepository();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
