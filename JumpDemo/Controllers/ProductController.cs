using Jump.Attributes.Actions;
using Jump.Attributes.Actions.Http;
using Jump.Attributes.Components.Controllers;
using Jump.Http_response;
using JumpDemo.Domain;
using JumpDemo.Managers;

namespace JumpDemo.Controllers;

/// <summary>
///     This class is a simple example of a <see cref="RestController" />
/// </summary>
[RestController]
public class ProductController(ProductManager manager)
{
    [Route("/warehouse/{id}/products", HttpAction.Get)]
    public IResponse GetProductsInWarehouse(int id)
    {
        var products = manager.ReadProductsInWarehouse(id).ToList();
        return new JsonResponse<List<Product>>(products);
    }
    
}