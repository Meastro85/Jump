namespace Jump.Attributes.Actions;

[AttributeUsage(AttributeTargets.Method)]
public class KeyAction : Attribute
{
    public ConsoleKey Key { get; }

    public KeyAction(ConsoleKey key)
    {
        Key = key;
    }
}