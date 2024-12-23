using Jump;
using Jump.Exceptions;
using Tests.Cyclic_dependency_test_domain.Indirect_dependency;
using FirstTestManager = Tests.Cyclic_dependency_test_domain.Direct_dependency.FirstTestManager;

namespace Tests.Unit_tests;

[Collection("CyclicDependencyTest")]
public class CyclicDependencyTest
{
    [Fact]
    public void DirectCyclicDependenciesAreDetected()
    {
        Assert.Throws<CyclicDependencyException>(() => JumpApplication.ScanComponents(typeof(FirstTestManager)));
    }

    [Fact]
    public void IndirectCyclicDependenciesAreDetected()
    {
        Assert.Throws<CyclicDependencyException>(() => JumpApplication.ScanComponents(typeof(ThirdTestManager)));
    }
}