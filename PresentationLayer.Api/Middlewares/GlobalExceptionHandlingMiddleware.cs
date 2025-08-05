using System.Net;

namespace PresentationLayer.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // logic
                await _next(context);
                // logic
            }
            catch (Exception ex)
            {
                var id = Guid.NewGuid().ToString();
                _logger.LogError(ex, $"{id}-{ex.Message}");
                await HandleException(context, ex, id);
            }
        }

        public async Task HandleException(HttpContext context, Exception ex, string id)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An Internal Server Error Occured. Please try again later",
                Description = $"{id}-{ex.Message}" //  for development need
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
