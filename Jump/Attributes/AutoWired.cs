namespace Jump.Attributes;

/// <summary>
/// Attribute <c>AutoWired</c> is used to mark a constructor as being automatically wired.
/// This is for when there's multiple valid constructors.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor)]
public class AutoWired : Attribute;