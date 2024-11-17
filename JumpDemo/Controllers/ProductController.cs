using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[RestController]
public class ProductController(ProductManager manager)
{
    [Route("/warehouse/{id}/products")]
    public void GetProductsInWarehouse(int id)
    {
        var products = manager.ReadProductsInWarehouse(id).ToList();
        if(products.Count == 0) Console.WriteLine("No products found");
        foreach (var product in products)
        {
            Console.Write($"Product: {product.Name}\n" +
                          $"Quantity: {product.Quantity}\n");
        }
    }
    
}