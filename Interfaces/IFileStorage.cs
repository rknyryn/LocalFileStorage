using LocalFileStorage.Models;
using Microsoft.AspNetCore.Http;

namespace LocalFileStorage.Interfaces;

public interface IFileStorage
{
    /// <summary>
    /// Uploads a file to the specified directory.
    /// </summary>
    /// <param name="file">The <see cref="IFormFile"/> instance representing the file to be uploaded.</param>
    /// <param name="directoryPath">The path to upload the file to.</param>
    /// <returns>Returns a <see cref="FileUploadResult"/> indicating the outcome of the file upload operation.</returns>
    FileUploadResult UploadFile(IFormFile file, string directoryPath);

    /// <summary>
    /// Uploads a file to the specified directory with optional file extension filtering.
    /// </summary>
    /// <param name="file">The <see cref="IFormFile"/> instance representing the file to be uploaded.</param>
    /// <param name="directoryPath">The path to upload the file to.</param>
    /// <param name="extensionFilter">An array of file extensions that are allowed to be uploaded.</param>
    /// <param name="includeGlobalFileExtensionFilter">
    ///     Indicates whether to include the global file extension filter settings defined at application level.
    ///     If set to true (default), both the global file extension filter and the extensions provided in the 'extensionFilter' parameter will be enforced.
    ///     If set to false, only the extensions provided in the 'extensionFilter' parameter will be considered.
    /// </param>
    /// <returns>Returns a <see cref="FileUploadResult"/> indicating the outcome of the file upload operation.</returns>
    FileUploadResult UploadFile(IFormFile file, string directoryPath, string[] extensionFilter,
        bool includeGlobalFileExtensionFilter = true);


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