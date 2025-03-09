using Jump.Attributes.Components.Controllers;
using Tests.Component_test_domain.Managers;

namespace Tests.Component_test_domain.Controllers;

[RestController]
public class TestController(TestManager manager)
{
    public string Test()
    {
        return manager.Test();
    }
}