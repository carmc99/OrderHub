using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using OrderHub.Core.Repositories.EF;

namespace OrderHub.Test
{
    internal static class ServiceCollectionExtensions
    {
        public static Mock<TClass> MockClass<TClass>(this IServiceCollection services)
            where TClass : class
        {
            Mock<TClass> moq = new();
            services.AddSingleton(moq.Object);

            return moq;
        }

        public static ReadDbContext GetInMemoryContext(this IServiceCollection services)
        {
            DbContextOptions<ReadDbContext> options = new DbContextOptionsBuilder<ReadDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            ReadDbContext context = new(options);

            services.AddSingleton(context);

            return context;
        }

        public static void LoadData<TClass>(this ReadDbContext context, List<TClass> data) where TClass : class
        {
            context.AddRange(data);
            context.SaveChanges();
        }
    }
}
