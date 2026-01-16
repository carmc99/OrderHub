using FluentValidation;
using System.Net;
using System.Text.Json;

namespace OrderHub.Api.Application
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionHandlingMiddleware> Logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorResponse errorResponse = new();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Title = "Validation failed";
                    errorResponse.Detail = "One or more validation errors occurred.";
                    errorResponse.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    errorResponse.Errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    break;

                case InvalidOperationException invalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Title = "Invalid operation";
                    errorResponse.Detail = invalidOperationException.GetBaseException().Message;
                    errorResponse.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorResponse.Title = "Internal Server Error";
                    errorResponse.Detail = "An unexpected error occurred.";
                    errorResponse.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    break;
            }

            errorResponse.Status = (int)statusCode;
            errorResponse.Instance = context.Request.Path;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = errorResponse.Status;

            string jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
