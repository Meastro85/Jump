namespace JumpDemo.Domain;

public class Warehouse(long id, string name, string address, ICollection<Product> products)
{
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Address { get; set; } = address;
    public ICollection<Product> Products { get; set; } = products;
}