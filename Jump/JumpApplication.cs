using Jump.Attributes.Components;
using Jump.Attributes.Components.Controllers;
using Jump.Listeners;
using Jump.Providers;

namespace Jump;

/// <summary>
/// Class <c>JumpApplication</c> is the primary starting point of the dependency injection framework.
/// </summary>
public static class JumpApplication
{

    private static readonly ComponentStore ComponentStore = ComponentStore.Instance;
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;
    
    /// <summary>
    /// This method starts the program and registers all components.
    /// </summary>
    /// <param name="primarySource">The starting class to begin assembly scanning.</param>
    public static async Task Run(Type primarySource)
    {
        OrderComponents(primarySource);
        RegisterSingletons();
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
        
    }

    private static void RegisterSingletons()
    {
        var singletons = ComponentStore.GetSingletons();
        ComponentProvider.AddSingletons(singletons);
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
                    tasks.Add(KeyBoardListener.RegisterKeyboardControllers(kvp.Value));
                    break;
                case { } type 
                    when type == typeof(RestController):
                    tasks.Add(RestListener.RegisterRestControllers(kvp.Value));
                    break;
            }
        }
        await Task.WhenAll(tasks);
    }

    
    
}