using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[ConsoleController]
public class WarehouseController
{

    private readonly WarehouseManager _manager;

    public WarehouseController(WarehouseManager manager)
    {
        _manager = manager;
    }
    
}