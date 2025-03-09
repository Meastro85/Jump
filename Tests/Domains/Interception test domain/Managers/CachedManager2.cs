using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using Tests.Interception_test_domain.Domain;

namespace Tests.Interception_test_domain.Managers;

[Service]
public class CachedManager2
{
    
    [Cacheable("test")]
    public virtual TestObject CacheItem()
    {
        return null;
    }
    
    [Cacheable("test", "id")]
    public virtual TestObject CacheItemWithId(int id)
    {
        return null;
    }
    
}