using Jump;
using Tests.Interception_test_domain;

namespace Tests.Fixtures;

public sealed class InterceptionFixture : IDisposable
{
    public InterceptionFixture()
    {
        JumpApplication.ScanComponents(typeof(BaseClass));
    }

    public void Dispose()
    {
        JumpApplication.Dispose();
    }
}