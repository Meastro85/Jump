using Jump.Attributes.Components.Controllers;
using Tests.Component_test_domain.Managers;

namespace Tests.Component_test_domain.Hop_test;

[RestController]
public class HopController(TestManager manager, string hopMessage)
{
    public string Test()
    {
        return manager.Test() + " " + hopMessage;
    }
}