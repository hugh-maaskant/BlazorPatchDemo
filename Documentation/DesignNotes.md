# BlazorPatchDemo Design Notes

## Introduction

BlazorPatchDemo is a very simple demo App to show how to use RESTfull HTTP operations in ASP.NET Core with a hosted Blazor WASM application. The emphasis is on the use of the PATCH operation.

The App provides a minimal UI to manipulate (a List of) Items that are maintained in a MongoDB database on the server.
The definition of the Items, and large parts of the Server's Controler logic, are re-used from Julio Casal's excellent "[Building Microservices With .NET](https://dotnetmicroservices.com/)" course.
They mimic an inventory of Game attributes, with a Name, Description, and a Price as the main user-relevant properties.

The Blazor UI provides a simple CRUD capabilty, respectively using using HTTP POST, GET, PUT/PATCH, and DELETE operations.

#### Create - POST

### Limitations

This demo has a couple of limitations, mostly to avoid complicating the code too much. In a real production app, some changes are recommended as described later in the document.

#### Generic Typed `HttpClient`

The demo uses a named [`HttpClient`](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-7.0) and wraps that in a `SafeItemClient`, which provides the HTTP operation for the `Item` type.
In production this should be replaced by a typed HttpClient which is wrapped by a generc (for all Entity types) "`ApiClient`", e.g.

```csharp
<T> where T : IEntity
public class ApiClient<TEntity> : where TEntity : IEntity
{
    private readonly HttpClient _client;
        
    protected ApiClient(HttpClient client)
    { 
        _client = client;
    }
    
    // All required Operations, e.g.
    
    protected async Task<ApiResult<TEntity>> GetAsync(string path, CancellationToken token = default)
    { ... }
}
```

And then for each Entity Type

```
public class ItemClient : ApiClient<Item>
{
    private readonly string _path;
    
    public ItemClient(HttpClient client) : base(client)
    {
        _path = client.BaseAddress?.AbsolutePath ?? string.Empty;
    }
    
}

```

#### Replace `XxxAsJson` 

For convenience, some of the HTTP requests generated from the Client use the `XxxAsJson` convenience methods. 
This gives very little control over the request.
Instead, build the request using the [`HttpRequestMessage`](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httprequestmessage?view=net-7.0) type and use async processing for the response headers and body as need.

### Error injection

To simulate connection and server errors, and also to test Client error handling, 
the Server will randomly (p = 20%) return an error in stead of performing the requested operation.

This can be switched off in the Controller's `FailRandomly()` method:

```csharp
private static readonly Random StaticRandom = new Random();
private const int MaxRandom = 5;

private static void FailRandomly()
{
    if (StaticRandom.Next(MaxRandom) == 0) 
        throw new Exception("Random server fail (used for testing only)");
}
```
Replace this with

```csharp
private static void FailRandomly() {}
```
Or update the `MaxRandom` value to change the odds.
