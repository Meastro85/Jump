namespace Jump.Attributes.Components.Controllers;

/// <summary>
/// Attribute <c>Controller</c> is the base attribute for all controllers.
/// Using this attribute will register the attribute as a controller, but won't add any listeners.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public abstract class Controller : Component;