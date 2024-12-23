using System.Reflection;
using Jump.Attributes.Components;
using Jump.Exceptions;
using Jump.Util;

namespace Jump.Providers.Component_store;

internal sealed class ComponentStore
{
    private static ComponentStore? _instance;
    private static readonly object Padlock = new();
    private readonly ComponentRegistry _componentRegistry = new();
    private readonly SingletonStore _singletonStore = new();

    internal static ComponentStore Instance
    {
        get
        {
            lock (Padlock)
            {
                return _instance ??= new ComponentStore();
            }
        }
    }

    internal static ComponentStore Dispose()
    {
        _instance = null;
        return Instance;
    }

    internal void AddComponent(Type component)
    {
        var componentType = ValidateAndGetComponentType(component);

        _singletonStore.RegisterSingleton(component);

        var constructor = ConstructorSearch.GetMainConstructor(component);
        _componentRegistry.RegisterConstructor(component, constructor);

        ComponentValidator.CreateGraph(component, constructor.GetParameters());

        _componentRegistry.RegisterComponent(component, componentType);
    }

    private static Type ValidateAndGetComponentType(Type component)
    {
        var componentAttributes = component.CustomAttributes
            .Where(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .ToList();

        if (componentAttributes.Count > 1)
            throw new TooManyAttributesException("Multiple component attributes on a single class is not allowed");

        var componentType = componentAttributes
            .First(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .AttributeType;

        ComponentValidator.ValidateComponent(component, componentType);
        return componentType;
    }

    internal List<Type> GetSingletons()
    {
        return _singletonStore.GetSingletons();
    }

    internal IEnumerable<Type> GetConfigurations()
    {
        return _componentRegistry.GetConfigurations();
    }

    internal IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _componentRegistry.GetComponents();
    }

    internal ConstructorInfo GetConstructor(Type type)
    {
        return _componentRegistry.GetConstructor(type);
    }

    internal ParameterInfo[] GetParameters(ConstructorInfo constructor)
    {
        return _componentRegistry.GetParameters(constructor);
    }
}