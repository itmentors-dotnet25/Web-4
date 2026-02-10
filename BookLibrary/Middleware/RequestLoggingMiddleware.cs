namespace BookLibrary.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var query = context.Request.QueryString;

        try
        {
            logger.LogInformation("Started {Method} {Path}{Query}", method, path, query);
            
            await next(context);

            var duration = DateTime.UtcNow - startTime;
            var statusCode = context.Response.StatusCode;

            logger.LogInformation(
                "Completed {Method} {Path} with status {StatusCode} in {DurationMs}ms",
                method, path, statusCode, duration.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - startTime;
            
            logger.LogError(
                ex, 
                "Failed {Method} {Path} after {DurationMs}ms", 
                method, path, duration.TotalMilliseconds);
            
            throw;
        }
    }
}
