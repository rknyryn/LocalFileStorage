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

**BaseFolderPath** specifies the root directory where uploaded files will be stored within the file system. All uploaded files will be organized relative to this base folder.

**GlobalFileExtensionFilter** is a setting that defines a global filter for file extensions. It determines which file types are allowed to be uploaded and stored within the system. Only files with extensions specified in this filter will be accepted.

## Usage

### Example Response

```json
{
  "fileName": "combinepdf.pdf",
  "fileNameGenerated": "20240311_e67a0_combinepdf-912.pdf",
  "filePath": "uploaded/files/documents/20240311_e67a0_combinepdf-912.pdf",
  "fullPath": "/Users/kaanyarayan/Projects/CookieAuthenticationDemo/uploaded/files/documents/20240311_e67a0_combinepdf-912.pdf",
  "contentType": "application/pdf"
}
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
    public FileUploadResult UploadFile(IFormFile uploadedFile)
    {
        FileUploadResult fileUploadResult = _fileStorage.UploadFile(file: uploadedFile, directoryPath: "documents");
        // FileUploadResult fileUploadResult = fileStorage.UploadFile(file: uploadedFile, directoryPath: "documents", extensionFilter: [".jpg"]);
        // FileUploadResult fileUploadResult = fileStorage.UploadFile(file: uploadedFile, directoryPath: "documents", extensionFilter: [".jpg"], includeGlobalFileExtensionFilter: false);

        return fileUploadResult;
    }

    [HttpGet("downloadFile")]
    public FileStreamResult DownloadFile(string filePath)
    {
        var stream = _fileStorage.GetFile(filePath: filePath);

        return new FileStreamResult(stream, "application/octet-stream")
        {
            FileDownloadName = Path.GetFileName(filePath),
            EnableRangeProcessing = true
        };
    }

    [HttpPost("deleteFile")]
    public string DeleteFile(string filePath)
    {
        _fileStorage.DeleteFile(filePath: filePath);

        return filePath;
    }
}
```

## Feedback

Local File Storage is released as open source under the [MIT license](https://github.com/rknyryn/LocalFileStorage/blob/main/LICENSE). Bug reports and contributions are welcome at the [GitHub repository](https://github.com/rknyryn/LocalFileStorage.git).
