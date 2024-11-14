using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[ConsoleController]
public class ProductController
{

    private readonly ProductManager _manager;

    public ProductController(ProductManager manager)
    {
        _manager = manager;
    }

}