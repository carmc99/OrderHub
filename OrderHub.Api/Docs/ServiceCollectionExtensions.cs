using Microsoft.OpenApi.Models;

namespace OrderHub.Api.Docs
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type =>
                    type.ToString()
                        .Replace('+', '.'));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OrderHub",
                });
            });

            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
