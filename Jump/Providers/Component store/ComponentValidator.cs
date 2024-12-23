using System.Reflection;
using Jump.Attributes;
using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using Jump.Exceptions;
using QuikGraph;
using QuikGraph.Algorithms;

namespace Jump.Providers.Component_store;

internal class ComponentValidator
{
    private readonly BidirectionalGraph<Type, Edge<Type>> _dependencyGraph = new();
    private readonly object _graphLock = new();

    internal static void ValidateComponent(Type component, Type componentType)
    {
        if (componentType != typeof(Service)) CheckCacheAttributes(component);
        if (componentType != typeof(Configuration)) CheckCloudAttributes(component);
    }

    private static void CheckCacheAttributes(Type component)
    {
        var methodAttributes = component.GetMethods()
            .SelectMany(m => m.GetCustomAttributes(false));
        if (methodAttributes.Any(attr => attr is Cacheable or CacheEvict))
            throw new InvalidComponentException(
                $"Cacheable or CacheEvict attributes are not allowed on component: {component.Name} since it's not a service.");
    }

    private static void CheckCloudAttributes(Type component)
    {
        var methodAttributes = component.GetMethods().SelectMany(m => m.GetCustomAttributes(false));
        if (methodAttributes.Any(attr => attr is Hop))
            throw new InvalidComponentException(
                $"Cloud attributes are not allowed on component: {component.Name} since it's not a configuration.");
    }

    internal void CreateGraph(Type component, ParameterInfo[] dependencies)
    {
        lock (_graphLock)
        {
            _dependencyGraph.AddVertex(component);
            foreach (var dependency in dependencies.Select(p => p.ParameterType))
            {
                _dependencyGraph.AddVertex(dependency);
                _dependencyGraph.AddEdge(new Edge<Type>(component, dependency));
            }

            if (!_dependencyGraph.IsDirectedAcyclicGraph())
                throw new CyclicDependencyException("Circular dependency detected in component: " + component.Name);
        }
    }
}