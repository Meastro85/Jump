namespace Jump.Attributes;

/// <summary>
///     For use in a <c>Configuration</c> class,
///     this attribute is used to mark a method as a 'configured' component.
///     Methods annotated with this attribute ALWAYS need to have a return value of a complex object.
///     They should also always be marked as static.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Hop : Attribute;