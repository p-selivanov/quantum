namespace Quantum.Lib.AspNet;

public class ErrorResponse
{
    public string Error { get; }

    public string Code { get; }

    public ErrorResponse(string error, string code = null)
    {
        Error = error;
        Code = code;
    }
}
