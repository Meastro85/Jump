namespace Tests.Domains.Interception_test_domain.Domain;

public class TestObject
{
    public TestObject(string name, string message)
    {
        Name = name;
        Message = message;
    }

    public string Name { get; set; }
    public string Message { get; set; }
}