using Jump.Attributes.Components;

namespace Jump.Providers.Component_store;

internal class SingletonStore
{
    private readonly List<Type> _singletons = new();

    internal void RegisterSingleton(Type component)
    {
        if (component.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton))) _singletons.Add(component);
    }

    internal List<Type> GetSingletons()
    {
        return _singletons;
    }
}