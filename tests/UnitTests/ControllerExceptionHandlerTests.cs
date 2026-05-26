using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.Infrastructure;
using WebApp.Common.Constants;
using WebApp.Common.Models.Api;

namespace UnitTests;

public class ControllerExceptionHandlerTests
{
    [Fact]
    public void Handle_should_map_unauthorized_to_401()
    {
        var result = ControllerExceptionHandler.Handle(new UnauthorizedAccessException("denied"));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var body = Assert.IsType<ApiErrorResponse>(objectResult.Value);
        Assert.Equal(ApiErrorCodes.Unauthorized, body.Code);
        Assert.Equal("denied", body.Message);
    }

    [Fact]
    public void Handle_should_map_session_expired_when_flag_is_set()
    {
        var result = ControllerExceptionHandler.Handle(
            new UnauthorizedAccessException("expired"),
            treatUnauthorizedAsSessionExpired: true);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var body = Assert.IsType<ApiErrorResponse>(objectResult.Value);
        Assert.Equal(ApiErrorCodes.SessionExpired, body.Code);
    }

    [Fact]
    public void Handle_should_map_invalid_operation_to_bad_request()
    {
        var result = ControllerExceptionHandler.Handle(new InvalidOperationException("bad op"));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, objectResult.StatusCode);
        var body = Assert.IsType<ApiErrorResponse>(objectResult.Value);
        Assert.Equal(ApiErrorCodes.BadRequest, body.Code);
    }

    [Fact]
    public void Handle_should_map_key_not_found_to_not_found()
    {
        var result = ControllerExceptionHandler.Handle(new KeyNotFoundException("missing"));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
    }

    [Fact]
    public void Handle_should_map_db_update_concurrency_to_conflict()
    {
        var result = ControllerExceptionHandler.Handle(new DbUpdateConcurrencyException());

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(409, objectResult.StatusCode);
    }

    [Fact]
    public void Handle_should_map_db_update_exception_to_bad_request()
    {
        var result = ControllerExceptionHandler.Handle(new DbUpdateException("db error", new Exception()));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, objectResult.StatusCode);
    }

    [Fact]
    public void Handle_should_map_unknown_exception_to_internal_error()
    {
        var result = ControllerExceptionHandler.Handle(new Exception("boom"));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var body = Assert.IsType<ApiErrorResponse>(objectResult.Value);
        Assert.Equal(ApiErrorCodes.InternalError, body.Code);
    }

    [Fact]
    public void Handle_should_inspect_inner_exceptions()
    {
        var inner = new KeyNotFoundException("inner missing");
        var result = ControllerExceptionHandler.Handle(new Exception("wrapper", inner));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
    }
}
