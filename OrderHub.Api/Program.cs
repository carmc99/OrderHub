using OrderHub.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();

WebApplication app = builder.Build();
app.Configure();

await app.RunAsync();
