using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
