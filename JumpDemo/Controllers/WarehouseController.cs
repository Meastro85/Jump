using Jump.Attributes;
using Jump.Attributes.Actions.Http;
using Jump.Attributes.Components.Controllers;
using Jump.Attributes.Parameters;
using Jump.Http_response;
using JumpDemo.Domain;
using JumpDemo.Managers;
using JumpDemo.Repositories;
using JumpDemo.Requests;

namespace JumpDemo.Controllers;

/// <summary>
///     This class is a simple example of a <see cref="RestController" />.
///     It also includes an example of primary constructor injection through <see cref="AutoWired" />.
/// </summary>
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

    [HttpGet("/warehouse")]
    public IResponse GetWarehouses()
    {
        var warehouses = _manager.GetWarehouses();
        return new JsonResponse<IEnumerable<Warehouse>>(warehouses);
    }

    [HttpGet("/warehouse/{id}")]
    public IResponse GetWarehouseById(int id)
    {
        var warehouse = _manager.GetWarehouseById(id);
        return new JsonResponse<Warehouse>(warehouse);
    }

    [HttpPost("/warehouse/{id}/{name}/{address}")]
    public IResponse CreateWarehouse(int id, string name, string address)
    {
        var warehouse = _manager.CreateWarehouse(id, name, address);
        return new JsonResponse<Warehouse>(warehouse, 201);
    }

    [HttpPost("/warehouse")]
    public IResponse CreateWarehouse([BodyParam] WarehouseRequest warehouseRequest)
    {
        var warehouse = _manager.CreateWarehouse(warehouseRequest.Id, warehouseRequest.Name, warehouseRequest.Address);
        return new JsonResponse<Warehouse>(warehouse, 201);
    }
}