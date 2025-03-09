using System.Reflection;
using Jump.Attributes.Actions;
using Jump.LoggingSetup;
using Jump.Providers;

namespace Jump.Listeners;

internal static class KeyBoardListener
{
    private static readonly ComponentProvider ComponentProvider = ComponentProvider.Instance;
    private static bool _enabled;

    internal static Task RegisterKeyboardControllers(ICollection<Type> controllers)
    {
        var keyMappings = RegisterAllMappings(controllers);
        return StartKeyboardListener(keyMappings);
    }


    private static Dictionary<ConsoleKey, (object Controller, MethodInfo Method)> RegisterAllMappings(
        ICollection<Type> controllers)
    {
        var routeMappings = new Dictionary<ConsoleKey, (object Controller, MethodInfo Method)>();
        foreach (var controller in controllers)
        {
            var keyboardController = ComponentProvider.GetComponent(controller);
            var keyMappings = DiscoverKeyMappings(keyboardController);
            foreach (var (key, method) in keyMappings)
            {
                Logging.LogInformation("Registering key: " + key);
                if (routeMappings.ContainsKey(key)) throw new AmbiguousMatchException($"Key {key} is ambiguous");
                routeMappings[key] = (keyboardController, method);
            }

            Logging.LogInformation("Registered keyboard listener: " + controller);
        }

        return routeMappings;
    }

    private static Dictionary<ConsoleKey, MethodInfo> DiscoverKeyMappings(object controller)
    {
        var keyMappings = new Dictionary<ConsoleKey, MethodInfo>();
        var controllerType = controller.GetType();
        foreach (var method in controllerType.GetMethods())
        {
            var keyActionAttribute = method.GetCustomAttribute<KeyAction>();
            if (keyActionAttribute == null) continue;
            keyMappings[keyActionAttribute.Key] = method;
        }

        return keyMappings;
    }

    private static async Task StartKeyboardListener(
        Dictionary<ConsoleKey, (object Controller, MethodInfo Method)> routeMappings)
    {
        _enabled = true;
        while (_enabled)
        {
            var key = (await Task.Run(() => Console.ReadKey(true))).Key;
            if (!routeMappings.TryGetValue(key, out var mapping)) continue;
            var (controller, method) = mapping;
            method.Invoke(controller, []);
        }
    }
}