using PlaylistManagement.Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace PlaylistManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,"An unhandled exception occurred.");

                await HandleExceptionAsync(context, exception);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            var statusCode = exception switch
            {
                UnauthorizedAccessException =>(int)HttpStatusCode.Unauthorized,

                ForbiddenException =>(int)HttpStatusCode.Forbidden,

                NotFoundException =>(int)HttpStatusCode.NotFound,

                ConflictException =>(int)HttpStatusCode.Conflict,

                _ =>(int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = statusCode,
                message = exception.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
