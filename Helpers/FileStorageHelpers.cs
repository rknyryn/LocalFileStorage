using LocalFileStorage.Constants;
using LocalFileStorage.Exceptions;
using LocalFileStorage.Extensions;

namespace LocalFileStorage.Helpers;

public static class FileStorageHelpers
{
    /// <summary>
    /// Check if the file extension is valid.
    /// </summary>
    /// <param name="extensionFilter">Valid file extensions. If it is null, it will use the default file extensions.</param>
    /// <param name="fileExtension">File extension to check.</param>
    public static void CheckFileExtension(string fileExtension, string[]? extensionFilter = null)
    {
        if (extensionFilter is null)
        {
            if (FileStorageConstants.VALID_FILE_EXTENSIONS.Contains(fileExtension) is false)
            {
                throw new InvalidFileExtensionException();
            }
        }
        else
        {
            if (extensionFilter.Contains(fileExtension) is false)
            {
                throw new InvalidFileExtensionException();
            }
        }
    }

    /// <summary>
    /// Generate a new file name with the current date and a random string.
    /// </summary>
    /// <param name="fileName">Original file name.</param>
    /// <returns>Generated file name.</returns>
    public static string GenerateFileName(string fileName)
    {
        string fileExtension = Path.GetExtension(fileName);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        DateTime currentDate = DateTime.UtcNow;
        string year = currentDate.Year.ToString();
        string month = currentDate.Month.ToString().PadLeft(2, '0');
        string day = currentDate.Day.ToString().PadLeft(2, '0');
        string generatedFileName = $"{year}{month}{day}_{Guid.NewGuid().ToString()[..5]}_{fileNameWithoutExtension.Slugify()}{fileExtension.ToLower()}";

        return generatedFileName;
    }
}
