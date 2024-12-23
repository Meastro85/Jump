namespace Tests.Interception_test_domain.Domain;

public class TestObject
{
    public string Name { get; set; }
    public string Message { get; set; }

    public TestObject(string name, string message)
    {
        Name = name;
        Message = message;
    }
    
}