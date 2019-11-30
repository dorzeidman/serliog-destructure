# serliog-ttransformer [![Build status](https://ci.appveyor.com/api/projects/status/lypgm2unll1a16fk/branch/master?svg=true)]
Serilog T Generic Transformer 

This package allows to transform object logged in Serilog. You can ignore, mask, rename or write your custom transformer.
The package is written to support clean architecture, separating class creation from how it is written.

### Installation

Install from NuGet:

```powershell
Install-Package SerliogTTransformer
```

Modify logger configuration:

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b. ...)
  ...
```

### How to use:

```csharp

public class User
{
  public string Username { get; set; }
  public string Password { get; set; }
  public string Phone { get; set; }
}

```

#### Ignoring a property

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b.Ignore(u => u.Password))
  ...
 
```

#### Masking entire property with Xs

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b.Ignore(u => u.Password, 'X'))
  ...
 
```

#### Masking Property - partial chars, show first 3 chars and last 3 chars

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b.Ignore(u => u.Phone, 3, 3))
  ...
 
```

#### Rename Property

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b.Rename(u => u.Username, "Name"))
  ...
 
```

#### Custom Transformer - MyCustomerTranformer implements: IPropertyTransformer

```csharp
var log = new LoggerConfiguration()
	.AddTTransformer()
	.Destructure.Transform<User>(b => b.Transform(u => u.Username, new MyCustomerTranformer()))
  ...
 
```



