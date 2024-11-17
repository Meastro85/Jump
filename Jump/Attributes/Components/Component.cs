namespace Jump.Attributes.Components;

/// <summary>
/// Attribute <c>Component</c> is used to mark a class as a component.
/// This is the base attribute for all components.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Component : Attribute;