namespace Jump.Attributes.Components;

/// <summary>
/// Attribute <c>Configuration</c> is used to mark a class as a configuration.
/// Configuration classes are by default a singleton and should be annotated with the <see cref="ConfigurationProperties"/> attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Configuration : Component;