using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Api;
using OrderHub.Core.Customer.Repositories.EF;
using OrderHub.Core.Order.Repositories.EF;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Core
{
    public static class ServiceCollectionExtensions
    {
        // Usar InMemoryDatabaseRoot para persistir la informacion en multiples intancias DbContext
        // Particularidad de InMemoryDatabase
        private static readonly InMemoryDatabaseRoot DatabaseRoot = new();
        
        public static IServiceCollection AddOrderHubCore(this IServiceCollection services)
        {
            services.AddCustomerEFrepository(DatabaseRoot);
            services.AddOrderEFrepository(DatabaseRoot);
            services.AddReadDbContext();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }

        private static IServiceCollection AddReadDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ReadDbContext>(options =>
                options.UseInMemoryDatabase(ApplicationSetting.InMemoryDatabaseName, DatabaseRoot));

            return services;
        }
    }
}
