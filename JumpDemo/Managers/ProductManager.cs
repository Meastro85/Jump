using Jump.Attributes.Components;
using JumpDemo.Domain;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

/// <summary>
/// This class is a small example of a <see cref="Service"/>
/// </summary>
/// <param name="repo"></param>
[Service]
public class ProductManager(WarehouseRepo repo)
{
    public IEnumerable<Product> ReadProductsInWarehouse(int id)
    {
        return repo.ReadProductsInWarehouse(id);
    }
    
}