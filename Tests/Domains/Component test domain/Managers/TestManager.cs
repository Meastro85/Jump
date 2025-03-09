using Jump.Attributes.Components;

namespace Tests.Domains.Component_test_domain.Managers;

[Service]
public class TestManager
{
    
    protected TestManager(){}
    
    public string Test()
    {
        return "Test";
    }
}