using Jump.Attributes.Components;
using JumpDemo.Domain;

namespace JumpDemo.Repositories;

[Repository]
public class WarehouseRepo
{
    
    private readonly Warehouse[] _warehouses = [
        new(1, "Warehouse Global Wineries", "Julesplein 175b, 3231, Rijkhovenzele, Brussel, Belgium",
        [
          new Product("Red wine", 35),
          new Product("White wine", 10),
          new Product("Sparkling wine", 50)
        ]),
        new(2, "Warehouse woodworks", "Margotweg 54, 6999, Beverst, Oost-Vlaanderen, Belgium",
            [
                new Product("Oak door", 3),
                new Product("Birch chair", 6)
            ]),
        new(3, "Warehouse American Candy", "Decosterlaan 82, 0391, Sint-Katelijne-Wavertem, Oost-Vlaanderen, Belgium",
            [
                new Product("Reese's Peanut Butter Cups", 2500),
                new Product("Twizzlers", 3765),
                new Product("Hershey's Milk Chocolate Bar", 580)
            ])
    ];

    public IEnumerable<Warehouse> ReadWarehouses()
    {
        return _warehouses;
    }

    public Warehouse ReadWarehouseById(int id)
    {
        return _warehouses.First(warehouse => warehouse.Id == id);
    }

    public IEnumerable<Product> ReadProductsInWarehouse(int id)
    {
        return _warehouses.First(warehouse => warehouse.Id == id).Products;
    }
    
}