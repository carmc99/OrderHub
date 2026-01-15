using Microsoft.Extensions.DependencyInjection;
using OrderHub.Core;

namespace OrderHub.Test
{
    internal sealed class TestApplicationBuilder
    {
        private readonly IServiceCollection Services = new ServiceCollection();

        public IServiceProvider Setup(Action<IServiceCollection>? configure = null)
        {
            Services.AddOrderHubCore();

            configure?.Invoke(Services);
            return Services.BuildServiceProvider();
        }
    }
}
