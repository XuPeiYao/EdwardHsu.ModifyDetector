EdwardHsu.ModifyDetector
=====
[![Unit Test](https://github.com/XuPeiYao/EdwardHsu.ModifyDetector/actions/workflows/unit-test.yaml/badge.svg)](https://github.com/XuPeiYao/EdwardHsu.ModifyDetector/actions/workflows/unit-test.yaml) [![NuGet Version](https://img.shields.io/nuget/v/EdwardHsu.ModifyDetector.svg)](#) [![NuGet Download](https://img.shields.io/nuget/dt/EdwardHsu.ModifyDetector.svg)](https://www.nuget.org/packages/EdwardHsu.ModifyDetector/) [![Github license](https://img.shields.io/github/license/XuPeiYao/EdwardHsu.ModifyDetector.svg)](#)

This library is a .NET library designed to facilitate property modification detection within your classes. It provides a simple and efficient way to implement property change detection within your own classes.

## Features
* Property Modification Detection: Detect modifications to properties within your classes.
* Modified Property List Retrieval: Easily retrieve a list of properties that have been modified.

## Getting Started
To get started with ModifyDetector, follow these simple steps:

1. **Install the ModifyDetector package:** You can install the ModifyDetector library via NuGet Package Manager or NuGet CLI:

```shell
dotnet add package EdwardHsu.ModifyDetector
```

2. **Inherit from ModifyDetector class:** In the class where you want to implement modification detection, inherit from the `ModifyDetector` class.

```csharp
using EdwardHsu.ModifyDetector;

public class YourClass : ModifyDetector
{
    // Your class implementation
}
```

3. **Add ModifyDetectTargetAttribute attribute:** Mark the properties or fields you want to monitor for modifications with the `ModifyDetectTargetAttribute` attribute.

```csharp
using EdwardHsu.ModifyDetector;

public class YourClass : ModifyDetector
{
    [ModifyDetectTarget(Order = 0)]
    public Guid Id { get; set; } 

    [ModifyDetectTarget(Order = 1)]
    public string Name { get; set; }
}
```

4. **Initialize Modification Detection:** After setting initial values in your class constructor, call the `UpdateDetectorState()` method to initialize modification detection.

```csharp
public class YourClass : ModifyDetector
{
    public YourClass()
    {
        Id = Guid.NewGuid();
        Name = "Untitled";

        UpdateDetectorState();
    }

    [ModifyDetectTarget(Order = 0)]
    public Guid Id { get; set; } 

    [ModifyDetectTarget(Order = 1)]
    public string Name { get; set; }
}
```

5. Access modification information: You can access information about property modifications using the `HasModified(out IEnumerable<ModifiedMember> modifiedMembers)` method.

```csharp
var instance = new YourClass();
// Modify properties...
var hasModified = instance.HasModified(out var modifiedProperties);
```

### Example

```csharp
using EdwardHsu.ModifyDetector;

public class YourClass : ModifyDetector
{
    public YourClass()
    {
        Id = Guid.NewGuid();
        Name = "Untitled";

        UpdateDetectorState();
    }

    [ModifyDetectTarget(Order = 0)]
    public Guid Id { get; set; } 

    [ModifyDetectTarget(Order = 1)]
    public string Name { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        var instance = new YourClass();
        instance.Name = "New Name";

        var hasModified = instance.HasModified(out var modifiedProperties);

        // ...Do something...
    }
}
```

## License
This library is licensed under the [MIT License](./LICENSE).

## Contribution
Contributions are welcome! Feel free to submit issues, feature requests, or pull requests.
