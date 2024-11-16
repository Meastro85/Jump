namespace Jump.Attributes.Actions;

[AttributeUsage(AttributeTargets.Method)]
public class KeyAction(ConsoleKey key) : Attribute
{
    public ConsoleKey Key { get; } = key;
}