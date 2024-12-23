using Jump;
using Tests.Component_test_domain;

namespace Tests.Fixtures;

public sealed class ComponentFixture : IDisposable
{
    public ComponentFixture()
    {
        JumpApplication.ScanComponents(typeof(BaseClass));
    }

    public void Dispose()
    {
        JumpApplication.Dispose();
    }
}