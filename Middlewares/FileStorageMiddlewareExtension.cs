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
        services.AddScoped<IFileStorage, FileStorage>();

        return services;
    }

    public static IServiceCollection AddFileStorageServices(this IServiceCollection services, Action<FileStorageOptions> configureOptions)
    {
        var options = new FileStorageOptions();
        configureOptions(options);

        services.AddSingleton(options);
        return AddFileStorageServices(services);
    }
}