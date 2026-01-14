namespace OrderHub.Customer.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerEFrepository(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

    }
}
