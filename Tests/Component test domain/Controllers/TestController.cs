using Jump.Attributes.Components.Controllers;
using Tests.Test_Domain.Managers;

namespace Tests.Test_Domain.Controllers;

[RestController]
public class TestController(TestManager manager)
{
    public string Test()
    {
        return manager.Test();
    }
}