using Jump.Attributes;
using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;
using JumpDemo.Repositories;

namespace JumpDemo.Controllers;

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
    public void GetWarehouses()
    {
        var warehouses = _manager.GetWarehouses();
        foreach (var warehouse in warehouses)
        {
            Console.Write($"Warehouse: {warehouse.Name}\n" +
                          $"Address: {warehouse.Address}\n");
        }
    }
    
    [Route("/warehouse/{id}")]
    public void GetWarehouseById(int id)
    {
        var warehouse = _manager.GetWarehouseById(id);
        Console.Write($"Warehouse: {warehouse.Name}\n" +
                      $"Address: {warehouse.Address}\n");
    }

    [Route("/warehouse/{id}/{name}/{address}")]
    public void CreateWarehouse(int id, string name, string address)
    {
        var warehouse = _manager.CreateWarehouse(id, name, address);
        Console.Write($"Created warehouse successfully: {warehouse.Id}\n" +
                      $"Warehouse: {warehouse.Name}\n" +
                      $"Address: {warehouse.Address}\n");
    }
    
}