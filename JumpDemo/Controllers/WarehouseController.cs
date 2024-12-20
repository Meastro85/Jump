using Jump.Attributes;
using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using Jump.Http_response;
using JumpDemo.Domain;
using JumpDemo.Managers;
using JumpDemo.Repositories;

namespace JumpDemo.Controllers;

// Example of primary constructor usage.
[RestController]
public class WarehouseController
{
    private readonly WarehouseManager _manager;

    [AutoWired]
    public WarehouseController(WarehouseManager manager)
    {
        _manager = manager;
    }

    public WarehouseController(WarehouseManager newManager, WarehouseRepo repo)
    {
        _manager = newManager;
    }

    [Route("/warehouse")]
    public IJsonResponse GetWarehouses()
    {
        var warehouses = _manager.GetWarehouses();
        return new JsonResponse<IEnumerable<Warehouse>>(warehouses);
    }

    [Route("/warehouse/{id}")]
    public IJsonResponse GetWarehouseById(int id)
    {
        var warehouse = _manager.GetWarehouseById(id);
        return new JsonResponse<Warehouse>(warehouse);
    }

    [Route("/warehouse/{id}/{name}/{address}")]
    public IJsonResponse CreateWarehouse(int id, string name, string address)
    {
        var warehouse = _manager.CreateWarehouse(id, name, address);
        return new JsonResponse<Warehouse>(warehouse, 201);
    }
}