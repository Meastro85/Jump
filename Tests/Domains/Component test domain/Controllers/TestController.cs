using Jump.Attributes.Components.Controllers;
using Tests.Domains.Component_test_domain.Managers;

namespace Tests.Domains.Component_test_domain.Controllers;

[RestController]
public class TestController(TestManager manager)
{
    public string Test()
    {
        return manager.Test();
    }
}