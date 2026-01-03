using Microsoft.AspNetCore.Mvc;
using SmartERP.Application.Common.Responses;

namespace SmartERP.Api.Extensions;

public static class ApiResults
{
    public static ActionResult ApiCreated<T>(this ControllerBase c, string location, T data,
        string? message = "Created")
    {
        return c.Created(location, ApiResponse<T>.Ok(data, message, 201, c.HttpContext.TraceIdentifier));
    }

    public static ActionResult ApiOk<T>(this ControllerBase c, T data, string? message = null)
    {
        return c.Ok(ApiResponse<T>.Ok(data, message, 200, c.HttpContext.TraceIdentifier));
    }

    public static ActionResult ApiBadRequest(this ControllerBase c, string message, object? errors = null)
    {
        return c.BadRequest(ApiResponse<object>.Fail(message, 400, errors, c.HttpContext.TraceIdentifier));
    }

    public static ActionResult ApiNotFound(this ControllerBase c, string message, object? errors = null)
    {
        return c.NotFound(ApiResponse<object>.Fail(message, 404, errors, c.HttpContext.TraceIdentifier));
    }
}
