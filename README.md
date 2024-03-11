# Local File Storage

Local File Storage is a NuGet package developed with [.NET](https://dotnet.microsoft.com/en-us/download). This package basically performs file writing and deletion operations on the local disk. Developers can use this package for file operations in their small or medium-sized projects.

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

or you can configure

```csharp
builder.Services.AddFileStorageServices(options =>
{
    options.BaseFolderPath = Path.Combine("uploaded", "files");
    options.GlobalFileExtensionFilter = [".pdf", ".png"];
});
```

## Usage

### Example Response

```json
{
  "fileName": "combinepdf.pdf",
  "fileNameGenerated": "20240311_e67a0_combinepdf-912.pdf",
  "filePath": "/Users/kaanyarayan/Projects/CookieAuthenticationDemo/uploaded/files/documents/20240311_e67a0_combinepdf-912.pdf",
  "contentType": "application/pdf"
}
```

### Minimal API Example

```csharp
app.MapPost("/uploadFile", (IFormFile file, [FromServices] IFileStorage fileStorage) =>
{
    FileUploadResult fileUploadResult = fileStorage.UploadFile(file, "documents");
    // FileUploadResult fileUploadResult = fileStorage.UploadFile(file, "documents", [".jpg", ".png"]);

    return fileUploadResult;
});

app.MapGet("/downloadFile", (string filePath, [FromServices] IFileStorage fileStorage) =>
{
    var stream = fileStorage.GetFile(filePath);

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
    public FileUploadResult UploadFile(IFormFile file)
    {
        FileUploadResult fileUploadResult = _fileStorage.UploadFile(file, "documents");
        // FileUploadResult fileUploadResult = fileStorage.UploadFile(file, "documents", [".jpg", ".png"]);

        return fileUploadResult;
    }

    [HttpGet("downloadFile")]
    public FileStreamResult DownloadFile(string filePath)
    {
        var stream = _fileStorage.GetFile(filePath);

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

Local File Storage is released as open source under the [MIT license](https://github.com/rknyryn/LocalFileStorage/blob/main/LICENSE). Bug reports and contributions are welcome at the [GitHub repository](https://github.com/rknyryn/LocalFileStorage.git).
