using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;

namespace JumpDemo.Controllers;

[KeyboardController]
public class KeyPressController
{

    [KeyAction(ConsoleKey.A)]
    public void onKeyA()
    {
        Console.WriteLine("Key A pressed");
    }

    [KeyAction(ConsoleKey.Escape)]
    public void onEscape()
    {
        Console.WriteLine("Exiting appliction...");
        Environment.Exit(0);
    }
}