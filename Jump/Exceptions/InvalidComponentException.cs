namespace Jump.Exceptions;

/// <summary>
/// Exception <c>InvalidComponentException</c> is thrown when a component is not valid.
/// </summary>
/// <param name="message">The reason why the component is not valid</param>
public class InvalidComponentException(string message) : Exception(message);