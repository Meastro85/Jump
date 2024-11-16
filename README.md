# .NET DI Container - JUMP

JUMP is an attribute based dependency injection container for .NET.

## Using JUMP
### Getting started

To start using this framework, simply add:

```csharp
using Jump;

await JumpApplication.Run(typeof(Program))
```
This will scan all components starting from the root and start the application.

To define a component that you want to save for later injection, add the component attribute:
```csharp
[Component]
public class MyCustomComponent
{
    public string Name { get; set; } = "John";
}
```

### Creating controllers

To create a controller you need to add the corresponding controller attribute:

For example, a REST controller:
```csharp
[RestController]
public class MyController
{
    public MyService Service { get; set; }
}
```

Services and repositories can be created in the same way, using the Service and Repository attribute.
```csharp
[Service]
public class MyService
{
    public MyRepository Repository { get; set; }
}

[Repository]
public class MyRepository
{
}
```

Created by Vincent Verboven