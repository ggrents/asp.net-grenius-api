using grenius_api.Application.Middleware;

namespace grenius_api.Application.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
