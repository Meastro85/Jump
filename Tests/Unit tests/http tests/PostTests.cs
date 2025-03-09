using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Jump.Util;
using Newtonsoft.Json;
using Tests.Domains.Http_test_domain;

namespace Tests.Unit_tests.http_tests;

public partial class PostTests
{

    private MethodInfo _testMethod;
    private Match _testMatch;
    private TestClass _testObject;
    
    [SetUp]
    public void SetUp()
    {
        
        _testObject = new TestClass
        {
            Name = "John",
            Message = "Hello",
            Id = 1
        };
    }

    [Test]
    public async Task CreateObjectFromJsonTest()
    {
        _testMethod = typeof(TestController).GetMethod("CreateObjectFromBody")!;
        
        var regex = RouteRegex();
        _testMatch = regex.Match("/test");
        
        var jsonBody = JsonConvert.SerializeObject(_testObject);
        const string contentType = "application/json";
        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody));

        var result = await RouteFunctions.ParseParametersForTesting(_testMethod, _testMatch, bodyStream, contentType);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(((TestClass)result[0]!).Name, Is.EqualTo("John"));
            Assert.That(((TestClass)result[0]!).Message, Is.EqualTo("Hello"));
            Assert.That(((TestClass)result[0]!).Id, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task CreateObjectFromBodyWithExtraParamTest()
    {
        _testMethod = typeof(TestController).GetMethod("CreateObjectFromBody")!;
        
        var regex = RouteMultiParamRegex();
        _testMatch = regex.Match("/test/3");
        
        var jsonBody = JsonConvert.SerializeObject(_testObject);
        const string contentType = "application/json";
        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonBody));

        var result = await RouteFunctions.ParseParametersForTesting(_testMethod, _testMatch, bodyStream, contentType);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(((TestClass)result[0]!).Name, Is.EqualTo("John"));
            Assert.That(((TestClass)result[0]!).Message, Is.EqualTo("Hello"));
            Assert.That(((TestClass)result[0]!).Id, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task CreateObjectFromFormUrlEncodedTest()
    {
        _testMethod = typeof(TestController).GetMethod("CreateObjectFromBody")!;
        
        var regex = RouteRegex();
        _testMatch = regex.Match("/test");
        
        const string body = "name=John&message=Hello&id=1";
        const string contentType = "application/x-www-form-urlencoded";
        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        
        var result = await RouteFunctions.ParseParametersForTesting(_testMethod, _testMatch, bodyStream, contentType);
        
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(((TestClass)result[0]!).Name, Is.EqualTo("John"));
            Assert.That(((TestClass)result[0]!).Message, Is.EqualTo("Hello"));
            Assert.That(((TestClass)result[0]!).Id, Is.EqualTo(1));
        });
    }
    
    [GeneratedRegex(@"^\/test$")]
    private static partial Regex RouteRegex();

    [GeneratedRegex(@"/test/(?<param1>\w+)$")]
    private static partial Regex RouteMultiParamRegex();

}