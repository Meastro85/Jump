using Castle.DynamicProxy;
using Jump.Attributes.Cache;
using Jump.Providers;

namespace Jump.Interceptors;

public class CacheableInterceptor : IInterceptor

{
    public void Intercept(IInvocation invocation)
    {
        if (!invocation.Method.GetCustomAttributes(false).Any(attr => attr is Cacheable))
        {
            invocation.Proceed();
            return;
        }
        var attribute = invocation.Method.GetCustomAttributes(false).First(attr => attr is Cacheable) as Cacheable;
        
        var key = Utility.CreateCacheKey(attribute!.Key, invocation.Arguments);
        var cacheProvider = CacheProvider.Instance;
        if (cacheProvider.TryGetFromCache(key, out var value))
        {
            invocation.ReturnValue = value;
            return;
        }
        invocation.Proceed();
        cacheProvider.AddToCache(key, invocation.ReturnValue);
    }
    
}