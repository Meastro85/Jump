using System.Reflection;
using Jump.Attributes.Actions;

namespace Jump.Listeners;

public static class KeyBoardListener
{
    
    private static Dictionary<ConsoleKey, Action> RegisterKeyboardController(object keyboardController)
    {
        var actions = new Dictionary<ConsoleKey, Action>();
        var controllerType = keyboardController.GetType();
        foreach (var method in controllerType.GetMethods())
        {
            var keyActionAttribute = method.GetCustomAttribute<KeyAction>();
            if (keyActionAttribute == null) continue;
            var action = (Action)Delegate.CreateDelegate(typeof(Action), keyboardController, method);
            actions[keyActionAttribute.Key] = action;
        }
        return actions;
    }
    
    public static async Task StartKeyListening(object keyboardController)
    {
        Dictionary<ConsoleKey, Action> keyMappings = RegisterKeyboardController(keyboardController);
        
        Console.WriteLine("Listening for keypresses. Press 'Escape' to exit.");

        await Task.Run(() =>
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                if (keyMappings.TryGetValue(keyInfo.Key, out var action))
                {
                    action.Invoke();
                }
            }
        });
    }
}