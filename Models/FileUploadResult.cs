namespace LocalFileStorage.Models;

public class FileUploadResult
{
    public FileUploadResult(string fileName, string filePath, string contentType, string fileNameGenerated, string fullPath)
    {
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        FileNameGenerated = fileNameGenerated;
        FullPath = fullPath;
    }

    public string FileName { get; }
    public string FileNameGenerated { get; }
    public string FilePath { get; }
    public string FullPath { get; }
    public string ContentType { get; }
}
