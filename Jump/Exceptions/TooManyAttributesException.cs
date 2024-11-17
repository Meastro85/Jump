namespace Jump.Exceptions;

/// <summary>
/// Exception <c>TooManyAttributesException</c> is thrown when a component has more than one component attribute.
/// </summary>
/// <param name="message">The error message</param>
public class TooManyAttributesException(string message) : Exception(message);