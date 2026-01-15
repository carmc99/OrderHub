using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core.Order.Repositories.EF;
using OrderHub.Core.Repositories.EF;
using OrderHub.Customer.Repositories.EF;

namespace OrderHub.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderHubCore(this IServiceCollection services)
        {
            services.AddCustomerEFrepository();
            services.AddOrderEFrepository();
            services.AddReadDbContext();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }

        private static IServiceCollection AddReadDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ReadDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));

            return services;
        }
    }
}
