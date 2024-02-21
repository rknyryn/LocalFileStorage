using System.Runtime.Serialization;

namespace LocalFileStorage.Exceptions;

public class InvalidFileExtensionException : Exception
{
    public InvalidFileExtensionException()
    {
    }

    public InvalidFileExtensionException(string? message) : base(message)
    {
    }

    public InvalidFileExtensionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidFileExtensionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
