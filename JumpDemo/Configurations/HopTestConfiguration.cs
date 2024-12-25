using Jump.Attributes;
using Jump.Attributes.Components;
using JumpDemo.Domain;

namespace JumpDemo.Configurations;

/// <summary>
/// This class has a small example on how to use the <see cref="Hop"/> attribute
/// </summary>
[Configuration]
public class HopTestConfiguration
{
    [Hop]
    public static Product GetProduct()
    {
        return new Product("Hopped Product", 15);
    }
}