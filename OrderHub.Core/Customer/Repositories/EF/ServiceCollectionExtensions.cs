using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Api;

namespace OrderHub.Customer.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerEFrepository(
            this IServiceCollection services,
            InMemoryDatabaseRoot databaseRoot)
        {
            services.AddDbContext<CustomerDbContext>(options => 
                options.UseInMemoryDatabase(ApplicationSetting.InMemoryDatabaseName, databaseRoot));
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

    }
}
