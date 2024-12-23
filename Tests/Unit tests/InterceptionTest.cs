using Jump.Providers;
using Tests.Fixtures;
using Tests.Interception_test_domain.Managers;

namespace Tests.Unit_tests;

public class InterceptionTest : IClassFixture<InterceptionFixture>
{
    [Fact]
    public void CacheAttributeCachesFirstCall()
    {
        
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();

        Assert.NotNull(manager1.CacheItem());
        Assert.Equal(manager1.CacheItem(), manager2.CacheItem());
        
        manager1.RemoveCacheItem();
    }

    [Fact]
    public void CacheAttributeCachesFirstCallSecondTest()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();
        
        Assert.Null(manager2.CacheItem());
        Assert.Equal(manager2.CacheItem(), manager1.CacheItem());
        
        manager1.RemoveCacheItem();
    }

    [Fact]
    public void CacheEvictShouldRemoveCachedItem()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();
        
        Assert.NotNull(manager1.CacheItem());
        Assert.Equal(manager1.CacheItem(), manager2.CacheItem());
        
        manager1.RemoveCacheItem();
        
        Assert.Null(manager2.CacheItem());
        Assert.Equal(manager2.CacheItem(), manager1.CacheItem());
        
        manager1.RemoveCacheItem();
    }

    [Fact]
    public void CachedMethodWithIdShouldReturnCorrectValue()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();
        
        Assert.NotNull(manager1.CacheItemWithId(1));
        Assert.Equal(manager1.CacheItemWithId(1), manager2.CacheItemWithId(1));
        Assert.Null(manager2.CacheItemWithId(2));
        
        manager1.RemoveCacheItemWithId(1);
    }

    [Fact]
    public void CacheEvictWithIdShouldRemoveCachedItem()
    {
        var provider = ComponentProvider.Instance;
        var manager1 = provider.GetComponent<CachedManager1>();
        var manager2 = provider.GetComponent<CachedManager2>();
        
        Assert.NotNull(manager1.CacheItemWithId(1));
        Assert.Equal(manager1.CacheItemWithId(1), manager2.CacheItemWithId(1));
        
        manager1.RemoveCacheItemWithId(1);
        
        Assert.Null(manager2.CacheItemWithId(1));
        Assert.Equal(manager2.CacheItemWithId(1), manager1.CacheItemWithId(1));
        
        manager1.RemoveCacheItemWithId(1);
    }
    
}