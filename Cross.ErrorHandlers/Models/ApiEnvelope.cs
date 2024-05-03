namespace Cross.ErrorHandlers.Models;

public class ApiEnvelope : ApiEnvelope<Unit>
{
    public ApiEnvelope(Unit unit)
        : base(unit)
    {
    }
}
