using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[RestController]
public class WarehouseController(WarehouseManager manager)
{
    [Route("/warehouse")]
    public void GetWarehouses()
    {
        var warehouses = manager.GetWarehouses();
        foreach (var warehouse in warehouses)
        {
            Console.Write($"Warehouse: {warehouse.Name}\n" +
                          $"Address: {warehouse.Address}\n");
        }
    }
    
    [Route("/warehouse/{id}")]
    public void GetWarehouseById(int id)
    {
        var warehouse = manager.GetWarehouseById(id);
        Console.Write($"Warehouse: {warehouse.Name}\n" +
                      $"Address: {warehouse.Address}\n");
    }

    [Route("/warehouse/{id}/{name}/{address}")]
    public void CreateWarehouse(int id, string name, string address)
    {
        var warehouse = manager.CreateWarehouse(id, name, address);
        Console.Write($"Created warehouse successfully: {warehouse.Id}\n" +
                      $"Warehouse: {warehouse.Name}\n" +
                      $"Address: {warehouse.Address}\n");
    }
    
}