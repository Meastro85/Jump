using Jump.Attributes;
using Jump.Attributes.Actions;
using Jump.Attributes.Actions.Http;
using Jump.Attributes.Components.Controllers;
using Jump.Http_response;
using JumpDemo.Domain;
using JumpDemo.Managers;
using JumpDemo.Repositories;

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

    [Route("/warehouse", HttpAction.Get)]
    public IResponse GetWarehouses()
    {
        var warehouses = _manager.GetWarehouses();
        return new JsonResponse<IEnumerable<Warehouse>>(warehouses);
    }

    [Route("/warehouse/{id}", HttpAction.Get)]
    public IResponse GetWarehouseById(int id)
    {
        var warehouse = _manager.GetWarehouseById(id);
        return new JsonResponse<Warehouse>(warehouse);
    }

    [Route("/warehouse/{id}/{name}/{address}", HttpAction.Get)]
    public IResponse CreateWarehouse(int id, string name, string address)
    {
        var warehouse = _manager.CreateWarehouse(id, name, address);
        return new JsonResponse<Warehouse>(warehouse, 201);
    }
}