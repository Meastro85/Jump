using Jump;
using Jump.Attributes.Actions;
using Jump.Attributes.Components.Controllers;
using Jump.Providers;
using JumpDemo.Domain;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

[RestController]
public class ProductController(ProductManager manager)
{
    [Route("/warehouse/{id}/products")]
    public IJsonResponse GetProductsInWarehouse(int id)
    {
        var products = manager.ReadProductsInWarehouse(id).ToList();
        return new JsonResponse<List<Product>>(products);
    }
    
}