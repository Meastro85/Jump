using Jump.Attributes;
using Jump.Attributes.Components;

namespace JumpDemo.Configurations;

/// <summary>
/// This class has a small example of how to use the <see cref="ConfigurationProperties"/> attribute
/// </summary>
[Configuration]
[ConfigurationProperties(prefix: "HelloWorld")]
public class HelloWorldConfiguration
{
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
}