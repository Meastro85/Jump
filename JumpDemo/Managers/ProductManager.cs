using Jump.Attributes;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

[Service]
public class ProductManager
{

    private readonly WarehouseRepo _repo;

    public ProductManager(WarehouseRepo repo)
    {
        _repo = repo;
    }

}