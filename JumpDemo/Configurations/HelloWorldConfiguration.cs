using Jump.Attributes;
using Jump.Attributes.Components;

namespace JumpDemo.Configurations;

[Configuration]
[ConfigurationProperties(prefix: "HelloWorld")]
public class HelloWorldConfiguration
{
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
}