using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using TestObject = Tests.Interception_test_domain.Domain.TestObject;

namespace Tests.Interception_test_domain.Managers;

[Service]
public class CachedManager1
{
    
    [Cacheable("test")]
    public virtual TestObject CacheItem()
    {
        return new TestObject("Cached object", "This is a cached object");
    }

    [Cacheable("test", "id")]
    public virtual TestObject CacheItemWithId(int id)
    {
        TestObject[] items = [new("Cached object 1","This is the first object in the list."), new("Cached object 2", "This is the second object in the list.")];
        return items[id - 1];
    }
    
    [CacheEvict("test")]
    public virtual void RemoveCacheItem()
    {
    }
    
}