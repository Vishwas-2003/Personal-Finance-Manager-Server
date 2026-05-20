using WebApp.Common.Constants;
using WebApp.Common.Models.Api;

namespace WebApp.Api.Infrastructure;

public static class SessionExpiredResponseWriter
{
    public const string DefaultMessage = "Your session has expired. Please login again.";

    public static Task WriteAsync(HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        response.ContentType = "application/json";

        return response.WriteAsJsonAsync(new ApiErrorResponse
        {
            Code = ApiErrorCodes.SessionExpired,
            Message = DefaultMessage
        });
    }
}
