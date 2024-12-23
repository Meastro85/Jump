using Jump;
using Tests.Interception_test_domain;

namespace Tests.Fixtures;

public sealed class InterceptionFixture : IDisposable
{
    public InterceptionFixture()
    {
        JumpApplication.ScanComponents(typeof(Interception));
    }

    public void Dispose()
    {
        JumpApplication.Dispose();
    }
}