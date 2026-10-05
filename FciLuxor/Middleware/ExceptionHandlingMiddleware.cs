namespace FciLuxor.Middleware
{
    // Turns domain exceptions from the handlers into proper HTTP problem responses.
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (KeyNotFoundException ex)
            {
                await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Not found", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteProblemAsync(context, StatusCodes.Status409Conflict, "Conflict", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.");
            }
        }

        private static Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            }, options: null, contentType: "application/problem+json");
        }
    }
}
