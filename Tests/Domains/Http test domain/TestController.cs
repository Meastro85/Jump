using Jump.Attributes.Actions.Http;
using Jump.Attributes.Components.Controllers;
using Jump.Attributes.Parameters;
using Jump.Http_response;

namespace Tests.Domains.Http_test_domain;

[RestController]
public class TestController
{
    [HttpPost("/test")]
    public JsonResponse<TestClass> CreateObjectFromBody([BodyParam] TestClass testClass)
    {
        return new JsonResponse<TestClass>(testClass);
    }

    [HttpPost("/test/{3}")]
    public JsonResponse<TestClass> CreateObjectFromBodyWithExtraParameter([BodyParam] TestClass testClass, int id)
    {
        return new JsonResponse<TestClass>(testClass);
    }

}