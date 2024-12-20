using System.Reflection;
using Jump.Attributes;
using Jump.Exceptions;
using Jump.LoggingSetup;

namespace Jump.Providers.Component_store;

internal static class ConstructorSearch
{
    internal static ConstructorInfo GetMainConstructor(Type type)
    {
        var constructors = type.GetConstructors();

        var autowiredConstructors = GetAutoWiredConstructors(constructors);
        if (autowiredConstructors.Count > 1)
            throw new AmbiguousMatchException($"Type {type.Name} has multiple autowired constructors.");

        var sortedConstructors = SortConstructorsByParameterCount(constructors);
        if (HasAmbiguousConstructor(sortedConstructors))
            throw new AmbiguousMatchException(
                $"Type {type.Name} has multiple constructors with the same parameter count.");

        if (autowiredConstructors.Count == 1) return autowiredConstructors[0];
        var constructor = sortedConstructors.FirstOrDefault();

        if (constructor != null) return constructor;

        var exc = new InvalidComponentException("No valid constructors found for " + type.Name);
        Logging.Logger.LogError("No valid constructors found for " + type.Name, exc);
        throw exc;
    }

    private static List<ConstructorInfo> GetAutoWiredConstructors(ConstructorInfo[] constructors)
    {
        return constructors
            .Where(c => c.GetCustomAttribute(typeof(AutoWired)) != null)
            .ToList();
    }

    private static List<ConstructorInfo> SortConstructorsByParameterCount(ConstructorInfo[] constructors)
    {
        return constructors
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();
    }

    private static bool HasAmbiguousConstructor(List<ConstructorInfo> sortedConstructors)
    {
        return sortedConstructors.Count > 1 &&
               sortedConstructors[0].GetParameters().Length == sortedConstructors[1].GetParameters().Length;
    }
}