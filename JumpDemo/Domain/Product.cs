namespace JumpDemo.Domain;

public class Product(string name, int quantity)
{
    public string Name { get; set; } = name;
    public int Quantity { get; set; } = quantity;
}