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

    public FileUploadResult UploadFile(IFormFile file, string path)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        if (_fileStorageOptions.GlobalFileExtensionFilter is not null)
        {
            FileStorageHelpers.CheckFileExtension(Path.GetExtension(file.FileName).ToLower(),
                _fileStorageOptions.GlobalFileExtensionFilter);
        }

        string combinedPath = string.IsNullOrEmpty(_fileStorageOptions.BaseFolderPath)
            ? path
            : Path.Combine(_fileStorageOptions.BaseFolderPath, path);
        string uploadPath = Path.Combine(Environment.CurrentDirectory, combinedPath);

        if (!File.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string fileNameGenerated = FileStorageHelpers.GenerateFileName(file.FileName);
        string uploadFilePath = Path.Combine(uploadPath, fileNameGenerated);

        using (var stream = new FileStream(uploadFilePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return new FileUploadResult(fileName: file.FileName,
            filePath: uploadFilePath,
            contentType: file.ContentType,
            fileNameGenerated: fileNameGenerated);
    }

    public FileUploadResult UploadFile(IFormFile file, string path, string[] extensionFilter)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (_fileStorageOptions.GlobalFileExtensionFilter is not null)
        {
            extensionFilter = extensionFilter.Union(_fileStorageOptions.GlobalFileExtensionFilter).ToArray();
        }

        FileStorageHelpers.CheckFileExtension(fileExtension, extensionFilter);

        return UploadFile(file, path);
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
}