namespace Cross.ErrorHandlers.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }

    public ConflictException(string message, string subCode) : base(message)
    {
        Data.Add("SubCode", subCode);
    }
}
