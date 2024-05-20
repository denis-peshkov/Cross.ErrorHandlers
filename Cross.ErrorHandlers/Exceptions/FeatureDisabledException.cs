namespace Cross.ErrorHandlers.Exceptions;

public class FeatureDisabledException : Exception
{
    public FeatureDisabledException(string message) : base(message)
    {
    }
}
