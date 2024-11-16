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
    
}