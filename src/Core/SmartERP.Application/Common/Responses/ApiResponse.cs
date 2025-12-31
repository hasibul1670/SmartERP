namespace SmartERP.Application.Common.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public object? Errors { get; init; }
    public string? TraceId { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null, int statusCode = 200, string? traceId = null)
    {
        return new ApiResponse<T>
            { Success = true, StatusCode = statusCode, Message = message, Data = data, TraceId = traceId };
    }

    public static ApiResponse<T> Fail(string message, int statusCode, object? errors = null, string? traceId = null)
    {
        return new ApiResponse<T>
            { Success = false, StatusCode = statusCode, Message = message, Errors = errors, TraceId = traceId };
    }
}