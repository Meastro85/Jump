using Jump.Attributes.Components;
using JumpDemo.Domain;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

[Service]
public class WarehouseManager(WarehouseRepo repo)
{
    public IEnumerable<Warehouse> GetWarehouses()
    {
        return repo.ReadWarehouses();
    }

    public Warehouse GetWarehouseById(int id)
    {
        return repo.ReadWarehouseById(id);
    }

    public Warehouse CreateWarehouse(int id, string name, string address)
    {
        var warehouse = new Warehouse(id, name, address, []);
        repo.AddWarehouse(warehouse);
        return warehouse;
    }
    
}