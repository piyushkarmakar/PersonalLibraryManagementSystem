using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace PersonalLibraryAPI.Middleware
{
    public class ValidationLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationLoggingMiddleware> _logger;

        public ValidationLoggingMiddleware(RequestDelegate next, ILogger<ValidationLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Let request reach MVC first
            await _next(context);

            // Check AFTER MVC has processed model validation
            if (context.Response.StatusCode == 400 && context.Items.ContainsKey("InvalidModelState"))
            {
                string errors = context.Items["InvalidModelState"]?.ToString();
                _logger.LogWarning("MODEL VALIDATION FAILED → {Errors}", errors);
            }
        }
    }
}
