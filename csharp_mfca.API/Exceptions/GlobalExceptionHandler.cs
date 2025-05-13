using csharp_mfca.API.Extensions;
using csharp_mfca.API.Utils;
using Microsoft.AspNetCore.Diagnostics;

namespace csharp_mfca.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var result = BaseResponse<object>.Fail(httpContext.TraceIdentifier, exception);
        httpContext.Response.ContentType = "application/json";

        string jsonStr = result.ToJson();
        await httpContext.Response.WriteAsync(jsonStr, cancellationToken);

        return true;
    }
}
