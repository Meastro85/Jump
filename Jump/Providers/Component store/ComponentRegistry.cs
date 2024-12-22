using System.Reflection;
using Jump.Attributes.Components;
using Jump.LoggingSetup;

namespace Jump.Providers.Component_store;

internal class ComponentRegistry
{
    private readonly Dictionary<Type, ICollection<Type>> _components = new();
    private readonly Dictionary<Type, ConstructorInfo> _constructors = new();
    private readonly Dictionary<ConstructorInfo, ParameterInfo[]> _parameters = new();

    internal void RegisterComponent(Type component, Type componentType)
    {
        if (_components.TryGetValue(componentType, out var list)) list.Add(component);
        else _components.Add(componentType, [component]);
        Logging.Logger.LogInformation("Added component: " + component);
    }

    internal void RegisterConstructor(Type component, ConstructorInfo constructor)
    {
        _constructors.Add(component, constructor);
        _parameters.Add(constructor, constructor.GetParameters());
    }

    internal IEnumerable<Type> GetConfigurations()
    {
        var hasConfig = _components.TryGetValue(typeof(Configuration), out var configurations);
        return hasConfig ? configurations! : [];
    }

    internal IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _components;
    }

    internal ConstructorInfo GetConstructor(Type component)
    {
        return _constructors[component];
    }

    internal ParameterInfo[] GetParameters(ConstructorInfo constructor)
    {
        return _parameters[constructor];
    }
}