using System.Reflection;
using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using Jump.Exceptions;
using QuikGraph;
using QuikGraph.Algorithms;

namespace Jump.Providers.Component_store;

internal static class ComponentValidator
{
    private static readonly BidirectionalGraph<Type, Edge<Type>> DependencyGraph = new();
    private static readonly object GraphLock = new();

    internal static void ValidateComponent(Type component, Type componentType)
    {
        if (componentType != typeof(Service)) CheckCacheAttributes(component);
    }

    private static void CheckCacheAttributes(Type component)
    {
        var methodAttributes = component.GetMethods()
            .SelectMany(m => m.GetCustomAttributes(false));
        if (methodAttributes.Any(attr => attr is Cacheable or CacheEvict))
            throw new InvalidComponentException(
                $"Cacheable or CacheEvict attributes are not allowed on component: {component.Name} since it's not a service.");
    }

    internal static void CreateGraph(Type component, ParameterInfo[] dependencies)
    {
        lock (GraphLock)
        {
            DependencyGraph.AddVertex(component);
            foreach (var dependency in dependencies.Select(p => p.ParameterType))
            {
                DependencyGraph.AddVertex(dependency);
                DependencyGraph.AddEdge(new Edge<Type>(component, dependency));
            }

            if (!DependencyGraph.IsDirectedAcyclicGraph())
                throw new CyclicDependencyException("Circular dependency detected in component: " + component.Name);
        }
    }
}