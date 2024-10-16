namespace Bank.Api;

public class RequestLoggingMiddleware
{
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogRequest(context);

        await _next(context);

        LogResponse(context);
    }

    private void LogRequest(HttpContext context)
    {
        _logger.LogInformation("Handling request: {Method} {Path} {QueryString}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);
    }

    private void LogResponse(HttpContext context)
    {
        _logger.LogInformation("Response: {StatusCode}", context.Response.StatusCode);
    }
}