namespace Jump.Attributes.Actions;

/// <summary>
/// Attribute <c>Route</c> is used to map a method to a path.
/// </summary>
/// <param name="path">The path to listen to in the URL</param>
[AttributeUsage(AttributeTargets.Method)]
public class Route(string path) : Attribute
{
    public String Path { get; } = path;
}