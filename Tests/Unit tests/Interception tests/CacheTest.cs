using Jump;
using Jump.Providers;
using Tests.Domains.Interception_test_domain;
using Tests.Domains.Interception_test_domain.Managers;

namespace Tests.Unit_tests.Interception_tests;

public class CacheTest
{
    [SetUp]
    public void SetUp()
    {
        JumpApplication.ScanComponents(typeof(Interception));
    }

    [TearDown]
    public void TearDown()
    {
        JumpApplication.Dispose();
    }

    [Test]
    public void CacheAttributeCachesFirstCall()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.That(manager1.CacheItem(), Is.Not.Null);
        Assert.That(manager1.CacheItem(), Is.EqualTo(manager2.CacheItem()));
    }

    [Test]
    public void CacheAttributeCachesFirstCallSecondTest()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.That(manager2.CacheItem(), Is.Null);
        Assert.That(manager2.CacheItem(), Is.EqualTo(manager1.CacheItem()));
    }

    [Test]
    public void CacheEvictShouldRemoveCachedItem()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.That(manager1.CacheItem(), Is.Not.Null);
        Assert.That(manager1.CacheItem(), Is.EqualTo(manager2.CacheItem()));

        manager1.RemoveCacheItem();

        Assert.That(manager2.CacheItem(), Is.Null);
        Assert.That(manager2.CacheItem(), Is.EqualTo(manager1.CacheItem()));
    }

    [Test]
    public void CachedMethodWithIdShouldReturnCorrectValue()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.That(manager1.CacheItemWithId(1), Is.Not.Null);
        Assert.That(manager1.CacheItemWithId(1), Is.EqualTo(manager2.CacheItemWithId(1)));
        Assert.That(manager2.CacheItemWithId(2), Is.Null);
    }

    [Test]
    public void CacheEvictWithIdShouldRemoveCachedItem()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.That(manager1.CacheItemWithId(1), Is.Not.Null);
        Assert.That(manager1.CacheItemWithId(1), Is.EqualTo(manager2.CacheItemWithId(1)));

        manager1.RemoveCacheItemWithId(1);

        Assert.That(manager2.CacheItemWithId(1), Is.Null);
        Assert.That(manager2.CacheItemWithId(1), Is.EqualTo(manager1.CacheItemWithId(1)));
    }
}