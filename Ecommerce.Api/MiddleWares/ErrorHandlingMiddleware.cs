namespace Ecommerce.Api.MiddleWares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next; // next middleware in the pipeline
        private readonly ILogger<ErrorHandlingMiddleware> _logger; // logger for logging errors
        // Lets me log exceptions with ASP.NET Core's logging system.

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware
                // Run the next middleware (e.g., routing, controllers, etc.)

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred."); // Log the exception

                // Set HTTP response status to 500 (Internal Server Error).
                context.Response.StatusCode = 500; // set response status
                
                // Send a JSON response back to the client.
                await context.Response.WriteAsJsonAsync(new { message = "Something went wrong!" });


            }
        }
    }
}