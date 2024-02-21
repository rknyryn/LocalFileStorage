using Microsoft.AspNetCore.Http;

namespace LocalFileStorage.Interfaces;

public interface IFileStorage
{
    /// <summary>
    /// This method is used to upload a file to the file storage.
    /// </summary>
    /// <param name="file">The file to be uploaded.</param>
    /// <param name="path">The path to upload the file to.</param>
    /// <param name="extensionFilter">The file extensions that are allowed to be uploaded.</param>
    /// <returns>The URL of the uploaded file.</returns>
    string UploadFile(IFormFile file, string path, string[]? extensionFilter = null);

    /// <summary>
    /// This method is used to delete a file from the file storage.
    /// </summary>
    /// <param name="filePath">The path of the file to be deleted.</param>
    void DeleteFile(string filePath);

    /// <summary>
    /// This method is used to get a file from the file storage.
    /// </summary>
    /// <param name="filePath">The path of the file to be retrieved.</param>
    /// <returns>The file stream of the file.</returns>
    Stream GetFile(string filePath);
}
