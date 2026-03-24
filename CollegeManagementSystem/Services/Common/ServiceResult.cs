namespace CollegeManagementSystem.Services.Common;

public enum ServiceErrorType
{
    BadRequest,
    NotFound,
    Conflict
}

public class ServiceResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public ServiceErrorType? ErrorType { get; init; }

    public static ServiceResult Ok() => new() { Success = true };

    public static ServiceResult Fail(ServiceErrorType errorType, string message) =>
        new() { Success = false, ErrorType = errorType, ErrorMessage = message };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };

    public static new ServiceResult<T> Fail(ServiceErrorType errorType, string message) =>
        new() { Success = false, ErrorType = errorType, ErrorMessage = message };
}
