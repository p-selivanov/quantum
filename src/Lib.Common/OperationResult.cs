namespace Quantum.Lib.Common;

public class OperationResult
{
    public OperationError Error { get; }

    public bool IsSuccess => Error is null;

    public bool IsFailure => Error is not null;

    public OperationResult()
    {
    }

    public OperationResult(string errorMessage, string errorCode = null)
    {
        Error = new OperationError(errorMessage, errorCode);
    }

    public static OperationResult Success()
    {
        return new OperationResult();
    }

    public static OperationResult Failure(string message, string code = null)
    {
        return new OperationResult(message, code);
    }

    public static OperationResult<T> Success<T>(T value)
    {
        return new OperationResult<T>(value);
    }

    public static OperationResult<T> Failure<T>(string message, string code = null)
    {
        return new OperationResult<T>(message, code);
    }
}

public class OperationResult<T> : OperationResult
{
    public T Value { get; private set; }

    public OperationResult(T value)
    {
        Value = value;
    }

    public OperationResult(string errorMessage, string errorCode = null) 
        :base(errorCode, errorMessage)
    {
    }

    public static implicit operator OperationResult<T>(T value)
    {
        return new OperationResult<T>(value);
    }
}
