namespace Jump.Attributes.Actions;

[AttributeUsage(AttributeTargets.Method)]
public class Route(string path) : Attribute
{
    public String Path { get; } = path;
}