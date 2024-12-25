using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Configurations;

namespace JumpDemo.Controllers;

/// <summary>
/// This class is an example of the <see cref="KeyboardController"/> combined with its corresponding <see cref="KeyAction"/> attributes.
/// </summary>
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