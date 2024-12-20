namespace Jump.Exceptions;

/// <summary>
/// This exception is throw when there is a cyclic dependency detected.
/// A cyclic dependency happens when class A depends on class B, and class B depends on class A.
/// This will cause an infinite loop.
/// </summary>
public class CyclicDependencyException(string message): Exception(message);