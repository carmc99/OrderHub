using Asp.Versioning;
using OrderHub.Api.Application;
using OrderHub.Api.Docs;
using OrderHub.Core;

namespace OrderHub.Api
{
    public static class StartUp
    {
        public static void Configure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.ConfigureSwagger();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwagger();
            services.AddVersioning();

            services.AddOrderHubCore();
        }

        private static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-API-Version")
                );
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
