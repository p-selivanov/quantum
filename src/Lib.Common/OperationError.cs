namespace Quantum.Lib.Common;

public class OperationError
{
    public string Message { get; }

    public string Code { get; }

    public OperationError(string message, string code = null)
    {
        Code = code;
        Message = message;
    }
}
