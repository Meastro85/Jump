using Jump.Attributes.Components;
using Jump.Attributes.Components.Controllers;
using Jump.Listeners;
using Jump.Providers;

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
            .Where(t => t.CustomAttributes.Any(attr => Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component))));

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
        
    }

    private static async Task RegisterListeners()
    {
        var components = ComponentStore.GetComponents();
        List<Task> tasks = [];
        
        foreach (var kvp in components.AsParallel())
        {
            switch (kvp.Key)
            {
                case { } type 
                    when type == typeof(KeyboardController):
                    tasks.AddRange(KeyBoardListener.RegisterKeyboardControllers(kvp.Value));
                    break;
                case { } type 
                    when type == typeof(RestController):
                    tasks.AddRange(RestListener.RegisterRestControllers(kvp.Value));
                    break;
            }
        }
        await Task.WhenAll(tasks);
    }

    
    
}