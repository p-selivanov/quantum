namespace Quantum.Lib.Common;

public class OperationResult
{
    public const string NotFoundError = "Not Found";

    public string Error { get; protected set; }

    public bool IsSuccess
    {
        get
        {
            return string.IsNullOrEmpty(Error);
        }
    }

    public bool IsFailure
    {
        get
        {
            return string.IsNullOrEmpty(Error) == false;
        }
    }

    public bool IsNotFound
    {
        get
        {
            return Error == NotFoundError;
        }
    }

    public OperationResult(string errorMessage = null)
    {
        Error = errorMessage;
    }

    public static OperationResult Success()
    {
        return new OperationResult();
    }

    public static OperationResult Failure(string error)
    {
        return new OperationResult(error);
    }

    public static OperationResult NotFound()
    {
        return new OperationResult(NotFoundError);
    }
}

public class OperationResult<T> : OperationResult
{
    public T Value { get; private set; }

    public OperationResult(T value, string error = null)
        : base(error)
    {
        Value = value;
    }

    public static OperationResult<T> Success(T value)
    {
        return new OperationResult<T>(value);
    }

    public static new OperationResult<T> Failure(string error)
    {
        return new OperationResult<T>(default, error);
    }

    public static new OperationResult<T> NotFound()
    {
        return new OperationResult<T>(default, NotFoundError);
    }
}
