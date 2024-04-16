using LocalFileStorage.Helpers;
using LocalFileStorage.Interfaces;
using LocalFileStorage.Middlewares;
using LocalFileStorage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace LocalFileStorage.Concretes;

public class FileStorage : IFileStorage
{
    private readonly FileStorageOptions _fileStorageOptions;

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
        CheckFilePathExists(filePath);
        File.Delete(filePath);
    }

    public Stream GetFile(string filePath)
    {
        CheckFilePathExists(filePath);
        return new FileStream(filePath, FileMode.Open);
    }

    public void CopyFile(string srcFilePath, string destDirectoryPath)
    {
        ArgumentNullException.ThrowIfNull(destDirectoryPath, nameof(destDirectoryPath));
        CheckFilePathExists(srcFilePath);

        var destDirectoryFullPath = Path.Combine(Environment.CurrentDirectory, destDirectoryPath);
        if (Directory.Exists(destDirectoryFullPath) is false)
        {
            Directory.CreateDirectory(destDirectoryFullPath);
        }

        var fileName = Path.GetFileName(srcFilePath);
        var copyFilePath = Path.Combine(destDirectoryPath, fileName);

        File.Copy(srcFilePath, copyFilePath);
    }

    public void MoveFile(string srcFilePath, string destDirectoryPath)
    {
        ArgumentNullException.ThrowIfNull(destDirectoryPath, nameof(destDirectoryPath));
        CheckFilePathExists(srcFilePath);

        var fileName = Path.GetFileName(srcFilePath);
        var moveFilePath = Path.Combine(destDirectoryPath, fileName);

        File.Move(srcFilePath, moveFilePath);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    #region Private Methods

    private FileUploadResult Upload(IFormFile file, string directoryPath)
    {
        var combinedDirectoryPath = string.IsNullOrEmpty(_fileStorageOptions.BaseFolderPath)
            ? directoryPath
            : Path.Combine(_fileStorageOptions.BaseFolderPath, directoryPath);
        var uploadDirectoryPath = Path.Combine(Environment.CurrentDirectory, combinedDirectoryPath);

        if (Directory.Exists(uploadDirectoryPath) is false)
        {
            Directory.CreateDirectory(uploadDirectoryPath);
        }

        var fileNameGenerated = _fileStorageOptions.GenerateFileName ? 
            FileStorageHelpers.GenerateFileName(file.FileName)
            : file.FileName;
        var fullPath = Path.Combine(uploadDirectoryPath, fileNameGenerated);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        var filePath = fullPath.Replace(Environment.CurrentDirectory, "").Remove(0, 1);

        return new FileUploadResult(
            fileName: file.FileName,
            filePath: filePath,
            fullPath: fullPath,
            contentType: GetMimeType(file.FileName),
            fileNameGenerated: fileNameGenerated);
    }

    private static string GetMimeType(string fileName)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }
        
        return contentType;            
    }
    
    private static void CheckFilePathExists(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
        
        if (File.Exists(filePath) is false)
        {
            throw new FileNotFoundException();
        }
    }

    #endregion
}