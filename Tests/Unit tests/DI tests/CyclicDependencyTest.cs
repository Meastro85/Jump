using Jump;
using Jump.Exceptions;
using Tests.Domains.Cyclic_dependency_test_domain.Indirect_dependency;
using FirstTestManager = Tests.Domains.Cyclic_dependency_test_domain.Direct_dependency.FirstTestManager;

namespace Tests.Unit_tests.DI_tests;

public class CyclicDependencyTest
{
    [TearDown]
    public void TearDown()
    {
        JumpApplication.Dispose();
    }

    [Test]
    public void DirectCyclicDependenciesAreDetected()
    {
        Assert.Throws<CyclicDependencyException>(() => JumpApplication.ScanComponents(typeof(FirstTestManager)));
    }

    [Test]
    public void IndirectCyclicDependenciesAreDetected()
    {
        Assert.Throws<CyclicDependencyException>(() => JumpApplication.ScanComponents(typeof(ThirdTestManager)));
    }
}