using Jump.Attributes.Cache;
using Jump.Attributes.Components;
using JumpDemo.Domain;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

[Service]
public class WarehouseManager(WarehouseRepo repo)
{
    [Cacheable("Warehouses")]
    public virtual IEnumerable<Warehouse> GetWarehouses()
    {
        return repo.ReadWarehouses();
    }

    [Cacheable("Warehouses", "id")]
    public virtual Warehouse GetWarehouseById(int id)
    {
        return repo.ReadWarehouseById(id);
    }

    [CacheEvict("Warehouses")]
    public virtual Warehouse CreateWarehouse(int id, string name, string address)
    {
        var warehouse = new Warehouse(id, name, address, []);
        repo.AddWarehouse(warehouse);
        return warehouse;
    }
}