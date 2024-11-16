using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[RestController]
public class ProductController(ProductManager manager)
{

    private readonly ProductManager _manager = manager;
}