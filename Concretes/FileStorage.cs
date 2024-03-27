using LocalFileStorage.Helpers;
using LocalFileStorage.Interfaces;
using LocalFileStorage.Middlewares;
using LocalFileStorage.Models;
using Microsoft.AspNetCore.Http;

namespace LocalFileStorage.Concretes;

public class FileStorage : IFileStorage
{
    readonly FileStorageOptions _fileStorageOptions;

    public FileStorage(FileStorageOptions fileStorageOptions)
    {
        _fileStorageOptions = fileStorageOptions;
    }

    public FileUploadResult UploadFile(IFormFile file, string directoryPath)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        ArgumentNullException.ThrowIfNull(directoryPath, nameof(directoryPath));

        if (_fileStorageOptions.GlobalFileExtensionFilter is not null)
        {
            FileStorageHelpers.CheckFileExtension(Path.GetExtension(file.FileName).ToLower(),
                _fileStorageOptions.GlobalFileExtensionFilter);
        }

        return Upload(file, directoryPath);
    }

    public FileUploadResult UploadFile(IFormFile file, string directoryPath, string[] extensionFilter,
        bool includeGlobalFileExtensionFilter = true)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        ArgumentNullException.ThrowIfNull(directoryPath, nameof(directoryPath));

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (_fileStorageOptions.GlobalFileExtensionFilter is not null && includeGlobalFileExtensionFilter)
        {
            extensionFilter = extensionFilter.Union(_fileStorageOptions.GlobalFileExtensionFilter).ToArray();
        }

        FileStorageHelpers.CheckFileExtension(fileExtension, extensionFilter);

        return Upload(file, directoryPath);
    }

    public void DeleteFile(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        if (File.Exists(filePath) is false) throw new FileNotFoundException();

        File.Delete(filePath);
    }

    public Stream GetFile(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        if (File.Exists(filePath) is false) throw new FileNotFoundException();

        return new FileStream(filePath, FileMode.Open);
    }
    
    private FileUploadResult Upload(IFormFile file, string directoryPath)
    {
        var combinedDirectoryPath = string.IsNullOrEmpty(_fileStorageOptions.BaseFolderPath)
            ? directoryPath
            : Path.Combine(_fileStorageOptions.BaseFolderPath, directoryPath);
        var uploadDirectoryPath = Path.Combine(Environment.CurrentDirectory, combinedDirectoryPath);

        if (!File.Exists(uploadDirectoryPath)) Directory.CreateDirectory(uploadDirectoryPath);

        var fileNameGenerated = FileStorageHelpers.GenerateFileName(file.FileName);
        var fullPath = Path.Combine(uploadDirectoryPath, fileNameGenerated);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var filePath = fullPath.Replace(Environment.CurrentDirectory, "").Remove(0, 1);

        return new FileUploadResult(fileName: file.FileName,
            filePath: filePath,
            fullPath: fullPath,
            contentType: file.ContentType,
            fileNameGenerated: fileNameGenerated);
    }
}