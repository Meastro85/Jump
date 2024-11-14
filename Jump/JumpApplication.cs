namespace Jump;

public static class JumpApplication
{

    private static readonly ComponentStore ComponentStore = ComponentStore.Instance;
    
    public static void Run(Type primarySource)
    {
        OrderComponents(primarySource);
        
    }

    private static void OrderComponents(Type primarySource)
    {
        var components = primarySource.Assembly
            .DefinedTypes
            .Where(t => t.CustomAttributes.Any(attr => Utility.InheritsFromComponent(attr.AttributeType)));

        foreach (var component in components)
        {
            ComponentStore.AddComponent(component);
        }
        
        foreach (var kvp in ComponentStore.GetComponents())
        {
            Console.WriteLine($"Component: {kvp.Key.Name}");

            foreach (var type in kvp.Value)
            {
                Console.WriteLine($"\t- {type.Name}");
            }
        }
        
        foreach (var kvp in ComponentStore.GetConstructors())
        {
            Console.WriteLine($"Component: {kvp.Key.Name}");

            foreach (var constructor in kvp.Value)
            {
                Console.WriteLine($"\t- Constructor: {constructor}");
            }
        }
        
    }
    
    
}