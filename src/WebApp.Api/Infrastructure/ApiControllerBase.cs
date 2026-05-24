using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Infrastructure;

public abstract class ApiControllerBase : ControllerBase
{
    protected async Task<ActionResult> ExecuteAsync(
        Func<Task<ActionResult>> action,
        bool treatUnauthorizedAsSessionExpired = false)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return (ActionResult)ControllerExceptionHandler.Handle(ex, treatUnauthorizedAsSessionExpired);
        }
    }

    protected async Task<IActionResult> ExecuteResultAsync(
        Func<Task<IActionResult>> action,
        bool treatUnauthorizedAsSessionExpired = false)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return ControllerExceptionHandler.Handle(ex, treatUnauthorizedAsSessionExpired);
        }
    }
}
