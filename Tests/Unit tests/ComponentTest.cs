using Jump.Providers;
using Tests.Component_test_domain.Controllers;
using Tests.Component_test_domain.Hop_test;
using Tests.Component_test_domain.Managers;
using Tests.Fixtures;

namespace Tests.Unit_tests;

public class ComponentTest : IClassFixture<ComponentFixture>
{
    private readonly ComponentFixture _fixture;

    public ComponentTest(ComponentFixture fixture)
    {
        _fixture = fixture;
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

    [Fact]
    public void HopGetsCreatedSuccessfullyWithDependencies()
    {
        var provider = ComponentProvider.Instance;
        var controller = provider.GetComponent<HopController>();

        Assert.NotNull(controller);
        Assert.Equal("Test This is a hop test", controller.Test());
    }
}