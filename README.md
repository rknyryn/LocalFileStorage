# Local File Storage

Local File Storage is a NuGet package developed with [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). This package basically performs file writing and deletion operations on the local disk. Developers can use this package for file operations in their small or medium-sized projects.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

## Getting started

### Install the package

Install the Local File Storage library for .NET.
Using the NuGet package manager console within Visual Studio run the following command:

```
Install-Package rknyryn.LocalFileStorage
```

Or using the .net core CLI from a terminal window:

```
dotnet add package rknyryn.LocalFileStorage
```

### Configuration

You can get IFileStorage instance via dependency injection, you need add to services.

```csharp
builder.Services.AddFileStorageServices();
```

## Usage

### Minimal API Example

```csharp
app.MapPost("/uploadFile", (IFormFile file, [FromServices] IFileStorage fileStorage) =>
{
    var filePath = fileStorage.UploadFile(file, "uploads");
    // var filePath = fileStorage.UploadFile(file, "uploads", [".jpg", ".png"]);

    return filePath;
});

app.MapGet("/downloadFile", (string filePath, [FromServices] IFileStorage fileStorage) =>
{
    var stream = fileStorage.DownloadFile(filePath);

    return new FileStreamResult(stream, "application/octet-stream")
    {
        FileDownloadName = Path.GetFileName(filePath),
        EnableRangeProcessing = true
    };
});

app.MapPost("/deleteFile", (string filePath, [FromServices] IFileStorage fileStorage) =>
{
    fileStorage.DeleteFile(filePath);

    return filePath;
});
```

### Controller-based API Example

```csharp
[ApiController]
[Route("[controller]")]
public class FileStorageController : ControllerBase
{
    private readonly IFileStorage _fileStorage;

    public FileStorageController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpPost("uploadFile")]
    public string UploadFile(IFormFile file)
    {
        var filePath = _fileStorage.UploadFile(file, "uploads");
        // var filePath = fileStorage.UploadFile(file, "uploads", [".jpg", ".png"]);

        return filePath;
    }

    [HttpGet("downloadFile")]
    public FileStreamResult DownloadFile(string filePath)
    {
        var stream = _fileStorage.DownloadFile(filePath);

        return new FileStreamResult(stream, "application/octet-stream")
        {
            FileDownloadName = Path.GetFileName(filePath),
            EnableRangeProcessing = true
        };
    }

    [HttpPost("deleteFile")]
    public string DeleteFile(string filePath)
    {
        _fileStorage.DeleteFile(filePath);

        return filePath;
    }
}
```

## Feedback

Local File Storage is released as open source under the [MIT license](./LICENSE). Bug reports and contributions are welcome at the [GitHub repository](https://github.com/rknyryn/LocalFileStorage.git).
