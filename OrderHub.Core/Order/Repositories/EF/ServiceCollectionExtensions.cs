using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderHub.Customer.Repositories.EF;

namespace OrderHub.Core.Order.Repositories.EF
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderEFrepository(this IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
              options.UseInMemoryDatabase("InMemoryDb"));

            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
