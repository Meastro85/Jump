using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using Tests.Domains.Interception_test_domain.Domain;

namespace Tests.Domains.Interception_test_domain.Managers;

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