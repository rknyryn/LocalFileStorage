using LocalFileStorage.Concretes;
using LocalFileStorage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LocalFileStorage.Middlewares;

public class FileStorageOptions
{
    public string? BaseFolderPath { get; set; }
    public string[]? GlobalFileExtensionFilter { get; set; }
}

public static class FileStorageMiddlewareExtension
{
    public static IServiceCollection AddFileStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileStorage, FileStorage>();

        return services;
    }

    public static IServiceCollection AddFileStorageServices(this IServiceCollection services,
        Action<FileStorageOptions> configureOptions)
    {
        var options = new FileStorageOptions();
        configureOptions(options);

        if (options.BaseFolderPath is not null && Path.IsPathRooted(options.BaseFolderPath))
        {
            throw new Exception("Base folder path cannot starts with root!");
        }

        services.AddSingleton(options);

        return AddFileStorageServices(services);
    }
}