# .NET DI Container - JUMP

JUMP is an attribute based dependency injection container for .NET based on Spring from Java.

## Using JUMP
### Getting started

To start using this framework, simply add this to your program.cs:

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

## Running and compiling

### Compiling

You can compile the framework by using:
````
dotnet build
````

### Demo application
You can run the demo application by using:
````
dotnet run --project JumpDemo
````

### Tests

You can run the tests by using:
````
dotnet test
````
in the terminal.

## Plans for development

This was a school project for programming 3 at KdG.
Since I found this a fun and interesting project I plan on expanding on it in the future.

The current plans are:

### High priority
- [ ] Add support for repositories
- [ ] Add support for databases

### Medium priority
- [ ] Expand on the documentation
- [ ] Expand the Http Calls to allow classic CRUD operations

### Low priority
- [ ] More interception mechanics

Created by Vincent Verboven