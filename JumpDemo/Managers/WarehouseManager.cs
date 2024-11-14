using Jump.Attributes;
using Jump.Attributes.Components;
using JumpDemo.Repositories;

namespace JumpDemo.Managers;

[Service]
public class WarehouseManager
{

    private readonly WarehouseRepo _repo;

    public WarehouseManager(WarehouseRepo repo)
    {
        _repo = repo;
    }
    
}