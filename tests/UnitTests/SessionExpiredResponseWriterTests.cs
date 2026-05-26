using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WebApp.Api.Infrastructure;
using WebApp.Common.Constants;
using WebApp.Common.Models.Api;

namespace UnitTests;

public class SessionExpiredResponseWriterTests
{
    [Fact]
    public async Task WriteAsync_should_write_session_expired_json_response()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await SessionExpiredResponseWriter.WriteAsync(context.Response);

        Assert.Equal(401, context.Response.StatusCode);
        context.Response.Body.Position = 0;
        var body = await JsonSerializer.DeserializeAsync<ApiErrorResponse>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(body);
        Assert.Equal(ApiErrorCodes.SessionExpired, body.Code);
        Assert.Equal(SessionExpiredResponseWriter.DefaultMessage, body.Message);
    }
}
