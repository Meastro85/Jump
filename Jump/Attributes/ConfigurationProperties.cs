namespace Jump.Attributes;

/// <summary>
///     Attribute <c>ConfigurationProperties</c> is used to add a prefix to all properties in a configuration class.
/// </summary>
/// <param name="prefix">The prefix for your properties in jump.properties</param>
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationProperties(string prefix) : Attribute
{
    public string Prefix { get; } = prefix;
}