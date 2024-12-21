using Jump.Attributes;
using Jump.Attributes.Components;
using JumpDemo.Domain;

namespace JumpDemo.Configurations;

[Configuration]
public class HopTestConfiguration
{
    [Hop]
    public static Product GetProduct()
    {
        return new Product("Hopped Product", 15);
    }
}