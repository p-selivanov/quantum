namespace Quantum.Lib.AspNet
{
    public class ErrorResponse
    {
        public string Error { get; private set; }

        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}
