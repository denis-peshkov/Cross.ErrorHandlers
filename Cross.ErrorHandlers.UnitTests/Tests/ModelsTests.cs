namespace Cross.ErrorHandlers.UnitTests.Tests;

[TestFixture]
public class ModelsTests
{
    [Test]
    public void ApiEnvelope_NonGenericConstructor_SetsData()
    {
        var envelope = new ApiEnvelope(Unit.Value);

        envelope.Data.Should().Be(Unit.Value);
        envelope.Error.Should().BeNull();
    }

    [Test]
    public void ApiEnvelope_GenericConstructor_WithData_SetsData()
    {
        var envelope = new ApiEnvelope<string>("payload");

        envelope.Data.Should().Be("payload");
        envelope.Error.Should().BeNull();
    }

    [Test]
    public void ErrorModel_Constructor_CodeAndMessage_SetsProperties()
    {
        var model = new ErrorModel("codeX", "msgY");

        model.Code.Should().Be("codeX");
        model.Message.Should().Be("msgY");
        model.SubCode.Should().BeNull();
        model.Errors.Should().BeNull();
    }
}
