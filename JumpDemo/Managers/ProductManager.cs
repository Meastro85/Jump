using Jump.Attributes.Components;
using JumpDemo.Domain;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

[Service]
public class ProductManager(WarehouseRepo repo)
{
    public IEnumerable<Product> ReadProductsInWarehouse(int id)
    {
        return repo.ReadProductsInWarehouse(id);
    }
    
}