using LocalFileStorage.Concretes;
using LocalFileStorage.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LocalFileStorage.Middlewares;

public static class FileStorageMiddlewareExtension
{
    public static IServiceCollection AddFileStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, FileStorage>();

        return services;
    }

}
