using Jump.Attributes.Components;
using Jump.Attributes.Components.Controllers;
using Jump.Listeners;
using Jump.LoggingSetup;
using Jump.Providers;
using Jump.Providers.Component_store;
using Jump.Util;

namespace Jump;

/// <summary>
///     Class <c>JumpApplication</c> is the primary starting point of the dependency injection framework.
/// </summary>
public static class JumpApplication
{
    private static readonly ComponentStore ComponentStore = ComponentStore.Instance;
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;
    private static readonly ConfigurationProvider ConfigurationProvider = ConfigurationProvider.Instance;

    /// <summary>
    ///     This method starts the program and registers all components.
    /// </summary>
    /// <param name="primarySource">The starting class to begin assembly scanning.</param>
    public static async Task Run(Type primarySource)
    {
        ScanComponents(primarySource);
        await RegisterListeners();
    }

    public static void ScanComponents(Type primarySource)
    {
        OrderComponents(primarySource);
        RegisterSingletons();
        RegisterConfigurations();
    }

    private static void OrderComponents(Type primarySource)
    {
        Logging.Logger.LogInformation("Starting component scanning.");
        var components = primarySource.Assembly
            .DefinedTypes
            .Where(t => t.CustomAttributes.Any(attr =>
                Utility.InheritsFromAttribute(attr.AttributeType, typeof(Component))));

        foreach (var component in components) ComponentStore.AddComponent(component);
    }

    private static void RegisterSingletons()
    {
        Logging.Logger.LogInformation("Registering singletons.");
        var singletons = ComponentStore.GetSingletons();
        ComponentProvider.AddSingletons(singletons);
    }

    private static void RegisterConfigurations()
    {
        Logging.Logger.LogInformation("Registering configurations.");
        var configurations = ComponentStore.GetConfigurations().ToList();
        if (configurations.Count > 0) ConfigurationProvider.AddConfigurations(configurations);
    }

    private static async Task RegisterListeners()
    {
        Logging.Logger.LogInformation("Registering listeners.");
        var components = ComponentStore.GetComponents();
        List<Task> tasks = [];

        foreach (var kvp in components.AsParallel())
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

        Logging.Logger.LogInformation("Started listeners.");
        await Task.WhenAll(tasks);
    }
}