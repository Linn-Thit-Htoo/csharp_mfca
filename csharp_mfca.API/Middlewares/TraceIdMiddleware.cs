namespace csharp_mfca.API.Middlewares;

public class TraceIdMiddleware(RequestDelegate next)
{
    internal readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("X-Trace-Id", out var headerValue);
        httpContext.TraceIdentifier = string.IsNullOrEmpty(headerValue)
            ? Guid.NewGuid().ToString()
            : headerValue!;

        httpContext.Response.Headers["X-Trace-Id"] = httpContext.TraceIdentifier;
        await _next(httpContext);
    }
}
