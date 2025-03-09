using Jump.Attributes.Components.Controllers;
using Tests.Domains.Component_test_domain.Managers;

namespace Tests.Domains.Component_test_domain.Hop_test;

[RestController]
public class HopController(TestManager manager, string hopMessage)
{
    public string Test()
    {
        return manager.Test() + " " + hopMessage;
    }
}