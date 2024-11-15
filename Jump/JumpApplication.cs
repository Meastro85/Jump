using Jump.Attributes.Components.Controllers;
using Jump.Listeners;

namespace Jump;

public static class JumpApplication
{

    private static readonly ComponentStore ComponentStore = ComponentStore.Instance;
    
    public static async Task Run(Type primarySource)
    {
        OrderComponents(primarySource);
        await RegisterListeners();
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

    private static async Task RegisterListeners()
    {
        var components = ComponentStore.GetComponents();
        List<Task> tasks = new();
        
        foreach (var kvp in components.AsParallel())
        {
            switch (kvp.Key)
            {
                case { } type 
                    when type == typeof(KeyboardController):
                    Console.WriteLine("Registering keyboard controllers");
                    foreach (var controller in kvp.Value)
                    {
                        var constructor = controller.GetConstructors()[0];
                        var keyboardController = constructor.Invoke(null);
                        tasks.Add(KeyBoardListener.StartKeyListening(keyboardController));
                    }
                    break;
            }
        }
        await Task.WhenAll(tasks);
    }
    
}