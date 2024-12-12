using Jump.Attributes.Actions;
using Jump.Attributes.Cache;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Configurations;

namespace JumpDemo.Controllers;

[KeyboardController]
public class KeyPressController
{
    
    private readonly HelloWorldConfiguration _configuration;

    public KeyPressController(HelloWorldConfiguration configuration)
    {
        _configuration = configuration;
    }
    

    [KeyAction(ConsoleKey.A)]
    public void OnKeyA()
    {
        Console.WriteLine("Key A pressed");
        Console.WriteLine(_configuration.Prefix + " " + _configuration.Suffix);
    }
}