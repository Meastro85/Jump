using System.Reflection;
using Jump.Attributes;
using Jump.Attributes.Components;
using Jump.Exceptions;

namespace Jump.Providers;

internal sealed class ComponentStore
{

    private static ComponentStore? _instance;
    private static readonly object Padlock = new();
    private readonly Dictionary<Type, ICollection<Type>> _components = new();
    private readonly Dictionary<Type, ConstructorInfo> _constructors = new ();
    private readonly Dictionary<ConstructorInfo, ParameterInfo[]> _parameters = new ();
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
        
        if(componentAttributes.Count > 1) throw new TooManyAttributesException("Multiple component attributes on a single class is not allowed");
            
        var componentType = componentAttributes
            .First(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component)))
            .AttributeType;
        
        var isSingleton = component.CustomAttributes.Any(attr => attr.AttributeType == typeof(Singleton));
        
        if(isSingleton) _singletons.Add(component);
        
        
        var constructor = GetMainConstructor(component);
        _constructors.Add(component, constructor);
        _parameters.Add(constructor, constructor.GetParameters());
        
        if (_components.TryGetValue(componentType, out var list)) list.Add(component);
        else _components.Add(componentType, [component]);
    }
    
    internal List<Type> GetSingletons()
    {
        return _singletons;
    }

    internal IEnumerable<Type> GetConfigurations()
    {
        return _components[typeof(Configuration)];
    }
    
    internal IDictionary<Type, ICollection<Type>> GetComponents()
    {
        return _components;
    }

    internal ConstructorInfo GetConstructor(Type type)
    {
        return _constructors[type];
    }

    internal ParameterInfo[] GetParameters(ConstructorInfo constructor)
    {
        return _parameters[constructor];
    }
    
    private static ConstructorInfo GetMainConstructor(Type type)
    {
        var constructors = type.GetConstructors();
        var sortedConstructors = constructors.OrderByDescending(c => c.GetParameters().Length)
            .ToList();

        var autowiredConstructors = constructors
            .Where(c => c.GetCustomAttribute(typeof(AutoWired)) != null)
            .ToList();
        
        if ((sortedConstructors.Count > 1 && sortedConstructors[0].GetParameters().Length ==
            sortedConstructors[1].GetParameters().Length) || autowiredConstructors.Count > 1)
            throw new AmbiguousMatchException($"Type ${type.Name} has multiple valid constructors.");

        if(autowiredConstructors.Count == 1) return autowiredConstructors[0];
        var constructor = sortedConstructors.FirstOrDefault();
        
        if(constructor == null) throw new InvalidComponentException("No valid constructors found for " + type.Name);

        return constructor;
    }
    
}