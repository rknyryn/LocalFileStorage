using LocalFileStorage.Helpers;
using LocalFileStorage.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LocalFileStorage.Concretes;

public class FileStorage : IFileStorage
{
    public string UploadFile(IFormFile file, string path, string[]? extensionFilter = null)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        FileStorageHelpers.CheckFileExtension(fileExtension, extensionFilter);

        string uploadPath = Path.Combine(Environment.CurrentDirectory, path);

        if (!File.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

        string fileName = FileStorageHelpers.GenerateFileName(file.FileName);
        string uploadFilePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(uploadFilePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return uploadFilePath.Replace(Path.Combine(Environment.CurrentDirectory), "");
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
