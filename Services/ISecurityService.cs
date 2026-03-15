namespace NorthEndOneEntrance.Services;

public interface ISecurityService
{
    bool ValidatePin(string pin);
    string GetConfiguredPin();
}

public class MockSecurityService : ISecurityService
{
    // Mock security PIN - in production, this would come from secure configuration
    private const string SECURITY_PIN = "1234";

    public bool ValidatePin(string pin)
    {
        return !string.IsNullOrWhiteSpace(pin) && pin == SECURITY_PIN;
    }

    public string GetConfiguredPin()
    {
        // In production, this would not be exposed
        // Only used for development/testing
        return SECURITY_PIN;
    }
}
