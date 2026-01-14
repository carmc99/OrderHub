using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Moq;

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
    }
}
