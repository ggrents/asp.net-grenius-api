using grenius_api.Application.Controllers;
using grenius_api.Domain.Exceptions;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using NotImplementedException = grenius_api.Domain.Exceptions.NotImplementedException;
using UnauthorizedAccessException = grenius_api.Domain.Exceptions.UnauthorizedAccessException;

namespace grenius_api.Application.Middleware
{
    public class ErrorHandlingMiddleware 
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            HttpStatusCode statusCode;
            string? stackTrace = string.Empty;
            string message = string.Empty;

            var exceptionType = ex.GetType();

            if (exceptionType  == typeof(NotFoundException)) {
                message = ex.Message;
                statusCode = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
            }
            
            else if (exceptionType == typeof(BadRequestException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
            }
            
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.NotImplemented;
                stackTrace = ex.StackTrace;
            }
            
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.Unauthorized;
                stackTrace = ex.StackTrace;
            }
            
            else
            {
                message = "An unhandled error has occurred while executing the request.";
                statusCode = HttpStatusCode.InternalServerError;
            }

            var result = JsonSerializer.Serialize(new { message = message, status = (int)statusCode});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            logger.LogError("Error has occurred, @{result}", result);
            return context.Response.WriteAsync(result);
        }
    }
}
