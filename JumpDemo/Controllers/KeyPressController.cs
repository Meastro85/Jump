using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;

namespace JumpDemo.Controllers;

[KeyboardController]
public class KeyPressController
{

    [KeyAction(ConsoleKey.A)]
    public void OnKeyA()
    {
        Console.WriteLine("Key A pressed");
    }
}