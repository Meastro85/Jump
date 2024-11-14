using System.Reflection;
using Jump.Attributes;

namespace Jump;

public static class JumpApplication
{

    private static readonly IDictionary<Type, ICollection<Type>> Components = new Dictionary<Type, ICollection<Type>>();
    private static readonly IDictionary<Type, ICollection<ConstructorInfo>> Constructors = new Dictionary<Type, ICollection<ConstructorInfo>>();
    
    public static void Run(Type primarySource)
    {
        var components = primarySource.Assembly
            .DefinedTypes
            .Where(t => t.CustomAttributes.Any(attr => InheritsFromComponent(attr.AttributeType)));

        foreach (var component in components)
        {
            var componentType = component.CustomAttributes
                .First(attr => InheritsFromComponent(attr.AttributeType))
                .AttributeType;
            
            if (Components.TryGetValue(componentType, out var list)) list.Add(component);
            else Components.Add(componentType, [component]);
            
            Constructors.Add(component, component.GetConstructors().ToList());
        }
        
        foreach (var kvp in Components)
        {
            Console.WriteLine($"Component: {kvp.Key.Name}");

            foreach (var type in kvp.Value)
            {
                Console.WriteLine($"\t- {type.Name}");
            }
        }
        
        foreach (var kvp in Constructors)
        {
            // Print the component type (key)
            Console.WriteLine($"Component: {kvp.Key.Name}");

            // Print each constructor in the collection (value)
            foreach (var constructor in kvp.Value)
            {
                Console.WriteLine($"\t- Constructor: {constructor}");
            }
        }
        
    }
    
    private static bool InheritsFromComponent(Type attributeType)
    {
        while (attributeType != null && attributeType != typeof(object))
        {
            if (attributeType == typeof(Component))
                return true;

            if (attributeType.BaseType != null) attributeType = attributeType.BaseType;
        }
        return false;
    }
}