namespace Jump.Attributes.Components;

/// <summary>
/// Attribute <c>Singleton</c> is used to mark a class as a singleton.
/// This will ensure that only one instance of the class is created, and used in all needed injections.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Singleton : Attribute;