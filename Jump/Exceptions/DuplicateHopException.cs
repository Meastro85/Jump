namespace Jump.Exceptions;

/// <summary>
/// Exception <c>DuplicateHopException</c> is thrown when a hop is added twice.
/// </summary>
/// <param name="message">The error message</param>
public class DuplicateHopException(string message) : Exception(message);