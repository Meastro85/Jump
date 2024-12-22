using Jump;
using Jump.Providers;
using Tests.Component_test_domain;
using Tests.Test_Domain;
using Tests.Test_Domain.Controllers;
using Tests.Test_Domain.Managers;

namespace Tests.Unit_tests;

public class ComponentTest
{
    public ComponentTest()
    {
        JumpApplication.ScanComponents(typeof(BaseClass));
    }

    [Fact]
    public void ComponentCanBeCreated()
    {
        var provider = ComponentProvider.Instance;

        var testManager = provider.GetComponent<TestManager>();

        Assert.NotNull(testManager);
        Assert.Equal("Test", testManager.Test());
    }

    [Fact]
    public void ComponentsCanBeCreatedWithDependencies()
    {
        var provider = ComponentProvider.Instance;

        var testController = provider.GetComponent<TestController>();

        Assert.NotNull(testController);
        Assert.Equal("Test", testController.Test());
    }
}