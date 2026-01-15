using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderHub.Customer.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerEFrepository(this IServiceCollection services)
        {
            services.AddDbContext<CustomerDbContext>(options => 
                options.UseInMemoryDatabase("InMemoryDb"));
            
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

    }
}
