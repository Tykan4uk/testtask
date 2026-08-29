namespace Application.Common
{
    public class Error
    {
        public int Code { get; }

        public string Message { get; }

        public ErrorType Type { get; }

        public Error(int code, string message, ErrorType type = ErrorType.Failure)
        {
            Code = code;
            Message = message;
            Type = type;
        }
    }
}
