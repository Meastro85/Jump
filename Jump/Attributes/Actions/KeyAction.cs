namespace Jump.Attributes.Actions;

/// <summary>
/// Attribute <c>KeyAction</c> is used to mark a method as a key action.
/// </summary>
/// <param name="key">The key to listen for in the console</param>
[AttributeUsage(AttributeTargets.Method)]
public class KeyAction(ConsoleKey key) : Attribute
{
    public ConsoleKey Key { get; } = key;
}