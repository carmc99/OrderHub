using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Api;

namespace OrderHub.Core.Order.Repositories.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderEFrepository(
            this IServiceCollection services,
            InMemoryDatabaseRoot databaseRoot)
        {
            services.AddDbContext<OrderDbContext>(options =>
              options.UseInMemoryDatabase(ApplicationSetting.InMemoryDatabaseName, databaseRoot));

            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
