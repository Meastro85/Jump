using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;

namespace JumpDemo.Controllers;

[KeyboardController]
public class SecondKeyPressController
{
    
    [KeyAction(ConsoleKey.B)]
    public void OnKeyB()
    {
        Console.WriteLine("Key B pressed");
    }
    
}