using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Domain;

namespace JumpDemo.Controllers;

[KeyboardController]
public class SecondKeyPressController
{
    private readonly Product _hopProduct;
    
    public SecondKeyPressController(Product hopProduct)
    {
        _hopProduct = hopProduct;
    }

    [KeyAction(ConsoleKey.B)]
    public void OnKeyB()
    {
        Console.WriteLine("Key B pressed");
        Console.WriteLine(_hopProduct.Name + " " + _hopProduct.Quantity);
    }
}