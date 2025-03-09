using Jump;
using Jump.Providers;
using Tests.Domains.Component_test_domain;
using Tests.Domains.Component_test_domain.Controllers;
using Tests.Domains.Component_test_domain.Hop_test;
using Tests.Domains.Component_test_domain.Managers;

namespace Tests.Unit_tests.DI_tests;

[TestFixture]
public class ComponentTest
{
    [SetUp]
    public void SetUp()
    {
        JumpApplication.ScanComponents(typeof(IBaseClass));
    }

    [TearDown]
    public void TearDown()
    {
        JumpApplication.Dispose();
    }

    [Test]
    public void ComponentCanBeCreated()
    {
        var provider = ComponentProvider.Instance;

        var testManager = provider.GetComponent<TestManager>();

        Assert.That(testManager, Is.Not.Null);
        Assert.That(testManager.Test(), Is.EqualTo("Test"));
    }

    [Test]
    public void ComponentsCanBeCreatedWithDependencies()
    {
        var provider = ComponentProvider.Instance;

        var testController = provider.GetComponent<TestController>();

        Assert.That(testController, Is.Not.Null);
        Assert.That(testController.Test(), Is.EqualTo("Test"));
    }

    [Test]
    public void HopGetsCreatedSuccessfullyWithDependencies()
    {
        var provider = ComponentProvider.Instance;
        var controller = provider.GetComponent<HopController>();

        Assert.That(controller, Is.Not.Null);
        Assert.That(controller.Test(), Is.EqualTo("Test This is a hop test"));
    }
}