using System.Diagnostics.CodeAnalysis;

namespace SmartERP.Domain.Common;

public class Result
{
    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = new Dictionary<string, string> { { "", error } };
    }

    protected Result(bool isSuccess, string errorKey, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = new Dictionary<string, string> { { errorKey, error } };
    }

    protected Result(bool isSuccess, Dictionary<string, string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? [];
        Error = string.Empty;

        if (Errors.Count > 0) Error = string.Join("; ", errors.Select(e => $"{e.Key}: {e.Value}"));
    }

    protected Result(bool isSuccess, IDictionary<string, string[]> errors)
    {
        IsSuccess = isSuccess;
        Errors = new Dictionary<string, string>();
        Error = string.Empty;

        if (errors != null && errors.Count > 0)
        {
            Errors = errors.ToDictionary(
                kvp => kvp.Key,
                kvp => string.Join(", ", kvp.Value));

            Error = string.Join("; ", Errors.Select(e => $"{e.Key}: {e.Value}"));
        }
    }

    public bool IsSuccess { get; }

    public string Error { get; }

    public Dictionary<string, string> Errors { get; }

    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    public static Result Failure(string erroKey, string error)
    {
        return new Result(false, erroKey, error);
    }

    public static Result Failure(Dictionary<string, string> errors)
    {
        return new Result(false, errors);
    }

    public static Result Failure(IDictionary<string, string[]> errors)
    {
        return new Result(false, errors);
    }
}

[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
public class Result<T> : Result
{
    private Result(bool isSuccess, string error, T value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    private Result(bool isSuccess, string errorKey, string error, T value)
        : base(isSuccess, errorKey, error)
    {
        Value = value;
    }

    private Result(bool isSuccess, Dictionary<string, string> errors, T value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    private Result(bool isSuccess, IDictionary<string, string[]> errors, T value)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, string.Empty, value);
    }

    public new static Result<T> Failure(string error)
    {
        return new Result<T>(false, error, default!);
    }

    public new static Result<T> Failure(string errorKey, string error)
    {
        return new Result<T>(false, errorKey, error, default!);
    }

    public new static Result<T> Failure(Dictionary<string, string> errors)
    {
        return new Result<T>(false, errors, default!);
    }

    public new static Result<T> Failure(IDictionary<string, string[]> errors)
    {
        return new Result<T>(false, errors, default!);
    }
}