using Jump.Attributes.Components;
using Jump.Exceptions;

namespace Jump.Providers;

internal sealed class ComponentStore
{

    private static ComponentStore? _instance;
    private static readonly object Padlock = new();
    private readonly Dictionary<Type, ICollection<Type>> _components = new();
    private readonly List<Type> _singletons = new();

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
    
    internal void AddComponent(Type component)
    {
        var componentAttributes = component.CustomAttributes
            .Where(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .ToList();
        
        bool isSingleton = component.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton));
        
        if(componentAttributes.Count > 1) throw new TooManyAttributesException("Multiple component attributes on a single class is not allowed");
            
        var componentType = componentAttributes
            .First(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .AttributeType;
        
        if(isSingleton) _singletons.Add(component);
        
        if (_components.TryGetValue(componentType, out var list)) list.Add(component);
        else _components.Add(componentType, [component]);
    }

    internal List<Type> GetSingletons()
    {
        return _singletons;
    }
    
    internal IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _components;
    }
    
}